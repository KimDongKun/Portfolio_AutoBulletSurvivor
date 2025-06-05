using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabCtrl : MonoBehaviour
{
    public GameObject[] itemList_Class1; //일반 등급
    public GameObject[] itemList_Class2; //레어 등급
    public GameObject[] itemList_Class3; //에픽 등급
    public GameObject[] itemList_Class4; //레전드 등급

    // Start is called before the first frame update
    void Start()
    {
        //여기서 생산 및 관리 해줘야할듯
        GetItemClass();
    }

    void GetItemClass()
    {
        int _min = (int)GameStageManager.timer / 60 % 60;
        switch (_min)
        {
            case 2:
                ItemSpawnMangement(itemList_Class1);
                break;
            case 4:
                ItemSpawnMangement(itemList_Class1);
                break;
            case 6:
                ItemSpawnMangement(itemList_Class2);
                break;
            case 8:
                ItemSpawnMangement(itemList_Class2);
                break;
            case 10:
                ItemSpawnMangement(itemList_Class2);
                break;
            case 12:
                ItemSpawnMangement(itemList_Class3);
                break;
            case 14:
                ItemSpawnMangement(itemList_Class3);
                break;
            case 16:
                ItemSpawnMangement(itemList_Class3);
                break;
            case 17:
                ItemSpawnMangement(itemList_Class4);
                break;
            case 18:
                ItemSpawnMangement(itemList_Class4);
                break;
        }
    }

    void ItemSpawnMangement(GameObject[] _itemList)
    {
        int _random = Random.Range(0, _itemList.Length);
        Instantiate(_itemList[_random]);
    }
}
