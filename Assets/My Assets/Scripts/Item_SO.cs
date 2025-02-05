using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Item_SO : ScriptableObject
{
    public Rarity rarity;
    public ItemType itemType;
    public string itemName;
    public int itemValue;
    public string itemDescription;

}
