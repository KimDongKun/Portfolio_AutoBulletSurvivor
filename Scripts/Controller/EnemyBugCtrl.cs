using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBugCtrl : MonoBehaviour
{
    public GameObject coinPrefab;

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

    private bool isJumpAttack = false;

    Vector3 playerPos;

    [SerializeField]
    private EnemyUIManager enemyUIManager;
    // Start is called before the first frame update
    void Start()
    {
        isJumpAttack = false;
        getCritcal = false;
        player = GameObject.Find("Player_Sprite");
        followCam = Camera.main;
        anim = this.transform.GetComponent<Animator>();
        fullHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
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

            distance = Vector3.Distance(player.transform.position, this.transform.position);
            

            if (distance < attackRange && PlayerCtrl.isAlive && !isJumpAttack)//도약 공격 유닛(곱등이)
            {
                //Debug.Log("플레이어 도약");
                anim.SetInteger("Animate", 0);
                if (attackTimer >= attackLate)
                {
                    isJumpAttack = true;
                    StartCoroutine(Attack_Jump());
                    attackTimer = 0;
                }
                else
                {
                    attackTimer += Time.deltaTime;
                }
                
            }
            else if (PlayerCtrl.isAlive && !isJumpAttack)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime * speed);
                anim.SetInteger("Animate", 1);
            }

        }

    }

    void Attack()
    {
        float _range = 3f;


        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, _range, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            //Debug.Log("playerName" + player.name);

            if (player != null && PlayerCtrl.isAlive)
            {
                player.GetComponent<PlayerCtrl>().GetAttackDamage(AttackDamage/10);
                //attackSound.Play();
                if(!attackSound.isPlaying) attackSound.Play();
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

            //int _random = Random.Range(0, 100);
            // GameObject[] skillBoxValue = GameObject.FindGameObjectsWithTag("SkillBox");
            /* if (skillBoxValue.Length < 5 && _random > 80)
             {
                 Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
             }
             else*/
            Instantiate(coinPrefab, this.transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
    IEnumerator Attack_Jump()
    {
        playerPos = player.transform.position;
        //Debug.LogError(this.transform.position - playerPos);
        anim.SetInteger("Animate", 2);
        
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("Animate", 3);
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        rb.AddForce((this.transform.position-playerPos) * -10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isJumpAttack = false;
       
        
    }

   
    IEnumerator HitScan()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
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
