using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameStageManager : MonoBehaviour
{
    public SaveJSonData JsonDataManager;
    public Raid_InventoryManager RaidinventoryData;
    public SkillManager skillManager;

    public SkillSelectInterface skillSelectInterface;

    public float maxValue;
    public float minValue;

    public static bool isGamePause;
    public static bool playerAlive;
    public static bool isOpenBox = false;
    private bool isPlayerDead = false;

    public GameObject inventory_UI;
    public GameObject setting_UI;
    public GameObject gamePause_ui;
    public GameObject openRandom_ui;
    public GameObject gameOver_ui;

    public GameObject skill_ui;
    public GameObject sub_TAB_ui;

    public GameObject spawnSetting;
    
    public float playTime;

    public GameObject item_Prefab;
    public GameObject healPack_Prefab;
    public GameObject randomItem_Prefab;
    public Image[] randomItem_List;
    private int _itemCode;

    public Text stageTimer;
    public static float timer;
    public static int hiveStack;

    public Text killText;
    public int killScore;

    public Text roundText;
    public Text nextRoundText;
    public int round;
    public int next_roundKill;

    public Text lv_Damage_Text;
    public Text lv_FireLate_Text;
    public Text lv_Critical_Text;
    public Text lv_MoveSpeed_Text;
    public Text lv_HP_Text;

    public Text level_Damage;
    public Text level_FireLate;
    public Text level_Critical;
    public Text level_MoveSpeed;
    public Text level_HP;
    //public Text lv_Defence_Text;

    public StageClear resultUI;
    public EscapeZoneCtrl escapeZone;
    public GameObject cursorAim_Image;
    public AudioSource background_Music;
    public Animator stageEffectAnim;

    // Start is called before the first frame update
    void Awake()
    {
        
        timer = 0;
        hiveStack = 0;
        killScore = 0;
        round = 1;
        next_roundKill = 5;
        isGamePause = false;
        playerAlive = false;
        gamePause_ui.SetActive(false);
        openRandom_ui.SetActive(false);

        resultUI.resultPage.SetActive(false);

        background_Music.mute = false;
        //해상도 고정
        Screen.SetResolution(1920, 1080, true);
        //커서 비활성
        Cursor.visible = false;
        //Cursor.SetCursor(cursorAim_Image, new Vector2(-1,-1), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        cursorAim_Image.GetComponent<RectTransform>().position = Input.mousePosition; 

        killText.text = killScore.ToString();
        timer += Time.deltaTime;
        stageTimer.text = ((int)timer/60 % 60).ToString("D2") + ":"+((int)timer%60).ToString("D2");

        roundText.text = round.ToString() + " Round";
        nextRoundText.text = "Next Round Kill : " + next_roundKill.ToString();

        RoundUpdate();
        RandromPosition_EscapeZone();
        HealPack_Spawn();
        StartCoroutine(SkillBox_Spawn());

        if (Input.GetKeyDown(KeyCode.Escape) && isGamePause && !isOpenBox)
        {
            GameRestart();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && !isGamePause)
        {
            GamePause_Settiong();
            //GamePurse();
            //JsonDataManager.ItemJsonData();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !isGamePause)
        {
            GamePause_Inventory();
            //GamePurse();
            //JsonDataManager.ItemJsonData();
        }
        if (Input.GetMouseButtonDown(0) && isOpenBox)
        {
            //CloseRandomBox(_itemCode);
        }

        if (Input.GetKeyDown(KeyCode.E) && isPlayerDead)
        {
            GameRestart();
            __BackShelterButton();
        }
        if (PlayerCtrl.cost_Stats <= PlayerCtrl.getCoinValue && !PlayerCtrl.isSkillMaster)//Input.GetKeyDown(KeyCode.Tab)
        {
            skill_ui.SetActive(true);
           
            
            /*
            if (skill_ui.activeSelf)
            {
                sub_TAB_ui.SetActive(true);
                skill_ui.SetActive(false);
            }
            else
            {
                sub_TAB_ui.SetActive(false);
                skill_ui.SetActive(true);
            }
            */
        }
        else
        {
            skill_ui.SetActive(false);
        }

        if (playerAlive)
        {
            background_Music.mute = true;
            escapeZone.gameObject.SetActive(false);
            resultUI.resultPage.SetActive(true);

            Time.timeScale = 0;
            isGamePause = true;

            
            resultUI.resultTimer.text = stageTimer.text;
        }
    }
    public void __BackShelterButton()
    {
        SceneManager.LoadScene("ShelterScene");
    }
    public void ItemList_Spawn()
    {
        Vector3 _randomItemPos = new Vector2(Random.Range(-minValue, maxValue), Random.Range(-maxValue, minValue));
        GameObject _item = Instantiate(item_Prefab, _randomItemPos, Quaternion.identity);
    }
    public void HealPack_Spawn()
    {
        Vector3 _randomHealPos = new Vector2(Random.Range(-(minValue - 16), (maxValue - 16)), Random.Range(-(maxValue - 16), (minValue - 16)));
        GameObject[] _healPacks = GameObject.FindGameObjectsWithTag("HealPack");

        if (_healPacks.Length < 1)
        {
            Instantiate(healPack_Prefab, _randomHealPos, Quaternion.identity);
        }
    }
    public IEnumerator SkillBox_Spawn() // Hive를 통해 SkillBox 생성
    {
        yield return new WaitForSeconds(3f);
        Vector3 _randomSkillPos = new Vector2(Random.Range(-(minValue-16), (maxValue - 16)), Random.Range(-(maxValue - 16), (minValue - 16)));
        //GameObject[] _skillBox = GameObject.FindGameObjectsWithTag("SkillBox");
        GameObject[] _Hive = GameObject.FindGameObjectsWithTag("EnemyHive");

        if (_Hive.Length < 1)
        {
           GameObject hiveObject = Instantiate(randomItem_Prefab, _randomSkillPos, Quaternion.identity);
        }
        else if(_Hive.Length > 1)
        {
            for(int i = 1; i< _Hive.Length; i++)
            {
                Destroy(_Hive[i]);
            }
        }
    }
    public void Get_StageItemList(ItemDatabase _item)
    {
        JsonDataManager.InventorySlotData(_item);
        RaidinventoryData.Add_Inventory(_item);
    }
    public void GamePause_Inventory()
    {
        Time.timeScale = 0;
        isGamePause = true;
        inventory_UI.SetActive(true);
    }
    public void GamePause_Settiong()
    {
        Time.timeScale = 0;
        isGamePause = true;
        setting_UI.SetActive(true);
    }
    public void GamePause()
    {
        Time.timeScale = 0;
        isGamePause = true;
        gamePause_ui.SetActive(true);
    }
    public void GameRestart()
    {
        Time.timeScale = 1;
        isGamePause = false;
        gamePause_ui.SetActive(false);
        inventory_UI.SetActive(false);
        setting_UI.SetActive(false);
    }
    public void PlayerDead()
    {
        GamePause();
        isPlayerDead = true;
        gameOver_ui.SetActive(true);

        JsonDataManager.GameOver_JsonSave();

        for (int i = 0; i < skillSelectInterface.skillDataList.Length; i++)
            skillSelectInterface.skillDataList[i].skillButton.onClick.RemoveAllListeners();
    }

    public void OpenRandomBox(List<int> skillcodeList)
    {
        isOpenBox = true;
        //_itemCode = itemCode;

        GamePause();
        openRandom_ui.SetActive(true);
        //randomItem_List[itemCode].gameObject.SetActive(true);
        for (int i = 0; i < skillSelectInterface.skillDataList.Length; i++)
        {
            skillSelectInterface.skillDataList[i].skillButton.onClick.RemoveAllListeners();
            int skillCode = skillcodeList[i];
            skillSelectInterface.skillDataList[i].skillCode = skillCode;
            skillSelectInterface.skillDataList[i].skillImage.sprite = randomItem_List[skillcodeList[i]].sprite;

            skillSelectInterface.skillDataList[i].skillButton.onClick.AddListener(() => OpenRandomBox_Select(skillCode));
        }
    }
    public void OpenRandomBox_Select(int skillCode)
    {
        Debug.Log("선택한 스킬 코드: " + skillCode);
        skillManager.UpgradeSkill(skillCode);
        CloseRandomBox(skillCode);
    }
    public void CloseRandomBox(int itemCode)
    {
        isOpenBox = false;

        GameRestart();
        openRandom_ui.SetActive(false);
        //randomItem_List[itemCode].gameObject.SetActive(false);
    }
    public void RandromPosition_EscapeZone()
    {
        int minTime = (int)(timer / 60 % 60);
        if (minTime%2 == 0 && !escapeZone.gameObject.activeSelf) //2분마다 이스케이프존 활성화
        {
            escapeZone.gameObject.SetActive(true);
            Vector3 _randomPos = new Vector2(Random.Range(-(minValue - 16), (maxValue - 16)), Random.Range(-(maxValue - 16), (minValue - 16)));
            escapeZone.transform.position = _randomPos;
        }

    }
    public void RoundUpdate()
    {
        if (next_roundKill < killScore)
        {
            int next = 5;
            next = round * 5;
           
            next_roundKill += next;
            round += 1;

            StageEffect();
        }
    }
    public void StageEffect()
    {
        stageEffectAnim.Play("StageRoundEffect");
    }
}
[System.Serializable]
public class StageClear
{
    public GameObject resultPage;
    public Text resultTimer;
}

[System.Serializable]
public class SkillSelectData
{
    public Button skillButton;
    public Image skillImage;
    public int skillCode;
}
[System.Serializable]
public class SkillSelectInterface
{
    public GameObject skillSelectPanel;
    public SkillSelectData[] skillDataList;
}