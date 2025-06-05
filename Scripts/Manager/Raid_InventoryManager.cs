using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Raid_InventoryManager : MonoBehaviour
{
    public SaveJSonData getItemList;
    public Transform itemInventory_parent;
    public Transform inventory_GetUI_parent;

    public GameObject inventory_ItemIcon;
    public GetUI_ItemElement inventory_GetUI_Icon;

    public List<GameObject> inventorySlot = new List<GameObject>();
   
    public void Get_Item_InventoryUpdate()
    {
        int value = getItemList.itemNameList.Count;
        for (int i = 0; i< value; i++)
        {
            for(int j = 0; j < getItemList.itemCountList[i]; j++)
            {
                GameObject itemIcon = Instantiate(inventory_ItemIcon, itemInventory_parent);
                itemIcon.GetComponent<Image>().sprite = getItemList.itemImage_List[i];

               
                inventorySlot.Add(itemIcon);
            }
            
        }
    }

    public void Add_Inventory(ItemDatabase _itemData)
    {
        GameObject itemIcon = Instantiate(inventory_ItemIcon, itemInventory_parent);
        itemIcon.GetComponent<Image>().sprite = _itemData.itemSptrie;

        GameObject GetUIIcon = Instantiate(inventory_GetUI_Icon.gameObject, inventory_GetUI_parent);
        GetUI_ItemElement GetUI = GetUIIcon.GetComponent<GetUI_ItemElement>();
        GetUI.GetUI_Icon.sprite = _itemData.itemSptrie;
        GetUI.GetUI_maskImage.color = Color.white;

        Destroy(GetUIIcon, 0.8f);

        inventorySlot.Add(itemIcon);
    }
}

