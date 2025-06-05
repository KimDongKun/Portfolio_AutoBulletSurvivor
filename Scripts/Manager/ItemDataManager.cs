using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemDataManager : MonoBehaviour
{
    private SaveJSonData jsonData;
    public AudioLimit upgradeSound;

    public Sprite[] itemIcon_Array;
    public string[] itemName_Array;

    public List<UpgradeData> UpgradeDataList = new List<UpgradeData>();
    public List<Image> upgrade_Icon; //4개
    public List<Text> upgrade_text;  //4개

    private List<string> upgrade_getItemName;
    private List<int> upgrade_getItemValue;
    private int upgrade_costDollar;
    public Button upgradeButton;

    public int playerLevel;

    public Text coreLevelUI_UpgradePanel_text;
    public Text coreLevelUI_text;
    public Text costDollar_text;

    public Text stateUI_HP;
    public Text stateUI_Damage;
    public Text stateUI_Move;
    public Text stateUI_FireRate;
    // Start is called before the first frame update
    void Start()
    {
        jsonData = this.GetComponent<SaveJSonData>();
        
    }
    int GetItemValue(string _searchItemName)
    {
        int _result = 0;
        if (jsonData.inventory_itemName.Contains(_searchItemName))
        {
            _result = jsonData.inventory_itemValue[jsonData.inventory_itemName.IndexOf(_searchItemName)];
        }
        else
        {
            _result = 0;
        }
        
        return _result;
    }
    int CostItemValue(int _playerLevel)
    {
        int _result = 0;

        if (_playerLevel < 15)//레벨별 플레이 타임 예상 : 0-5분
        {
            _result = 3;
            upgrade_costDollar = 300;
        }
        else if (_playerLevel < 30)//5-10분
        {
            _result = 5;
            upgrade_costDollar = 500;
        }
        else if (_playerLevel < 40)//10-15분
        {
            _result = 10;
            upgrade_costDollar = 1000;
        }
        else if (_playerLevel < 50)//15분
        {
            _result = 20;
            upgrade_costDollar = 3000;
        }
        else if (_playerLevel < 60)//15-20분
        {
            _result = 25;
            upgrade_costDollar = 5000;
        }
        else if (_playerLevel < 80)// && _playerLevel < 80)//20분
        {
            _result = 30;
            upgrade_costDollar = 10000;
        }
        costDollar_text.text = "$" + upgrade_costDollar;
        return _result;
    }
    public void Next_Upgrade()//다음 업그레이드 필요한 아이템 나열
    {
        playerLevel = 0;
        /*for (int i = 0; i< jsonData.playerStateData.Count; i++)
        {
            playerLevel += jsonData.playerStateData[i];
        }*/
        playerLevel = jsonData.playerStateData[0];
        int maxLevel = 80;

        if (playerLevel < maxLevel)
        {
            coreLevelUI_text.text = playerLevel.ToString();
            coreLevelUI_UpgradePanel_text.text = playerLevel.ToString();

            upgrade_getItemValue = new List<int>();
            upgrade_getItemName = new List<string>();
            //int upgradeCount = jsonData.playerStateData[0];
            int upgradeCount = jsonData.playerStateData[0]/4;
            for (int i = 0; i < upgrade_Icon.Count; i++)
            {
                upgrade_Icon[i].sprite = itemIcon_Array[upgradeCount + i];
                upgrade_getItemName.Add(itemName_Array[upgradeCount + i]);
                upgrade_getItemValue.Add(GetItemValue(itemName_Array[upgradeCount + i]));
                upgrade_text[i].text = upgrade_getItemValue[i] + "/" + CostItemValue(playerLevel);
            }
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(delegate { __Upgrade_Button(); });

            PlayerStateUpgrade(jsonData.playerStateData);
        }
        else
        {
            //Player Level == Max
            coreLevelUI_text.text = maxLevel.ToString();
            coreLevelUI_UpgradePanel_text.text = maxLevel.ToString();
            costDollar_text.text = "MAX";
            upgradeButton.onClick.RemoveAllListeners();
        }
        
        //Level_Upgrade();
    }
    void Level_Upgrade()//클릭시 레벨 업그레이드
    {
        int _lv_count = ((int)(playerLevel / jsonData.playerStateData.Count)) + 1;
        int _lv = playerLevel % jsonData.playerStateData.Count;

        //jsonData.playerStateData[_lv] += 1;
        for (int i = 0; i < jsonData.playerStateData.Count; i++)
        {
            jsonData.playerStateData[i] += 1;
        }
        PlayerStateUpgrade(jsonData.playerStateData);
    }
    void Cost_Upgrade()
    {
        int cost = CostItemValue(playerLevel);
        for (int i = 0; i< upgrade_Icon.Count; i++)
        {
            if (jsonData.inventory_itemName.Contains(upgrade_getItemName[i]))
            {
                int indexOfList = jsonData.inventory_itemName.IndexOf(upgrade_getItemName[i]);
                if (jsonData.inventory_itemValue[indexOfList] < cost)
                {
                    Debug.Log("아이템이 부족합니다.");
                    break;
                }
                else
                {
                    if(i == upgrade_Icon.Count-1)
                    {
                        Debug.Log("업그레이드의 조건을 만족하였습니다.");
                        for(int j = 0; j < upgrade_Icon.Count; j++)
                        {
                            int _indexOfList = jsonData.inventory_itemName.IndexOf(upgrade_getItemName[j]);
                            if (jsonData.inventory_itemValue[_indexOfList] >= cost && jsonData.nowDollar >= upgrade_costDollar)
                            {
                                jsonData.inventory_itemValue[_indexOfList] -= cost;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if(jsonData.nowDollar < upgrade_costDollar)
                        {
                            break;
                        }
                        else
                        {
                            jsonData.nowDollar -= upgrade_costDollar;
                        }

                        upgradeSound.ExplodePlaySound(1);

                        Level_Upgrade();
                        jsonData.SAVE_JsonData();
                    }
                }
            }
        }
    }
    void PlayerStateUpgrade(List<int> PlayerLevel)
    {
        float data_HP = PlayerLevel[0] * 25f;
        stateUI_HP.text= data_HP.ToString();//1000
        stateUI_Damage.text = (PlayerLevel[1] * 5f).ToString();//100
        stateUI_Move.text = (PlayerLevel[2] * 0.1f).ToString();//2
        stateUI_FireRate.text = (PlayerLevel[3] * 0.004f).ToString();//0.15
    }
    public void __Upgrade_Button()
    {
        Cost_Upgrade();
        Next_Upgrade();
    }
}

[System.Serializable]
public class UpgradeData
{
    public List<Need_ItemInfo> needitemID_List;
    public int costDollar;
    public UpgradeData(List<Need_ItemInfo> _itemIDList, int _costDollar)
    {
        needitemID_List = _itemIDList;
        costDollar = _costDollar;
    }
}
[System.Serializable]
public class Need_ItemInfo
{
    public int itemID;
    public int itemValue;
    public Need_ItemInfo(int _itemID, int _itemValue)
    {
        itemID = _itemID;
        itemValue = _itemValue;
    }
}