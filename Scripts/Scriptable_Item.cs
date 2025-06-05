using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Scriptable_Item : ScriptableObject
{
    public string itemName;
    public int itemCode;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public enum ItemType
    {
        Money,
        Material
    }
}
