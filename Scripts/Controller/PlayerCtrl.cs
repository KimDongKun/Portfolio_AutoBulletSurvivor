using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 공격력  lv.0 : 0.15
/// 공속    lv.0 : 0.15
/// 치명타  lv.0 : 0.15
/// 이동속도lv.0 : 0.15
/// 체력    lv.0 : 0.15
/// </summary>
public class PlayerCtrl : MonoBehaviour
{
    public static bool isAlive = true;
    public static bool isSkillMaster = false;
    public static bool isMagnetActive = false;

    public static int getCoinValue = 0;
    public static int cost_Stats;

    public LayerMask coinLayer;

    public GameObject playerParticle;
    public GameObject playerLevelUpParticle;
    public GameObject deadMark;
    public GameObject weapon;
    public Image player_HPimage;
    public Text player_HPtext;
    public Image player_Dash;

    public SaveJSonData playerLevel;
    public FollowCamera followCam;
    public GameObject player;
    
    public float player_HP = 100f;
    public float player_MoveSpeed = 10f;
    public float player_Defence = 0f;
    public float dash_CoolTime = 5f;

    public GameObject marker_Hive_UI;
    public GameObject escapeTimer_UI;
    public Text coinText;
    public Text costStatsText;

    private bool usingDash = true;
    
    private float playerFull_HP;
    private Animator anim;
    private GameStageManager manager;
    private AudioLimit audioLimit;

    private int level_Damage = 0;
    private int level_Firelate = 0;
    private int level_Critical = 0;
    private int level_Movement = 0;
    private int level_HP = 0;

    bool isMaster_Damage = false;
    bool isMaster_Firelate = false;
    bool isMaster_Critical = false;
    bool isMaster_Movement = false;
    bool isMaster_HP = false;

    private float origin_Damage = 0;
    private float origin_Firelate = 0;
    private float origin_Critical = 0;
    private float origin_Movement = 0;
    private float origin_HP = 0;

    private int level_Defence = 0;

    public LayerMask skillBoxLayer;
    public List<Collider2D> trigger_BoxList = new List<Collider2D>();
    public void GetEscapeTimer(string timer, bool escaping)
    {
        escapeTimer_UI.SetActive(escaping);
        escapeTimer_UI.transform.GetChild(0).GetComponent<Text>().text = timer;
    }
    public void GetAttackDamage(float damage)
    {
        //player_Defence = Mathf.Clamp(player_Defence, 0, 19);//임시공식
        //damage -= player_Defence;
        StartCoroutine(HitScan());

        player_HP -= damage;
        player_HPimage.fillAmount = player_HP / playerFull_HP;
        //player_HPtext.text = (player_HPimage.fillAmount * 100).ToString("F0")+"/"+ playerFull_HP.ToString("F0");// 백분율 출력
        player_HPtext.text = player_HP.ToString("F0") + "/" + playerFull_HP.ToString("F0");

        if (player_HP <= 0 && isAlive)
        {
            Debug.LogError("Player is DEAD");
            isAlive = false;
            PlayerDead();
        }
    }
    IEnumerator HitScan()
    {
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
    public void Skill_UP_IncreaseHP()
    {
        if(PayforStats(isMaster_HP))
            if (level_HP >= 10)
            {
                isMaster_HP = true;
            }
            else
            {
                level_HP += 1;
                float LvUP_HP = origin_HP * (level_HP * 0.1f);

                player_HP += (origin_HP * 0.1f);
                playerFull_HP = origin_HP + LvUP_HP;

                player_HPtext.text = player_HP.ToString("F0") + "/" + playerFull_HP.ToString("F0");
                player_HPimage.fillAmount = player_HP / playerFull_HP;
                manager.lv_HP_Text.text = playerFull_HP.ToString("F0");
                manager.level_HP.text = "LV." + level_HP;
                if (level_HP >= 10)
                {
                    isMaster_HP = true;
                    SkillMasterBool();
                }
            }
    }
    public void Skill_UP_Damage()
    {
        if(PayforStats(isMaster_Damage))
            if (level_Damage >= 10)
            {
                isMaster_Damage = true;
            }
            else
            {
                level_Damage += 1;
                float LvUP_Damage = origin_Damage * (level_Damage * 0.1f);

                followCam.damage = origin_Damage + LvUP_Damage;
                manager.lv_Damage_Text.text = followCam.damage.ToString("F1");
                manager.level_Damage.text = "LV." + level_Damage;
                if (level_Damage >= 10)
                {
                    isMaster_Damage = true;
                    SkillMasterBool();
                }

            }
    }
    public void Skill_UP_Firelate()
    {
        if(PayforStats(isMaster_Firelate))
            if (level_Firelate >= 10)
            {
                isMaster_Firelate = true;
            }
            else
            {
                level_Firelate += 1;
                float LvUP_Firelate = origin_Firelate * (level_Firelate * 0.05f);

                followCam.fireLate = origin_Firelate - LvUP_Firelate;
                manager.lv_FireLate_Text.text = followCam.fireLate.ToString("F2");
                manager.level_FireLate.text = "LV." + level_Firelate;
                if (level_Firelate >= 10)
                {
                    isMaster_Firelate = true;
                    SkillMasterBool();
                }
            }
    }
    public void Skill_UP_Critical()
    {
        if(PayforStats(isMaster_Critical))
        if (level_Critical >= 10)
        {
            isMaster_Critical = true;
        }
        else
        {
            level_Critical += 1;
            followCam.criticalValue += 4;
            manager.lv_Critical_Text.text = followCam.criticalValue.ToString("F1")+"%";
            manager.level_Critical.text = "LV." + level_Critical;
                if (level_Critical >= 10)
                {
                    isMaster_Critical = true;
                    SkillMasterBool();
                }

            }
    }
    public void Skill_UP_Movement()
    {
        if (PayforStats(isMaster_Movement))
            if (level_Movement >= 10)
            {
                isMaster_Movement = true;
            }
            else
            {
                level_Movement += 1;
                float LvUP_Movement = origin_Movement * (level_Movement * 0.1f);

                player_MoveSpeed = origin_Movement + LvUP_Movement;
                manager.lv_MoveSpeed_Text.text = player_MoveSpeed.ToString("F1");
                manager.level_MoveSpeed.text = "LV." + level_Movement;
                if (level_Movement >= 10)
                {
                    isMaster_Movement = true;
                    SkillMasterBool();
                }
                //manager.lv_MoveSpeed_Text.GetComponentInChildren<Text>().text = count_Movement.ToString();
            }
    }
    public void Skill_UP_Defence()// not Use this.
    {
        level_Defence += 1;
        player_Defence += 1;
        //manager.lv_Defence_Text.text = "LV." + count_Defence.ToString();
    }
    public void SkillMasterBool()
    {
        if(isMaster_Movement && isMaster_Critical&& isMaster_Firelate&& isMaster_Damage&& isMaster_HP)
        {
            isSkillMaster = true;
        }
        else
        {
            isSkillMaster = false;
        }
    }

    private void First_Load_PlayerLevel()
    {
        List<int> PlayerLevel = playerLevel.playerStateData;

        float data_HP = PlayerLevel[0] * 25f;
        player_HP += data_HP;
        playerFull_HP += data_HP;

        followCam.damage += PlayerLevel[1] * 10f;
        player_MoveSpeed += PlayerLevel[2] * 0.1f;
        followCam.fireLate -= PlayerLevel[3] * 0.004f;

        origin_Damage = followCam.damage;
        origin_Firelate = followCam.fireLate;
        origin_Critical = followCam.criticalValue;
        origin_Movement = player_MoveSpeed;
        origin_HP = playerFull_HP;
    }
    public void First_State_Interface()
    {
        First_Load_PlayerLevel();

        manager.lv_Damage_Text.text = followCam.damage.ToString("F1");
        manager.lv_FireLate_Text.text = followCam.fireLate.ToString("F2");
        manager.lv_Critical_Text.text = followCam.criticalValue.ToString("F1") + "%";
        manager.lv_MoveSpeed_Text.text = player_MoveSpeed.ToString("F1");
        player_HPtext.text = player_HP.ToString("F0") + "/" + playerFull_HP.ToString("F0");

        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        getCoinValue = 0;
        isSkillMaster = false;

        isMaster_Damage = false;
        isMaster_Firelate = false;
        isMaster_Critical = false;
        isMaster_Movement = false;
        isMaster_HP = false;

        cost_Stats = 10; //스텟에 지불해야하는 코인
        costStatsText.text = cost_Stats.ToString();
        playerFull_HP = player_HP;
        anim = player.GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameStageManager>();
        audioLimit = GameObject.Find("GameManager").GetComponent<AudioLimit>();
        //First_State_Interface();

        usingDash = true;
        isAlive = true;
        isMagnetActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Movement();
            Find_Box();
            PushStatsUp();

            if (!usingDash)
            {
                dash_CoolTime -= Time.deltaTime;
                player_Dash.fillAmount = dash_CoolTime / 3f;
                player_Dash.color = Color.black;
                if (dash_CoolTime <= 0)
                {
                    usingDash = true;
                    player_Dash.color = Color.white;
                    dash_CoolTime = 3f;
                }
            }
            GameObject Hive = GameObject.FindGameObjectWithTag("EnemyHive");
            if (Hive != null)
                Marker_Hive(Hive);

        }
    }

    IEnumerator PlayerParticleEffect()
    {
        player_Dash.fillAmount = 1;
        playerParticle.SetActive(true);
        usingDash = false;
        
        yield return new WaitForSeconds(0.4f);
        playerParticle.SetActive(false);
    }
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (Input.mousePosition.x > 0)
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }
        else
        {
            player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }
        if(x!=0 || y != 0)
        {
            Vector3 move = (transform.right * x + transform.up * y).normalized * Time.deltaTime * player_MoveSpeed;
            player.transform.position += move;
            player.transform.position = new Vector2(Mathf.Clamp(player.transform.position.x, -manager.minValue, manager.maxValue), Mathf.Clamp(player.transform.position.y, -manager.maxValue, manager.minValue));
            
            if (Input.GetKeyDown(KeyCode.Space) && usingDash)
            {
                //Debug.LogError("스페이스바: "+ this.GetComponent<Rigidbody2D>().angularVelocity);
                StartCoroutine(PlayerParticleEffect());
                this.transform.GetComponent<Rigidbody2D>().AddForce((Vector3.right * x + Vector3.up * y) * 5000f);
                
            }
            anim.SetInteger("Animate", 2); //Walk

           /* if (Input.GetMouseButton(0))
            {
                anim.SetInteger("Animate", 3);
            }
            else
            {
                anim.SetInteger("Animate", 2);
            }*/
        }
        else
        {
            anim.SetInteger("Animate", 0); //Idle

           /* if (Input.GetMouseButton(0))
            {
                anim.SetInteger("Animate", 1);
            }
            else
            {
                anim.SetInteger("Animate", 0);
            }*/
        }

        /// 0 = idle
        /// 1 = shoot idle
        /// 2 = walking
        /// 3 = shoot walking
        
    }
    void PlayerDead()//isAlive = false 일시 메소드 실행.
    {
        Instantiate(deadMark, transform.position, Quaternion.identity);
        this.GetComponent<SpriteRenderer>().enabled = false;
        weapon.SetActive(false);

        manager.PlayerDead();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "RandomItem") //아이템 습득시.
        {
            audioLimit.ExplodePlaySound(0);
            manager.Get_StageItemList(col.GetComponent<ItemDatabase>());
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "Item")
        {
            audioLimit.ExplodePlaySound(3);
            getCoinValue += 1;
            coinText.text = getCoinValue.ToString();
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "HealPack")
        {
            HealPack();

            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "MagnetItem")
        {
            Destroy(col.gameObject);
            StartCoroutine(MagnetRoutine(1));
        }
    }
    public void HealPack()
    {
        player_HP += playerFull_HP * 0.3f;
        player_HP = Mathf.Clamp(player_HP, 0, playerFull_HP);

        player_HPtext.text = player_HP.ToString("F0") + "/" + playerFull_HP.ToString("F0");
        player_HPimage.fillAmount = player_HP / playerFull_HP;
        manager.lv_HP_Text.text = playerFull_HP.ToString("F0");
    }
    /*private void OnTriggerStay2D(Collider2D box)
    {
        if (box.gameObject.tag == "SkillBox")
        {
            trigger_BoxList.Add(box);
            if (Input.GetKey(KeyCode.E) && trigger_BoxList[0].GetComponent<ItemboxCtrl>().costCoin <= getCoinValue)
            {
                //스킬이 만렙일시 게임머니or회복 랜덤 (스테이지 코인 x) 
                trigger_BoxList[0].GetComponent<ItemboxCtrl>().OpenSkillBox();
                getCoinValue = getCoinValue - trigger_BoxList[0].GetComponent<ItemboxCtrl>().costCoin;
                coinText.text = getCoinValue.ToString();

                Destroy(trigger_BoxList[0].gameObject);
                trigger_BoxList.Remove(trigger_BoxList[0]);
            }  
        }
        else if(box.gameObject.tag == "RandomBox")
        {

        }
    }*/
    private void OnTriggerExit2D(Collider2D box)
    {
        if (trigger_BoxList.Contains(box))
        {
            trigger_BoxList.Remove(box);
        }
    }
    private void PushStatsUp()
    {
        if(cost_Stats <= getCoinValue)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Skill_UP_Damage();
                StateLevelUp_Effect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Skill_UP_Firelate();
                StateLevelUp_Effect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Skill_UP_Critical();
                StateLevelUp_Effect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Skill_UP_Movement();
                StateLevelUp_Effect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Skill_UP_IncreaseHP();
                StateLevelUp_Effect();
            }
        }
        
    }
    private void StateLevelUp_Effect()
    {
        playerLevelUpParticle.SetActive(true);
        audioLimit.ExplodePlaySound(4);
    }
    private void Find_Box()//언박싱 중복방지.
    {
        Collider2D[] hitBoxItem = Physics2D.OverlapCircleAll(this.transform.position, 0.5f, skillBoxLayer);

        foreach (Collider2D boxItem in hitBoxItem)
        {
            if (!GameStageManager.isOpenBox /*&& Input.GetKeyDown(KeyCode.E)*/ && hitBoxItem[0].GetComponent<ItemboxCtrl>().costCoin <= getCoinValue && isAlive)
            {
                //스킬이 만렙일시 게임머니or회복 랜덤 (스테이지 코인 x) 
                hitBoxItem[0].GetComponent<ItemboxCtrl>().OpenBox();
                getCoinValue = getCoinValue - hitBoxItem[0].GetComponent<ItemboxCtrl>().costCoin;
                coinText.text = getCoinValue.ToString();

                Destroy(hitBoxItem[0].gameObject);
            }
        }
    }
    private void Marker_Hive(GameObject hive)
    {
        float Hive_dis = Vector2.Distance(hive.transform.position, this.transform.position);
        //Debug.Log("Dis: " + Hive_dis);
        if (Hive_dis > 20)
        {
            marker_Hive_UI.SetActive(true);

            Vector3 hivePos = hive.transform.position;
            Vector2 lookDir = hivePos - this.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            marker_Hive_UI.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            marker_Hive_UI.SetActive(false);
        }
            
    }
    private bool PayforStats(bool isMaster)//스텟 투자시 코인 차감.
    {
        if (cost_Stats <= getCoinValue && !isMaster)
        {
            //스텟 투자 허용
            getCoinValue = getCoinValue - cost_Stats;
            coinText.text = getCoinValue.ToString();
            cost_Stats += 10;

            //costStatsText.text = "Stats Cost: "+cost_Stats.ToString();
            costStatsText.text = cost_Stats.ToString();
            return true;
        }
        else
        {
            //스텟 투자 비허용
            return false;
        }
            
    }
    private IEnumerator MagnetRoutine(float duration)
    {
        isMagnetActive = true;
        float timer = 0f;

        while (timer < duration)
        {
            Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, 1000, coinLayer);

            foreach (Collider2D item in items)
            {
                Debug.Log("(자석)코인 갯수: "+ items.Length);
                item.transform.GetComponent<CoinCtrl>().isGetPlayer = true;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isMagnetActive = false;
    }

}
