using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHiveCtrl : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject bulletPrefab;
    public GameObject deadPaticle;
    public Transform fireStart;

    public float attackRange = 1f;
    public LayerMask playerLayer;
    public float attackLate = 1f;
    public float attackTimer;
    public Image HP_bar;
    public GameObject objectImage;

    private Camera followCam;
    private GameObject player;
    private float distance;
    private float fullHP;
    private bool getCritcal = false;

    public float HP = 100;
    public float AttackDamage = 10;
    public float aliveTime= 0;

    [SerializeField]
    private EnemyUIManager enemyUIManager;
    // Start is called before the first frame update
    void Start()
    {
        getCritcal = false;
        player = GameObject.Find("Player_Sprite");
        followCam = Camera.main;

        HP += GameStageManager.hiveStack * 500f;
        fullHP = HP;

        AttackDamage += GameStageManager.hiveStack * 5f;
        aliveTime = 0;
    }
    void Attack_Shoot()
    {
        Vector3 playerPos = player.transform.position;
        Vector2 lookDir = playerPos - this.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        fireStart.eulerAngles = new Vector3(0, 0, angle);

        Shoot();
    }

    void Shoot()
    {
       
        GameObject bullet = Instantiate(bulletPrefab, fireStart.position, fireStart.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        float speed = (int)(aliveTime / 10) * 10;
        if(speed == 0)
        {
            speed = 10;
        }
        rb.AddForce(fireStart.up * speed, ForceMode2D.Impulse);//20

        bullet.GetComponent<EnemyBulletCtrl>().enemyBullet_Damage = AttackDamage;
    }
    public void ExplosionDamage(float _damage)
    {
        GetPlayerAttack(_damage);
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
            Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
            Instantiate(deadPaticle, this.transform.position, Quaternion.identity);
            GameStageManager.hiveStack += 1;
            Destroy(this.gameObject);
        }
    }
    IEnumerator HitScan()
    {
        objectImage.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.2f);
        objectImage.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < attackRange && PlayerCtrl.isAlive)//원거리 공격 유닛(적)
        {
            //Debug.Log("플레이어 포착");
            if (attackTimer >= attackLate)
            {
                Attack_Shoot();
                attackTimer = 0;
            }
            else
            {
                attackTimer += Time.deltaTime;
            }
        }

        aliveTime += Time.deltaTime;
        if (attackLate > 0.3f)
        {
            attackLate = 1 - (aliveTime / 100);
        }
        
        //Debug.Log(attackLate);
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

            float _damage = followCam.GetComponent<FollowCamera>().damage * 5;
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
