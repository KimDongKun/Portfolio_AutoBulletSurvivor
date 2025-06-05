using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject itemPrefab;
    public GameObject magnetPrefab;
    public GameObject bulletPrefab;
    public Transform fireStart;

    public AudioSource attackSound;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask playerLayer;
    public float attackLate = 1f;
    public float attackTimer;
    public Image HP_bar;

    private Camera followCam;
    private GameObject player;
    private float distance;
    private float fullHP;
    private Animator anim;
    private bool getCritcal = false;

    public float HP = 100;
    public float AttackDamage = 10;
    public float speed = 4f;

    public bool isShooterEnemy = false;
    [SerializeField]
    private EnemyUIManager enemyUIManager;

    private SettingManager settingManager;
    // Start is called before the first frame update
    void Start()
    {
        getCritcal = false;
        player = GameObject.Find("Player_Sprite");
        followCam = Camera.main;
        anim = this.transform.GetComponent<Animator>();
        fullHP = HP;

        settingManager = GameObject.FindObjectOfType<SettingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<AudioSource>().volume = settingManager.effectUI.value * 0.5f;

        //if (!GameStageManager.isGamePause)
        {
            if (player.transform.position.x > this.transform.position.x)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }

            distance = Vector3.Distance(player.transform.position, attackPoint.transform.position);
            if (distance > attackRange && !isShooterEnemy && PlayerCtrl.isAlive)//근접 공격 유닛(적)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * speed);
                //Debug.Log("추적중:"+ distance);
                anim.SetInteger("Animate", 2);
            }
            else if(distance <= attackRange && !isShooterEnemy && PlayerCtrl.isAlive)
            {
                if (attackTimer >= attackLate)
                {
                    if (!isShooterEnemy)
                        Attack();

                    attackTimer = 0;
                }
                else
                {
                    attackTimer += Time.deltaTime;

                }
            }

            if (distance < attackRange && isShooterEnemy && PlayerCtrl.isAlive)//원거리 공격 유닛(적)
            {
                if (attackTimer >= attackLate)
                {
                    Attack_Shoot();
                    attackTimer = 0;
                }
                else
                {
                    attackTimer += Time.deltaTime;
                }
                anim.SetInteger("Animate", 0);
            }
            else if(isShooterEnemy && PlayerCtrl.isAlive)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * speed);
                anim.SetInteger("Animate", 2);
            }

        }

    }

    void Attack()
    {
        Debug.Log("공격시작");
        float _range = attackRange + 5;
        if (isShooterEnemy)
        {
            _range = attackRange + 5;
        }
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, _range, playerLayer);

        foreach(Collider2D player in hitPlayer)
        {
            Debug.Log("playerName"+player.name);
            
                if (player != null && PlayerCtrl.isAlive)
                {
                    player.GetComponent<PlayerCtrl>().GetAttackDamage(AttackDamage);
                    attackSound.Play();
                   
                }
            
           
        }
    }
    void GetPlayerAttack(float _damage)
    {
        float damageRange = Random.Range(_damage - 5f, _damage + 5);

        HP -= damageRange;
        HP_bar.fillAmount = HP / fullHP;

        StartCoroutine(HitScan());
        enemyUIManager.ActiveDamageText(damageRange, getCritcal);

        getCritcal = false;
        if (HP <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameStageManager>().killScore += 1;

            // int _random = Random.Range(0, 100);
            // GameObject[] skillBoxValue = GameObject.FindGameObjectsWithTag("SkillBox");
            /* if (skillBoxValue.Length < 5 && _random > 80)
             {
                 Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
             }
             else*/
            // Instantiate(coinPrefab, this.transform.position, Quaternion.identity);

            int _random = Random.Range(0, 100);
            if (_random < 95)
            {//95%확률
                Instantiate(coinPrefab, this.transform.position, Quaternion.identity);
            }
            else if (_random == 95)
            {
                Debug.Log("Magnet! " + _random);
                Instantiate(magnetPrefab, this.transform.position, Quaternion.identity);
            }
            else
            {//5%확률
                Debug.Log("Item! : "+_random);
                Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
                //GameObject.Find("EnemySpawnManager").GetComponent<DropItemManager>().__Random_DropItem(this.transform.position);
            }

            Destroy(this.gameObject);
        }
    }
    void Attack_Shoot()
    {
        Vector3 playerPos = player.transform.position;
        Vector2 lookDir = (playerPos - this.transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        fireStart.eulerAngles = new Vector3(0, 0, angle);

        Shoot();
    }

    void Shoot()
    {
        //반동 (아래 주석처리)
        //mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 0.5f, mainCam.transform.position.z);

        //gunSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, fireStart.position, fireStart.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(fireStart.up * 10, ForceMode2D.Impulse);//20

        bullet.GetComponent<EnemyBulletCtrl>().enemyBullet_Damage = AttackDamage;
    }
    IEnumerator HitScan()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(255,0,0,255);
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    public void ExplosionDamage(float _damage)
    {
        GetPlayerAttack(_damage);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //Debug.Log("총알 맞음");
            this.GetComponent<Rigidbody2D>().AddForce(this.transform.position - col.gameObject.transform.position);

            float _damage = followCam.GetComponent<FollowCamera>().damage;
            float _critcal = followCam.GetComponent<FollowCamera>().criticalValue;
            //Debug.Log(col.gameObject.name);

            int random = Random.Range(0, 100);
            if (random < _critcal)
            {
                getCritcal = true;
                _damage = _damage * 2;
            }
            Destroy(col.gameObject);

            GetPlayerAttack(_damage);
        }
    
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //Debug.Log("총알 맞음");
            this.GetComponent<Rigidbody2D>().AddForce(this.transform.position - col.gameObject.transform.position);

            float _damage = followCam.GetComponent<FollowCamera>().damage*5;
            float _critcal = followCam.GetComponent<FollowCamera>().criticalValue;
            //Debug.Log(col.gameObject.name);

            int random = Random.Range(0, 100);
            if (random < _critcal)
            {
                getCritcal = true;
                _damage = _damage * 2;
            }
            
            GetPlayerAttack(_damage);
        }

    }
}
