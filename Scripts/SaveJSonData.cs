using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Json 작업에 사용되는 using 리스트
using System;    //string <-> int 등 convert 작업용
using System.IO; // 경로에 Json 저장 및 로드 작업
using LitJson;   //Json 플러그인.

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

    public List<int> playerStateData = new List<int>(); //플레이어 스텟 레벨

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
        if (File.Exists(Application.dataPath + "/SaveData.json")) //저장된 Json 존재시.
        {
            Debug.Log(Application.dataPath + "/SaveData.json" + "\n해당경로에 저장된 데이터가 존재합니다.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");//SaveItemData
            //string loadData = File.ReadAllText(Application.dataPath + "/SaveItemData.json");
            JsonData loadJson = JsonMapper.ToObject(loadData);

            Debug.Log("불러온 Json: " + loadData);
            //JsonDataLoader(loadJson);//SaveData의 Json데이터를 파싱해서 List에 재배치.

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
        else //저장된 Json 존재 안할시.
        {
            Debug.Log("데이터가 존재하지 않습니다.");
            NewJson_SaveData(); //Json 추가.
        }
    }
    
    void NewJson_SaveData()
    {
        List<SaveData> newData = new List<SaveData>();

        List<int> _buildLevel = new List<int>();
        List<ItemData> _itemData = new List<ItemData>();
        PlayerData _playerData = new PlayerData(0, 0, 0, 0);
        ShelterData _shelterData = new ShelterData(0, 0, _buildLevel);
        
        newData.Add(new SaveData(_playerData, _shelterData, _itemData));// 제일 하단에 위치한 SaveData 클래스에 데이터를 삽입.
        JsonData data = JsonMapper.ToJson(newData); // saveData 데이터를 Json으로 치환 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // 해당 경로에 Json 형태로 저장.

        isFirstJoinGame = true; //첫 게임 시작시.

        LoadJson_SaveData();
    }

    void LoadJson_PlayerState(JsonData _loadJson)
    {
        //_loadJson["playerData"].Count 대신 '스텟갯수'만큼 적용해도 됨.
        for (int i = 0; i<_loadJson[0]["playerData"].Count; i++)
        {
            playerStateData.Add(Convert.ToInt32(_loadJson[0]["playerData"][i].ToString()));
        }
    }
    void LoadJson_ShelterData(JsonData _loadJson)
    {
        nowDollar = Convert.ToInt32(_loadJson[0]["shelterData"]["dollar"].ToString());
        nowRespectPoint = Convert.ToInt32(_loadJson[0]["shelterData"]["respectPoint"].ToString());

        //_loadJson["shelterData"]["BuildAreaLevelList"].Count 대신 '시설갯수'만큼 적용해도 됨.
        for (int i = 0; i < _loadJson[0]["shelterData"]["BuildAreaLevelList"].Count; i++)
        {
            buildAreaLevel.Add(Convert.ToInt32(_loadJson[0]["playerData"][i].ToString()));
        }
    }
    void LoadJson_Inventory(JsonData _loadJson)//Json 저장된 데이터 불러오기 - 쉘터 전용 Method
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

        saveData.Add(new SaveData(_playerData, save_shelterData, _itemData));// 제일 하단에 위치한 SaveData 클래스에 데이터를 삽입.
        JsonData data = JsonMapper.ToJson(saveData); // saveData 데이터를 Json으로 치환 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // 해당 경로에 Json 형태로 저장.
        Debug.Log("저장 완료");
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
            Debug.Log("게임");
            LoadJson_SaveData();
        }
        else if(stage == SceneName.Shelter)
        {
            Cursor.visible = true;
            Debug.Log("쉘터");
            Time.timeScale = 1;
            //Load_ItemJsonData();
            LoadJson_SaveData();
        }
        Debug.Log(stage);


        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            Debug.Log(Application.dataPath + "/SaveData.json"+"\n해당경로에 데이터가 존재합니다.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");
            JsonData loadJson = JsonMapper.ToObject(loadData);

            Debug.Log("불러온 Json:"+loadData);
            JsonDataLoader(loadJson);
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다.");
            NewJsonData(); //Json 추가.
        }
    }
    public void InventorySlotData(ItemDatabase _data)
    {
        Debug.Log(_data.itemName);
        
        if (itemNameList.Contains(_data.itemName)) //인벤에 중복된 템 습득시 count +1 증가
        {
            for (int i = 0; i < itemNameList.Count; i++)
            {
                if (itemNameList[i] == _data.itemName)
                {
                    itemCountList[i] += 1;
                }
            }
        }
        else //인벤에 없는 템(처음) 습득시
        {
            itemImage_List.Add(_data.itemSptrie);

            itemNameList.Add(_data.itemName);
            itemCountList.Add(1);
        }

        //아래 메소드는 esc 종료시 실행 -> Merge 실행.
        //ItemJsonData();
    }

    void JsonDataLoader(JsonData _loadJson) //내부 데이터 Debug.Log로 출력 Method
    {
        Debug.Log("Json(대괄호 기준): " + _loadJson.Count); // 대괄호 묶음
        for(int i = 0; i<_loadJson[0].Count; i++)
        {
            if(i == 4)
            {
                for (int j = 0; j < _loadJson[0][4].Count; j++)
                {
                    Debug.Log(j+"번째 세부내용: "+_loadJson[0][4][j]);
                }
            }
            else
            {
                Debug.Log(i + "번째 json 데이터 : " + _loadJson[0][i]);
            }
        }
        //Debug.Log(""+ Convert.ToInt32(_loadJson[0][0].ToString()))
        //Debug.Log("string -> int 컨버트 :" + Convert.ToInt32(_loadJson[0][0].ToString()));
        //Debug.Log("string -> int 컨버트 :" + Convert.ToInt32(_loadJson[0][1].ToString()));
        
        //NewJsonData();
    }
    void NewJsonData() //Json 저장 및 업데이트 테스트용.
    {
        //saveData.Add(new SaveData(1, 2, false, "GamGoo"));
        //saveData.Add(new SaveData(22, 4, false, "Player"));// 제일 하단에 위치한 SaveData 클래스에 데이터를 삽입.
        saveData = new List<SaveData>();
        JsonData data = JsonMapper.ToJson(saveData); // saveData 데이터를 Json으로 치환 
        File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // 해당 경로에 Json 형태로 저장.
    }
    public void ItemJsonData()// 게임종료 및 탈출시 단 사망시 실행X
    {

        if (File.Exists(Application.dataPath + "/SaveData.json"))
        {
            Debug.Log(Application.dataPath + "/SaveData.json" + "\n해당경로에 데이터가 존재합니다.");

            string loadData = File.ReadAllText(Application.dataPath + "/SaveData.json");
            JsonData loadItemData = JsonMapper.ToObject(loadData);

            _ItemDataList = new List<ItemData>();
            if(loadItemData[0]["item"].Count != 0)
            {
                for (int a = 0; a < loadItemData[0]["item"].Count; a++)
                {
                    _ItemDataList.Add(new ItemData(loadItemData[0]["item"][a]["name"].ToString(), Convert.ToInt32(loadItemData[0]["item"][a]["count"].ToString())));
                }
                Debug.Log("불러온 아이템 Json:" + loadItemData[0]["item"].Count);
                Debug.Log("불러온 아이템 Json:" + loadItemData[0]["item"]);

                for (int i = 0; i < _ItemDataList.Count; i++) // 기존 아이템 중복 획득시 갯수 증가.
                {
                    //Debug.Log(loadItemData[i]["name"] + "\n" + loadItemData[i]["count"]);
                    for (int j = 0; j < itemNameList.Count; j++)
                    {
                        if (_ItemDataList[i].name == itemNameList[j])
                        {
                            int _jsonCount = itemCountList[j] + _ItemDataList[i].count;

                            _ItemDataList[i].count = _jsonCount;
                            //New Item을 얻었을때 Json Data에 추가 및 수정 필요. 
                            //또는 모든 ItemList를 Json화 시키고 개수를 0으로 반영하여 수정.
                            //현재 저장한 json을 다시 List로 변환시켜 수정후 List를 다시 json화 시키는 작업.
                            //위 내용 작업 완료.
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
                                //아이템 추가
                                Debug.Log("아이템 추가" + itemNameList[k]);
                                _ItemDataList.Add(new ItemData(itemNameList[k], itemCountList[k]));
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("인벤 깨끗함");
                for(int i = 0; i<itemNameList.Count; i++)
                {
                    _ItemDataList.Add(new ItemData(itemNameList[i], itemCountList[i]));
                }
               
            }

            List<SaveData> saveData = new List<SaveData>();

            List<ItemData> _itemData = _ItemDataList;//문제 심각함, 스테이지 에서 먹은 아이템리스트가 Json으로 저장 안됨 ==>> 현재 해결한 상태임. 하지만, 추후 수정필요할수있으니 체크.
            
            PlayerData _playerData = new PlayerData(playerStateData[0], playerStateData[1], playerStateData[2], playerStateData[3]);
            ShelterData save_shelterData = new ShelterData(nowDollar, nowRespectPoint, buildAreaLevel);

            saveData.Add(new SaveData(_playerData, save_shelterData, _itemData));// 제일 하단에 위치한 SaveData 클래스에 데이터를 삽입.
            JsonData data = JsonMapper.ToJson(saveData); // saveData 데이터를 Json으로 치환 
            File.WriteAllText(Application.dataPath + "/SaveData.json", data.ToString()); // 해당 경로에 Json 형태로 저장.
            Debug.Log("저장 완료");

            //JsonData itemdata = JsonMapper.ToJson(_ItemDataList);
            //File.WriteAllText(Application.dataPath + "/SaveData.json", itemdata.ToString());
        }
        else //Json 데이터 미 존재시.
        {
            n_itemData.Clear();
            for (int i = 0; i < itemNameList.Count; i++)
            {
                n_itemData.Add(new ItemData(itemNameList[i], itemCountList[i]));
            }

            JsonData itemdata = JsonMapper.ToJson(n_itemData); // saveData 데이터를 Json으로 치환 
            File.WriteAllText(Application.dataPath + "/SaveData.json", itemdata.ToString()); // 해당 경로에 Json 형태로 저장.
        }
    }
    public void Load_ItemJsonData()//쉘터 내 SaveItemData.json 데이터 불러오기 - UI 및 인벤토리
    {
        if (File.Exists(Application.dataPath + "/SaveItemData.json"))
        {
            string InventoryData = File.ReadAllText(Application.dataPath + "/SaveItemData.json");
            JsonData load_InventoryJson = JsonMapper.ToObject(InventoryData);
            LoadJson_Inventory(load_InventoryJson);
        }
        else
        {
            // 인벤토리를 저장할 Json 파일 조차 없어, 첫 시작한 유저로 인지하여 튜토리얼 진행.
            Debug.Log("튜토리얼");
        }
    }
    public void GameStart_JsonLoad() //게임시작시
    {
        //StageItem_Json();
    }
    public void GameOver_JsonSave() //게임오버시 or 클리어
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
    /// ex) Lv_Damage 데미지 레벨로 쉘터에서 달러를 지불하고 영구적으로 '데미지' 레벨 상승
    /// 이하 필요시 스텟 구매 가능. 스텟 표기 예시 = "Lv_" 해당 예시대로 필요시 추가.
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
    /// 쉘터 내 시설 정리 및  유무
    /// isCleanArea False -> True 이후 isBuildArea로 시설 Build 유무
    /// isBuildAreaList은 시설을 업그레이드 시킨다면 int형으로 레벨관리 필요. - 개발 고려 중
    /// 
    /// 위 언급으로 할려고했으나 
    /// 레벨별로 관리하기로함 ex 0 = 더미, 1 = 깨끗 , 2 = 시설 건축 , 3 = 부터 시설 업그레이드
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