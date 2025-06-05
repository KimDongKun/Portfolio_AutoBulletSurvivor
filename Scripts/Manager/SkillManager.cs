using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static bool isSkillMaster;
    public static List<int> skill_List;


    public GameObject thrrowKnife;
    public GameObject supportBullet;
    public GameObject thrrowGrenade;
    public GameObject mortar;
    public GameObject airStrike;

    public GameObject[] skill_IconPrefabs;
    public GameObject skill_Interface;

    private Text lv_thrrowKnife;
    private Text lv_supportBullet;
    private Text lv_thrrowGrenade;
    private Text lv_mortar;
    private Text lv_airStrike;

    public int getSkillcount = 0;
    private List<GameObject> activeSkill_List;
    private PlayerCtrl playerState;

    bool is_ThrrowKnife = false;
    bool is_SupportBullet = false;
    bool is_ThrrowGrenade = false;
    bool is_Mortar = false;
    bool is_AirStrike = false;


    // Start is called before the first frame update
    void Start()
    {
        getSkillcount = 0;

        thrrowKnife.SetActive(false);
        thrrowGrenade.SetActive(false);
        mortar.SetActive(false);
        airStrike.SetActive(false);

        activeSkill_List = new List<GameObject>();
         
        is_ThrrowKnife = false;
        is_SupportBullet = false;
        is_ThrrowGrenade = false;
        is_Mortar = false;
        is_AirStrike = false;

        playerState = GameObject.Find("Player_Sprite").GetComponent<PlayerCtrl>();

        //test
        //Instantiate(skill_IconPrefabs[0], skill_Interface.transform.GetChild(0));
    }

    public void UpgradeSkill(int _code)
    {
        _code += 5;
        switch (_code)//P_[패시브] 관련 스킬 현재 차단. 
        {
            case 0:// P_공격력
                playerState.Skill_UP_Damage();
                break;
            case 1:// P_공격속도
                playerState.Skill_UP_Firelate();
                break;
            case 2:// P_치명타 확률
                playerState.Skill_UP_Critical();
                break;
            case 3:// P_이동속도
                playerState.Skill_UP_Movement();
                break;
            case 4:// P_체력증가
                playerState.Skill_UP_IncreaseHP();
                break;
            case 5:// A_칼 던지기
                ActiveSkill_ThrrowKnife();
                break;
            case 6:// A_지원 사격
                ActiveSkill_SupportBullet();
                break;
            case 7:// A_수류탄 투척
                ActiveSkill_ThrrowGrenade();
                break;
            case 8:// A_포격 요청
                ActiveSkill_Mortar();
                break;
            case 9:// A_에어스트라이커
                ActiveSkill_AirStrike();
                break;
            default:
                // 구급상자 선택.
                playerState.HealPack();
                break;
        }
    }

    void ActiveSkill_ThrrowKnife()
    {
        if (!is_ThrrowKnife)
        {
            GameObject icon = Instantiate(skill_IconPrefabs[0], skill_Interface.transform.GetChild(getSkillcount));
            lv_thrrowKnife = icon.transform.GetChild(0).GetComponent<Text>();

            getSkillcount += 1;
            is_ThrrowKnife = true;
            thrrowKnife.SetActive(true);

            
            //따로 관리 할지 고려.
            //activeSkill_List.Add(activeSkill_Manage[0]);
        }
        else
        {
            thrrowKnife.GetComponent<RandomShootCtrl>().ActiveSkill_LevelUP();
            lv_thrrowKnife.text = thrrowKnife.GetComponent<RandomShootCtrl>().skill_Level.ToString();
            //10렙 이상시 게임 머니 및 체력 회복으로 보상.
        }
    }
    void ActiveSkill_SupportBullet()
    {

        if (!is_SupportBullet)
        {
            GameObject icon = Instantiate(skill_IconPrefabs[1], skill_Interface.transform.GetChild(getSkillcount));
            lv_supportBullet = icon.transform.GetChild(0).GetComponent<Text>();

            getSkillcount += 1;
            is_SupportBullet = true;

            supportBullet.GetComponent<BulletSupportManager>().ActiveSkill();
        }
        else
        {
            supportBullet.GetComponent<BulletSupportManager>().ActiveSkill_LevelUP();
            lv_supportBullet.text = supportBullet.GetComponent<BulletSupportManager>().skill_Level.ToString();
        }
    }
    void ActiveSkill_ThrrowGrenade()
    {
        if (!is_ThrrowGrenade)
        {
            GameObject icon = Instantiate(skill_IconPrefabs[2], skill_Interface.transform.GetChild(getSkillcount));
            lv_thrrowGrenade = icon.transform.GetChild(0).GetComponent<Text>();

            getSkillcount += 1;
            is_ThrrowGrenade = true;
            thrrowGrenade.SetActive(true);

        }
        else
        {
            thrrowGrenade.GetComponent<RandomGrenadeCtrl>().ActiveSkill_LevelUP();
            lv_thrrowGrenade.text = thrrowGrenade.GetComponent<RandomGrenadeCtrl>().skill_Level.ToString();
        }
    }
    void ActiveSkill_Mortar()
    {
        if (!is_Mortar)
        {
            GameObject icon = Instantiate(skill_IconPrefabs[3], skill_Interface.transform.GetChild(getSkillcount));
            lv_mortar = icon.transform.GetChild(0).GetComponent<Text>();

            getSkillcount += 1;
            is_Mortar = true;
            mortar.SetActive(true);

        }
        else
        {
            mortar.GetComponent<RandomThrrowBombCtrl>().ActiveSkill_LevelUP();
            lv_mortar.text = mortar.GetComponent<RandomThrrowBombCtrl>().skill_Level.ToString();
        }

    }
    void ActiveSkill_AirStrike()
    {
        if (!is_AirStrike)
        {
            GameObject icon = Instantiate(skill_IconPrefabs[4], skill_Interface.transform.GetChild(getSkillcount));
            lv_airStrike = icon.transform.GetChild(0).GetComponent<Text>();

            getSkillcount += 1;
            is_AirStrike = true;
            airStrike.SetActive(true);
        }
        else
        {
            airStrike.GetComponent<AirBombCtrl>().ActiveSkill_LevelUP();
            lv_airStrike.text = airStrike.GetComponent<AirBombCtrl>().skill_Level.ToString();
        }
    }
}
