using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Item_SO : ScriptableObject
{
    public Rarity itemRarity = Rarity.Common;
    public ItemType itemType = ItemType.Weapon;
    public int itemValue = 0;
    public string itemName = "Item Name";
    public string itemDescription = "Item Description";

    #region // weapon stats

    [Header("Weapon Stats")]
    public Elements element = Elements.None;
    public int damage = 0;
    public int heal = 0;
    public List<Debuffs> debuffs = new List<Debuffs>();
    public List<Buffs> Buffs = new List<Buffs>();

    #endregion

    [Header("Armor Stats")]
    public List<Buffs> armorBuffs = new List<Buffs>();
    public int damageReduction = 0;

    public string GetItemInfo()
    {
        string itemInfo = $"{itemName} || {itemDescription} || value: {itemValue} || {itemRarity}";
        switch (itemType)
        {
            case ItemType.Armor:
                string buffList = "Can apply buffs:";
                foreach (Buffs buff in armorBuffs)
                {
                    buffList += $"|| {buff} || ";
                }
                itemInfo += $" \n Damage Reduction: {damageReduction}";
                break;
            case ItemType.Weapon:
                string debuffList = "Can apply debuffs:";
                foreach (Debuffs debuff in debuffs)
                {
                    debuffList += $"|| {debuff} || ";
                }
                string weaponBuffList = "Can apply buffs:";
                foreach (Buffs buff in Buffs)
                {
                    weaponBuffList += $"|| {buff} || ";
                }
                itemInfo += $" \n Damage: {damage} \n Heal: {heal} \n {debuffList} \n {weaponBuffList}";
                break;
            case ItemType.Potion:
                string potionDebuffList = "Can apply debuffs:";

                foreach (Debuffs debuff in debuffs)
                {
                    potionDebuffList += $"|| {debuff} || ";
                }
                string potionBuffList = "Can apply buffs:";
                foreach (Buffs buff in Buffs)
                {
                    potionBuffList += $"|| {buff} || ";
                }
                itemInfo += $" \n Damage: {damage} \n Heal: {heal} \n {potionDebuffList} \n {potionBuffList}";
                break;
            case ItemType.Scroll:
                string scrollDebuffList = "Can apply debuffs:";

                foreach (Debuffs debuff in debuffs)
                {
                    scrollDebuffList += $"|| {debuff} || ";
                }
                string scrollBuffList = "Can apply buffs:";
                foreach (Buffs buff in Buffs)
                {
                    scrollBuffList += $"|| {buff} || ";
                }
                itemInfo += $" \n Damage: {damage} \n Heal: {heal} \n {scrollDebuffList} \n {scrollBuffList}";
                break;
            case ItemType.Quest:
                break;
            default:
                break;

        }
        return itemInfo;
    }

}
