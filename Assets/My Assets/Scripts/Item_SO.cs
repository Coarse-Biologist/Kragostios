using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Item_SO : ScriptableObject
{
    public string itemName;
    public int itemValue;
    public string itemDescription;

}
