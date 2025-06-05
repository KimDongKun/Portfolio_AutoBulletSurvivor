using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Json �۾��� ���Ǵ� using ����Ʈ
using System;    //string <-> int �� convert �۾���
using System.IO; // ��ο� Json ���� �� �ε� �۾�
using LitJson;   //Json �÷�����.

public class SaveJSonData : MonoBehaviour
{
    public static bool isFirstJoinGame = false;

    public SettingManager audioSettingManager;
    public SettingData audioSettingData;
    public List<SaveData> saveData = new List<SaveData>();
    public List<ItemData> n_itemData = new List<ItemData>();

    public List<Sprite> itemImage_List = new List<Sprite>();
    public List<string> itemNameList = new List<string>();
    public List<int> itemCountList = new List<int>();
    public List<ItemData> _ItemDataList = new List<ItemData>();

    //===========================

    public List<int> playerStateData = new List<int>(); //�÷��̾� ���� ����

    public int nowDollar;
    public int nowRespectPoint;
    public List<int> buildAreaLevel = new List<int>();

    public List<string> inventory_itemName = new List<string>();
    public List<int> inventory_itemValue = new List<int>();
    public List<ItemData> inventory_Shelter = new List<ItemData>();

    public SceneName stage;
    public enum SceneName
    {
        Game,
        Shelter
    };
    
    void LoadJson_SaveData()
    {
        if (File.Exists(Application.dataPath + "/SaveData.json")) //����� Json �����.
        {
            Debug.Log(Application.dataPath + "/SaveData.json" + "\n�ش��ο� ����� �����Ͱ� �����մϴ�.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");//SaveItemData
            //string loadData = File.ReadAllText(Application.dataPath + "/SaveItemData.json");
            JsonData loadJson = JsonMapper.ToObject(loadData);

            Debug.Log("�ҷ��� Json: " + loadData);
            //JsonDataLoader(loadJson);//SaveData�� Json�����͸� �Ľ��ؼ� List�� ���ġ.

            LoadJson_PlayerState(loadJson);
            LoadJson_ShelterData(loadJson);
            LoadJson_Inventory(loadJson);

            Load_AudioSetting();
            var playerData = GameObject.FindObjectOfType<PlayerCtrl>();
            if (playerData != null)
            {
                playerData.First_State_Interface();
            }
        }
        else //����� Json ���� ���ҽ�.
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�.");
            NewJson_SaveData(); //Json �߰�.
        }
    }
    
    void NewJson_SaveData()
    {
        List<SaveData> newData = new List<SaveData>();

        List<int> _buildLevel = new List<int>();
        List<ItemData> _itemData = new List<ItemData>();
        PlayerData _playerData = new PlayerData(0, 0, 0, 0);
        ShelterData _shelterData = new ShelterData(0, 0, _buildLevel);
        
        newData.Add(new SaveData(_playerData, _shelterData, _itemData));// ���� �ϴܿ� ��ġ�� SaveData Ŭ������ �����͸� ����.
        JsonData data = JsonMapper.ToJson(newData); // saveData �����͸� Json���� ġȯ 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // �ش� ��ο� Json ���·� ����.

        isFirstJoinGame = true; //ù ���� ���۽�.

        LoadJson_SaveData();
    }

    void LoadJson_PlayerState(JsonData _loadJson)
    {
        //_loadJson["playerData"].Count ��� '���ݰ���'��ŭ �����ص� ��.
        for (int i = 0; i<_loadJson[0]["playerData"].Count; i++)
        {
            playerStateData.Add(Convert.ToInt32(_loadJson[0]["playerData"][i].ToString()));
        }
    }
    void LoadJson_ShelterData(JsonData _loadJson)
    {
        nowDollar = Convert.ToInt32(_loadJson[0]["shelterData"]["dollar"].ToString());
        nowRespectPoint = Convert.ToInt32(_loadJson[0]["shelterData"]["respectPoint"].ToString());

        //_loadJson["shelterData"]["BuildAreaLevelList"].Count ��� '�ü�����'��ŭ �����ص� ��.
        for (int i = 0; i < _loadJson[0]["shelterData"]["BuildAreaLevelList"].Count; i++)
        {
            buildAreaLevel.Add(Convert.ToInt32(_loadJson[0]["playerData"][i].ToString()));
        }
    }
    void LoadJson_Inventory(JsonData _loadJson)//Json ����� ������ �ҷ����� - ���� ���� Method
    {
        for (int i = 0; i < _loadJson[0]["item"].Count; i++)
        {
            /// inventory_itemName
            /// inventory_itemValue

            inventory_itemName.Add(_loadJson[0]["item"][i][0].ToString());
            inventory_itemValue.Add(Convert.ToInt32(_loadJson[0]["item"][i][1].ToString()));
            //inventory_Shelter.Add(new ItemData(_loadJson[0]["item"][i][0].ToString(), Convert.ToInt32(_loadJson[0]["item"][i][1].ToString())));
        }

    }
    public void SAVE_JsonData()
    {
        List<SaveData> saveData = new List<SaveData>();

        List<ItemData> _itemData = new List<ItemData>();
        for (int i = 0; i < inventory_itemName.Count; i++)
        {
            _itemData.Add(new ItemData(inventory_itemName[i], inventory_itemValue[i]));
        }
        PlayerData _playerData = new PlayerData(playerStateData[0], playerStateData[1], playerStateData[2], playerStateData[3]);
        ShelterData save_shelterData = new ShelterData(nowDollar, nowRespectPoint, buildAreaLevel);

        saveData.Add(new SaveData(_playerData, save_shelterData, _itemData));// ���� �ϴܿ� ��ġ�� SaveData Ŭ������ �����͸� ����.
        JsonData data = JsonMapper.ToJson(saveData); // saveData �����͸� Json���� ġȯ 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // �ش� ��ο� Json ���·� ����.
        Debug.Log("���� �Ϸ�");
    }
    public void Save_AudioSetting()
    {
        JsonData audio_JsonData = JsonMapper.ToJson(audioSettingData);
        File.WriteAllText(Application.dataPath + "/Setting.json", audio_JsonData.ToString());

        Load_AudioSetting();
    }
    public void Load_AudioSetting()
    {
        if (File.Exists(Application.dataPath + "/Setting.json"))
        {
            string loadData = File.ReadAllText(Application.dataPath + "/Setting.json");
            JsonData loadJson = JsonMapper.ToObject(loadData);

            float bgmVolume = float.Parse(loadJson["BGM_volume"].ToString());
            float fireVolume = float.Parse(loadJson["Fire_volume"].ToString());
            float effectVolume = float.Parse(loadJson["Effect_volume"].ToString());
            audioSettingManager.Volume_Setting(bgmVolume, fireVolume, effectVolume);
        }
        else
        {
            audioSettingData = new SettingData(1.0f, 1.0f, 1.0f);
            Save_AudioSetting();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(stage == SceneName.Game)
        {
            Debug.Log("����");
            LoadJson_SaveData();
        }
        else if(stage == SceneName.Shelter)
        {
            Cursor.visible = true;
            Debug.Log("����");
            Time.timeScale = 1;
            //Load_ItemJsonData();
            LoadJson_SaveData();
        }
        Debug.Log(stage);


        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            Debug.Log(Application.dataPath + "/SaveData.json"+"\n�ش��ο� �����Ͱ� �����մϴ�.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");
            JsonData loadJson = JsonMapper.ToObject(loadData);

            Debug.Log("�ҷ��� Json:"+loadData);
            JsonDataLoader(loadJson);
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�.");
            NewJsonData(); //Json �߰�.
        }
    }
    public void InventorySlotData(ItemDatabase _data)
    {
        Debug.Log(_data.itemName);
        
        if (itemNameList.Contains(_data.itemName)) //�κ��� �ߺ��� �� ����� count +1 ����
        {
            for (int i = 0; i < itemNameList.Count; i++)
            {
                if (itemNameList[i] == _data.itemName)
                {
                    itemCountList[i] += 1;
                }
            }
        }
        else //�κ��� ���� ��(ó��) �����
        {
            itemImage_List.Add(_data.itemSptrie);

            itemNameList.Add(_data.itemName);
            itemCountList.Add(1);
        }

        //�Ʒ� �޼ҵ�� esc ����� ���� -> Merge ����.
        //ItemJsonData();
    }

    void JsonDataLoader(JsonData _loadJson) //���� ������ Debug.Log�� ��� Method
    {
        Debug.Log("Json(���ȣ ����): " + _loadJson.Count); // ���ȣ ����
        for(int i = 0; i<_loadJson[0].Count; i++)
        {
            if(i == 4)
            {
                for (int j = 0; j < _loadJson[0][4].Count; j++)
                {
                    Debug.Log(j+"��° ���γ���: "+_loadJson[0][4][j]);
                }
            }
            else
            {
                Debug.Log(i + "��° json ������ : " + _loadJson[0][i]);
            }
        }
        //Debug.Log(""+ Convert.ToInt32(_loadJson[0][0].ToString()))
        //Debug.Log("string -> int ����Ʈ :" + Convert.ToInt32(_loadJson[0][0].ToString()));
        //Debug.Log("string -> int ����Ʈ :" + Convert.ToInt32(_loadJson[0][1].ToString()));
        
        //NewJsonData();
    }
    void NewJsonData() //Json ���� �� ������Ʈ �׽�Ʈ��.
    {
        //saveData.Add(new SaveData(1, 2, false, "GamGoo"));
        //saveData.Add(new SaveData(22, 4, false, "Player"));// ���� �ϴܿ� ��ġ�� SaveData Ŭ������ �����͸� ����.
        saveData = new List<SaveData>();
        JsonData data = JsonMapper.ToJson(saveData); // saveData �����͸� Json���� ġȯ 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // �ش� ��ο� Json ���·� ����.
    }
    public void ItemJsonData()// �������� �� Ż��� �� ����� ����X
    {

        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            Debug.Log(Application.dataPath + "/SaveData.json" + "\n�ش��ο� �����Ͱ� �����մϴ�.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");
            JsonData loadItemData = JsonMapper.ToObject(loadData);

            _ItemDataList = new List<ItemData>();
            if(loadItemData[0]["item"].Count != 0)
            {
                for (int a = 0; a < loadItemData[0]["item"].Count; a++)
                {
                    _ItemDataList.Add(new ItemData(loadItemData[0]["item"][a]["name"].ToString(), Convert.ToInt32(loadItemData[0]["item"][a]["count"].ToString())));
                }
                Debug.Log("�ҷ��� ������ Json:" + loadItemData[0]["item"].Count);
                Debug.Log("�ҷ��� ������ Json:" + loadItemData[0]["item"]);

                for (int i = 0; i < _ItemDataList.Count; i++) // ���� ������ �ߺ� ȹ��� ���� ����.
                {
                    //Debug.Log(loadItemData[i]["name"] + "\n" + loadItemData[i]["count"]);
                    for (int j = 0; j < itemNameList.Count; j++)
                    {
                        if (_ItemDataList[i].name == itemNameList[j])
                        {
                            int _jsonCount = itemCountList[j] + _ItemDataList[i].count;

                            _ItemDataList[i].count = _jsonCount;
                            //New Item�� ������� Json Data�� �߰� �� ���� �ʿ�. 
                            //�Ǵ� ��� ItemList�� Jsonȭ ��Ű�� ������ 0���� �ݿ��Ͽ� ����.
                            //���� ������ json�� �ٽ� List�� ��ȯ���� ������ List�� �ٽ� jsonȭ ��Ű�� �۾�.
                            //�� ���� �۾� �Ϸ�.
                        }
                    }
                }

                for (int k = 0; k < itemNameList.Count; k++)
                {
                    int stack = 0;
                    Debug.Log("asdasdsa " + stack);
                    for (int l = 0; l < _ItemDataList.Count; l++)
                    {
                        if (itemNameList[k] != _ItemDataList[l].name)
                        {
                            Debug.Log(itemNameList[k] + " and " + _ItemDataList[l].name);
                            stack += 1;
                            if (stack == _ItemDataList.Count)
                            {
                                //������ �߰�
                                Debug.Log("������ �߰�" + itemNameList[k]);
                                _ItemDataList.Add(new ItemData(itemNameList[k], itemCountList[k]));
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("�κ� ������");
                for(int i = 0; i<itemNameList.Count; i++)
                {
                    _ItemDataList.Add(new ItemData(itemNameList[i], itemCountList[i]));
                }
               
            }

            List<SaveData> saveData = new List<SaveData>();

            List<ItemData> _itemData = _ItemDataList;//���� �ɰ���, �������� ���� ���� �����۸���Ʈ�� Json���� ���� �ȵ� ==>> ���� �ذ��� ������. ������, ���� �����ʿ��Ҽ������� üũ.
            
            PlayerData _playerData = new PlayerData(playerStateData[0], playerStateData[1], playerStateData[2], playerStateData[3]);
            ShelterData save_shelterData = new ShelterData(nowDollar, nowRespectPoint, buildAreaLevel);

            saveData.Add(new SaveData(_playerData, save_shelterData, _itemData));// ���� �ϴܿ� ��ġ�� SaveData Ŭ������ �����͸� ����.
            JsonData data = JsonMapper.ToJson(saveData); // saveData �����͸� Json���� ġȯ 
            File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // �ش� ��ο� Json ���·� ����.
            Debug.Log("���� �Ϸ�");

            //JsonData itemdata = JsonMapper.ToJson(_ItemDataList);
            //File.WriteAllText(Application.dataPath + "/SaveData.json", itemdata.ToString());
        }
        else //Json ������ �� �����.
        {
            n_itemData.Clear();
            for (int i = 0; i < itemNameList.Count; i++)
            {
                n_itemData.Add(new ItemData(itemNameList[i], itemCountList[i]));
            }

            JsonData itemdata = JsonMapper.ToJson(n_itemData); // saveData �����͸� Json���� ġȯ 
            File.WriteAllText(Application.dataPath + "/SaveData.json", itemdata.ToString()); // �ش� ��ο� Json ���·� ����.
        }
    }
    public void Load_ItemJsonData()//���� �� SaveItemData.json ������ �ҷ����� - UI �� �κ��丮
    {
        if (File.Exists(Application.dataPath + "/SaveItemData.json"))
        {
            string InventoryData = File.ReadAllText(Application.dataPath + "/SaveItemData.json");
            JsonData load_InventoryJson = JsonMapper.ToObject(InventoryData);
            LoadJson_Inventory(load_InventoryJson);
        }
        else
        {
            // �κ��丮�� ������ Json ���� ���� ����, ù ������ ������ �����Ͽ� Ʃ�丮�� ����.
            Debug.Log("Ʃ�丮��");
        }
    }
    public void GameStart_JsonLoad() //���ӽ��۽�
    {
        //StageItem_Json();
    }
    public void GameOver_JsonSave() //���ӿ����� or Ŭ����
    {
        //StageItem_Json();
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

[System.Serializable]
public class SaveData
{
    public PlayerData playerData;
    public ShelterData shelterData;
    public List<ItemData> item;

    public SaveData(PlayerData _playerData, ShelterData _shelterData, List<ItemData> _item)
    {
        playerData = _playerData;
        shelterData = _shelterData;
        item = _item;
    }
}
[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// ex) Lv_Damage ������ ������ ���Ϳ��� �޷��� �����ϰ� ���������� '������' ���� ���
    /// ���� �ʿ�� ���� ���� ����. ���� ǥ�� ���� = "Lv_" �ش� ���ô�� �ʿ�� �߰�.
    /// </summary>

    public int Lv_HP;
    public int Lv_Damage;
    public int Lv_MoveSpeed;
    public int Lv_AtkSpeed;

    public PlayerData(int _Lv_HP, int _Lv_Damage, int _Lv_MoveSpeed, int _Lv_AtkSpeed)
    {
        Lv_HP = _Lv_HP;
        Lv_Damage = _Lv_Damage;
        Lv_MoveSpeed = _Lv_MoveSpeed;
        Lv_AtkSpeed = _Lv_AtkSpeed;
    }
}
[System.Serializable]
public class ShelterData
{
    /// <summary>
    /// ���� �� �ü� ���� ��  ����
    /// isCleanArea False -> True ���� isBuildArea�� �ü� Build ����
    /// isBuildAreaList�� �ü��� ���׷��̵� ��Ų�ٸ� int������ �������� �ʿ�. - ���� ��� ��
    /// 
    /// �� ������� �ҷ��������� 
    /// �������� �����ϱ���� ex 0 = ����, 1 = ���� , 2 = �ü� ���� , 3 = ���� �ü� ���׷��̵�
    /// </summary>

    public int dollar;
    public int respectPoint;
    public List<int> BuildAreaLevelList;
    public ShelterData(int _dollar, int _respectPoint, List<int> _BuildAreaLevelList)
    {
        dollar = _dollar;
        respectPoint = _respectPoint;
        BuildAreaLevelList = _BuildAreaLevelList;
    }
}
[System.Serializable]
public class ItemData
{
    public string name;
    public int count;
    public ItemData(string _name, int _count)
    {
        name = _name;
        count = _count;
    }
}
[System.Serializable]
public class SettingData
{
    public string BGM_volume;
    public string Fire_volume;
    public string Effect_volume;
    public SettingData(float _bgm, float _fire, float _effect)
    {
        BGM_volume = _bgm.ToString();
        Fire_volume = _fire.ToString();
        Effect_volume = _effect.ToString();
    }
    
}   