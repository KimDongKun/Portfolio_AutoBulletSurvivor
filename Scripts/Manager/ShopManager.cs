using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<ShopInfoData> sellButtons = new List<ShopInfoData>();
    public ItemDataManager itemdataManager;
    public ShelterUIManager SellManager;
    public AudioLimit clickSound;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< sellButtons.Count; i++)
        {
            int n = i;
            int itemID = sellButtons[n].itemID;
            sellButtons[i].sellText.text = "$" + sellButtons[i].sellValue.ToString();
            sellButtons[i].itemValue_Text.text = "x"+ SellManager.itemValue(itemdataManager.itemName_Array[itemID]).ToString();
            sellButtons[i].sellButton.onClick.RemoveAllListeners();
            sellButtons[i].sellButton.onClick.AddListener(delegate { __Add_Sell_Button(sellButtons[n], itemID, sellButtons[n].sellValue); });
            sellButtons[i].sellButton.onClick.AddListener(delegate { __Add_buttonSound(); });
        }
    }
    public void __Add_Sell_Button(ShopInfoData _shopInfo, int _num, int _sell)
    {
        SellManager.__Button_Sell_Item(itemdataManager.itemName_Array[_num], _sell);
        _shopInfo.itemValue_Text.text = "x" + SellManager.itemValue(itemdataManager.itemName_Array[_num]).ToString();
        //_shopInfo.itemValue_Text.text = SellManager.inventoryData_Shelter.inventory_itemValue[_num].ToString();
    }
  
    public void __Add_buttonSound()
    {
        clickSound.ExplodePlaySound(0);
    }
}
[System.Serializable]
public class ShopInfoData
{
    public Button sellButton;
    public Text sellText;
    public Text itemValue_Text;
    public int itemID;
    public int sellValue;
    public int itemValue;
    /*public ShopInfoData(Button _button,Text _sellText, int _itemID, int _sellValue)
    {
        sellButton = _button;
        sellText = _sellText;
        itemID = _itemID;
        sellValue = _sellValue;
    }*/
}