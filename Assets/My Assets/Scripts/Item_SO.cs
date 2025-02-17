using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;

using System;
using UnityEditor.Build.Pipeline;


[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Item_SO : ScriptableObject
{
    [SerializeField] AbilityLibrary abilityLibrary;
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

    [SerializeField] private Ability_SO ability;
    public Ability_SO Ability => ability;

    #endregion
    [Header("Armor Stats")]
    [SerializeField] private List<Buffs> armorBuffs = new List<Buffs>();
    public List<Buffs> ArmorBuffs => armorBuffs;
    [SerializeField] private int damageReduction = 0;
    public int DamageReduction => damageReduction;

    public Ability_SO GetItemsAbility()
    {
        if (ability != null)
        {
            return ability;
        }
        else return null;

    }

    public string GetItemInfo()
    {
        string itemInfo = $"{itemName} || {itemDescription} || value: {itemValue} || {itemRarity}";
        switch (itemType)
        {

            case ItemType.Armor:
                string buffList = "Confers buffs to wearer:";
                foreach (Buffs buff in armorBuffs)
                {
                    buffList += $"|| {buff} || ";
                }
                itemInfo += $" \n Damage Reduction: {damageReduction}";
                break;
            case ItemType.Weapon:
                string weaponAbilityInfo = "";
                if (ability != null)
                {
                    if (abilityLibrary != null)
                    {
                        weaponAbilityInfo = abilityLibrary.GetAbilityInfo(ability);
                    }
                    else weaponAbilityInfo = ability.AbilityName;
                }
                itemInfo += weaponAbilityInfo;

                break;
            case ItemType.Potion:
                string potionAbilityInfo = "";
                if (ability != null)
                {
                    if (abilityLibrary != null)
                    {
                        potionAbilityInfo += abilityLibrary.GetAbilityInfo(ability);
                    }
                    else potionAbilityInfo += ability.AbilityName;
                }
                itemInfo += potionAbilityInfo;

                break;
            case ItemType.Scroll:
                string scrollAbilityInfo = "";
                if (ability != null)
                {
                    if (abilityLibrary != null)
                    {
                        scrollAbilityInfo += abilityLibrary.GetAbilityInfo(ability);
                    }
                    else scrollAbilityInfo += ability.AbilityName;
                }
                itemInfo += scrollAbilityInfo;                //string scrollDebuffList = "Can apply debuffs:";

                break;
            case ItemType.Quest:
                break;
            default:
                break;

        }
        return itemInfo;
    }

}

