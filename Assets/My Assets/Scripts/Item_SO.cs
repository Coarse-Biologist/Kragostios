using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Item_SO : ScriptableObject
{
    [SerializeField] private Rarity itemRarity = Rarity.Common;
    public Rarity ItemRarity => itemRarity;
    [SerializeField] private ItemType itemType = ItemType.Weapon;
    public ItemType ItemType => itemType;
    [SerializeField] private int itemValue = 0;
    public int ItemValue => itemValue;
    [SerializeField] private string itemName = "Item Name";
    public string ItemName => itemName;
    [SerializeField] private string itemDescription = "Item Description";
    public string ItemDescription => itemDescription;
    [SerializeField] private List<ItemSlot> itemSlot = new List<ItemSlot>();
    public List<ItemSlot> ItemSlot => itemSlot;

    #region // weapon stats
    [Header("Weapon Stats")]

    [SerializeField] private List<Handedness> wieldability = new List<Handedness> { Handedness.None };
    public List<Handedness> Wieldability => wieldability;
    [SerializeField] private Elements element = Elements.None;
    public Elements Element => element;
    [SerializeField] private int damage = 0;
    public int Damage => damage;
    [SerializeField] private int heal = 0;
    public int Heal => heal;
    [SerializeField] private List<Debuffs> debuffs = new List<Debuffs>();
    public List<Debuffs> Debuffs => debuffs;
    [SerializeField] private List<Buffs> Buffs = new List<Buffs>();
    public List<Buffs> buffs => Buffs;
    #endregion
    [Header("Armor Stats")]
    [SerializeField] private List<Buffs> armorBuffs = new List<Buffs>();
    public List<Buffs> ArmorBuffs => armorBuffs;
    [SerializeField] private int damageReduction = 0;
    public int DamageReduction => damageReduction;

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
