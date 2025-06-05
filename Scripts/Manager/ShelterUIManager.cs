using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterUIManager : MonoBehaviour
{
    public ShelterPlayerCtrl playerManaer;
    public SaveJSonData inventoryData_Shelter;

    public GameObject[] ShopUI_Array;
    public GameObject shopTitle;
    public GameObject shopCloseButton;

    public GameObject upgradeUI;
    public GameObject settingUI;
    
    public Text nowDollar;
    public void Update()
    {
        nowDollar.text = "$"+inventoryData_Shelter.nowDollar.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            __Button_SettingUI_Active(true);
        }
    }
    public void __Button_SettingUI_Active(bool _active)
    {
        settingUI.SetActive(_active);
        if (!_active)
        {
            playerManaer.isShoping = false;
        }
        else
        {
            playerManaer.isShoping = true;
        }
    }
    public void __Button_Upgrade_Active(bool _active)
    {
        if (_active) this.GetComponent<ItemDataManager>().Next_Upgrade();

        upgradeUI.SetActive(_active);
        if (!_active)
        {
            playerManaer.isShoping = false;
        }
        else
        {
            playerManaer.isShoping = true;
        }
    }
  
    public void __Button_OpenShop(int _num)
    {
        ShopUI_Array[_num].SetActive(true);
        shopTitle.SetActive(true);
        shopCloseButton.SetActive(true);
    }
    public void __Button_CloseShop()
    {
        playerManaer.isShoping = false;

        shopTitle.SetActive(false);
        shopCloseButton.SetActive(false);

        for (int i = 0; i < ShopUI_Array.Length; i++)
        {
            ShopUI_Array[i].SetActive(false);
        }
    }
    public void __Button_Sell_Item(string _itemName,int _sellValue)
    {
        if (inventoryData_Shelter.inventory_itemName.Contains(_itemName)) 
        {
            for (int i = 0; i < inventoryData_Shelter.inventory_itemName.Count; i++)
            {
                if (inventoryData_Shelter.inventory_itemName[i] == _itemName)
                {
                    inventoryData_Shelter.nowDollar += _sellValue;

                    inventoryData_Shelter.inventory_itemValue[i] -= 1;
                    inventoryData_Shelter.SAVE_JsonData();

                    if (inventoryData_Shelter.inventory_itemValue[i] == 0)
                    {
                        inventoryData_Shelter.inventory_itemName.Remove(inventoryData_Shelter.inventory_itemName[i]);
                        inventoryData_Shelter.inventory_itemValue.Remove(inventoryData_Shelter.inventory_itemValue[i]);
                        inventoryData_Shelter.SAVE_JsonData();
                    }
                }
            }
        }
        else //인벤에 없는 템(처음) 습득시
        {
            Debug.Log("팔고자 하는 아이템이 없습니다.");
        }
    }
    public int itemValue(string _itemName)
    {
        int value = 0;
        if (inventoryData_Shelter.inventory_itemName.Contains(_itemName))
        {
            for (int i = 0; i < inventoryData_Shelter.inventory_itemName.Count; i++)
            {
                if (inventoryData_Shelter.inventory_itemName[i] == _itemName)
                {
                    if (inventoryData_Shelter.inventory_itemValue[i] == 0)
                    {
                        value = 0;
                    }
                    else
                    {
                        value = inventoryData_Shelter.inventory_itemValue[i];
                    }
                }
            }
        }
        return value;
    }
    
}
