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


[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Ether_SO : ScriptableObject
{
    [SerializeField] public Elements element = Elements.None;

    [SerializeField] public bool IsPpure = false;
}

public class AlchemyHandler : MonoBehaviour
{
    #region class variables
    [SerializeField] Ether_SO PureEther;
    [SerializeField] Ether_SO ImpureEther;

    private readonly StatsHandler PlayerStats;
    private readonly Dictionary<AlchemyTools, bool> AvailableTools = new Dictionary<AlchemyTools, bool>();
    private Dictionary<Ether_SO, int> PlayerEther = new Dictionary<Ether_SO, int> { };
    private Dictionary<Elements, int> KnowledgeDict = new Dictionary<Elements, int>();

    #endregion
    void Awake()
    {
        List<AlchemyTools> tools = GetAllEnums<AlchemyTools>();
        foreach (AlchemyTools tool in tools)
        {
            AvailableTools.TryAdd(tool, false);
        }

        List<Elements> elements = GetAllEnums<Elements>();
        foreach (Elements element in elements)
        {
            KnowledgeDict.TryAdd(element, 0);
        }

        PlayerEther.TryAdd(PureEther, 0);
        PlayerEther.TryAdd(ImpureEther, 0);
    }
    #region alchemy functions
    public static List<T> GetAllEnums<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
    private int GetNumToolsKnown()
    {
        int toolsKnown = 0;
        foreach (KeyValuePair<AlchemyTools, bool> kvp in AvailableTools)
        {
            if (kvp.Value == true)
            {
                toolsKnown++;
            }
        }
        return toolsKnown;
    }
    public string GainKnowledge(Elements element)
    {
        KnowledgeDict.TryGetValue(element, out int PlayerKnowledge);


        int knowledgeGain = 1;
        int toolsKnown = GetNumToolsKnown();
        int knowledgeBonus = 2 ^ toolsKnown;
        knowledgeGain *= knowledgeBonus;

        KnowledgeDict[element] += knowledgeGain;
        return $"You have gained {knowledgeGain} knowledge.";
    }
    private string AttemptPurification(Ether_SO ether)
    {
        string result = "";
        if (!ether.IsPpure)
        {
            float successChance = .2f;
            int toolsKnown = GetNumToolsKnown();
            successChance += .05f * toolsKnown;
            float diceRoll = UnityEngine.Random.Range(0, 1);
            if (successChance > diceRoll)
            {
                PurifyEther(ether);
                GainKnowledge(ether.element);
                result = "Some ether has been successfully purified";
            }
            else
            {
                GainKnowledge(ether.element);
                result = "This round of ether purification failed";
            }
        }
        else KDebug.SeekBug("Ether is already pure brah");
        return result;
    }
    private void PurifyEther(Ether_SO impureEther)
    {
        if (PlayerEther.TryGetValue(impureEther, out int amount))
        {
            if (amount > 0)
            {
                PlayerEther[impureEther]--;
                PlayerEther[PureEther]++;
            }
        }
    }
    #endregion


}

public class ModdedAbilities
{
    #region class variables
    public Dictionary<Ability_SO, Dictionary<AbilityVars, int>> AbilityIntMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, int>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, string>> AbilityStringMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, string>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>> AbilityResourceMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>> AbilityElementMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>> AbilityBuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>> AbilityDebuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>>();
    #endregion

    #region modify variables

    public void ModifyIntAttribute(Ability_SO ability, AbilityVars var, int amount) // can be used for AbilityCost, HealValue, DamageValue, TurnDuration, Targets
    {
        if (AbilityIntMods.TryGetValue(ability, out Dictionary<AbilityVars, int> intModDict))
        {
            if (intModDict.TryGetValue(var, out int previousValue))
            {
                intModDict[var] += amount;
            }
            else intModDict.TryAdd(var, amount);
        }
        else
        {
            AbilityIntMods.TryAdd(ability, new Dictionary<AbilityVars, int>());
            intModDict.TryAdd(var, amount);
        }
    }
    public void ModifyStringAttribute(Ability_SO ability, AbilityVars var, string newString) // can be used for name and description
    {
        if (AbilityStringMods.TryGetValue(ability, out Dictionary<AbilityVars, string> stringModDict))
        {
            if (stringModDict.TryGetValue(var, out string previousValue))
            {
                stringModDict[var] = newString;
            }
            else stringModDict.TryAdd(var, newString);
        }
        else
        {
            AbilityStringMods.TryAdd(ability, new Dictionary<AbilityVars, string>());
            stringModDict.TryAdd(var, newString);
        }
    }
    public void ModifyResourceAttribute(Ability_SO ability, AbilityVars var, ResourceTypes resource) // can be used for name and description
    {
        if (AbilityResourceMods.TryGetValue(ability, out Dictionary<AbilityVars, ResourceTypes> resourceModDict))
        {
            if (resourceModDict.TryGetValue(var, out ResourceTypes previousResource))
            {
                resourceModDict[var] = resource;
            }
            else resourceModDict.TryAdd(var, resource);
        }
        else
        {
            AbilityResourceMods.TryAdd(ability, new Dictionary<AbilityVars, ResourceTypes>());
            resourceModDict.TryAdd(var, resource);
        }
    }
    public void ModifyElementAttribute(Ability_SO ability, AbilityVars var, Elements element) // can be used for name and description
    {
        if (AbilityElementMods.TryGetValue(ability, out Dictionary<AbilityVars, Elements> elementModDict))
        {
            if (elementModDict.TryGetValue(var, out Elements previousValue))
            {
                elementModDict[var] = element;
            }
            else elementModDict.TryAdd(var, element);
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            elementModDict.TryAdd(var, element);
        }
    }
    public void ModifyBuffsAttribute(Ability_SO ability, AbilityVars var, Buffs buff) // can be used for name and description
    {
        if (AbilityBuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Buffs>> buffModDict))
        {
            if (buffModDict.TryGetValue(var, out List<Buffs> previousBuffsList))
            {
                buffModDict[var].Add(buff);
            }
            else buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
    }
    public void ModifyDebuffsAttribute(Ability_SO ability, AbilityVars var, Debuffs debuff) // can be used for name and description
    {
        if (AbilityDebuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Debuffs>> debuffModDict))
        {
            if (debuffModDict.TryGetValue(var, out List<Debuffs> debuffsList))
            {
                debuffModDict[var].Add(debuff);
            }
            else debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
    }
    #endregion

    #region get modded values

    public int GetModdedInt(Ability_SO ability, AbilityVars var)
    {

        if (AbilityIntMods.TryGetValue(ability, out Dictionary<AbilityVars, int> innerDict))
        {
            if (innerDict.TryGetValue(var, out int value))
            {
                return value;
            }
            else return 0;
        }
        else return 0;
    }
    public string GetModdedString(Ability_SO ability, AbilityVars var)
    {

        if (AbilityStringMods.TryGetValue(ability, out Dictionary<AbilityVars, string> innerDict))
        {
            if (innerDict.TryGetValue(var, out string value))
            {
                return value;
            }
            else return "None";
        }
        else return "None";
    }
    public Elements GetModdedElement(Ability_SO ability, AbilityVars var)
    {

        if (AbilityElementMods.TryGetValue(ability, out Dictionary<AbilityVars, Elements> innerDict))
        {
            if (innerDict.TryGetValue(var, out Elements value))
            {
                return value;
            }
            else return Elements.None;
        }
        else return Elements.None;
    }
    public ResourceTypes GetModdedResource(Ability_SO ability, AbilityVars var)
    {

        if (AbilityResourceMods.TryGetValue(ability, out Dictionary<AbilityVars, ResourceTypes> innerDict))
        {
            if (innerDict.TryGetValue(var, out ResourceTypes value))
            {
                return value;
            }
            else return ResourceTypes.None;
        }
        else return ResourceTypes.None;
    }

    public List<Buffs> GetModdedBuffs(Ability_SO ability, AbilityVars var)
    {

        if (AbilityBuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Buffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Buffs> value))
            {
                return value;
            }
            else return new List<Buffs>();
        }
        else return new List<Buffs>();
    }

    public List<Debuffs> GetModdedDebuffs(Ability_SO ability, AbilityVars var)
    {

        if (AbilityDebuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Debuffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Debuffs> value))
            {
                return value;
            }
            else return new List<Debuffs>();
        }
        else return new List<Debuffs>();
    }

    #endregion
}

public class ModdedItems
{

    #region class variables
    public Dictionary<Item_SO, Dictionary<ItemVars, int>> ItemIntMods = new Dictionary<Item_SO, Dictionary<ItemVars, int>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, string>> ItemStringMods = new Dictionary<Item_SO, Dictionary<ItemVars, string>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>> ItemResourceMods = new Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, Elements>> ItemElementMods = new Dictionary<Item_SO, Dictionary<ItemVars, Elements>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>> ItemBuffMods = new Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>> ItemDebuffMods = new Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>>();
    #endregion

    #region modify variables
    public void ModifyStringAttribute(Item_SO item, ItemVars var, string newValue) // can be used for name and description
    {
        if (ItemStringMods.TryGetValue(item, out Dictionary<ItemVars, string> stringModDict))
        {
            if (stringModDict.TryGetValue(var, out string previousValue))
            {
                stringModDict[var] = newValue;
            }
            else stringModDict.TryAdd(var, newValue);
        }
        else
        {
            ItemStringMods.TryAdd(item, new Dictionary<ItemVars, string>());
            stringModDict.TryAdd(var, newValue);
        }
    }
    public void ModifyIntAttribute(Item_SO item, ItemVars var, int newValue) // can be used for name and description
    {
        if (ItemIntMods.TryGetValue(item, out Dictionary<ItemVars, int> intModDict))
        {
            if (intModDict.TryGetValue(var, out int previousValue))
            {
                intModDict[var] = newValue;
            }
            else intModDict.TryAdd(var, newValue);
        }
        else
        {
            ItemIntMods.TryAdd(item, new Dictionary<ItemVars, int>());
            intModDict.TryAdd(var, newValue);
        }
    }
    public void ModifyBuffsAttribute(Item_SO item, ItemVars var, Buffs buff) // can be used for name and description
    {
        if (ItemBuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Buffs>> buffModDict))
        {
            if (buffModDict.TryGetValue(var, out List<Buffs> previousBuffsList))
            {
                buffModDict[var].Add(buff);
            }
            else buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
        else
        {
            ItemBuffMods.TryAdd(item, new Dictionary<ItemVars, List<Buffs>>());
            buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
    }
    public void ModifyDebuffsAttribute(Item_SO item, ItemVars var, Debuffs debuff) // can be used for name and description
    {
        if (ItemDebuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Debuffs>> debuffModDict))
        {
            if (debuffModDict.TryGetValue(var, out List<Debuffs> previousdebuffsList))
            {
                debuffModDict[var].Add(debuff);
            }
            else debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
        else
        {
            ItemDebuffMods.TryAdd(item, new Dictionary<ItemVars, List<Debuffs>>());
            debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
    }
    public void ModifyResourceAttribute(Item_SO item, ItemVars var, ResourceTypes resource) // can be used for name and description
    {
        if (ItemResourceMods.TryGetValue(item, out Dictionary<ItemVars, ResourceTypes> resourceModDict))
        {
            if (resourceModDict.TryGetValue(var, out ResourceTypes previousValue))
            {
                resourceModDict[var] = resource;
            }
            else resourceModDict.TryAdd(var, resource);
        }
        else
        {
            ItemElementMods.TryAdd(item, new Dictionary<ItemVars, Elements>());
            resourceModDict.TryAdd(var, resource);
        }
    }
    public void ModifyElementAttribute(Item_SO item, ItemVars var, Elements element) // can be used for name and description
    {
        if (ItemElementMods.TryGetValue(item, out Dictionary<ItemVars, Elements> elementModDict))
        {
            if (elementModDict.TryGetValue(var, out Elements previousValue))
            {
                elementModDict[var] = element;
            }
            else elementModDict.TryAdd(var, element);
        }
        else
        {
            ItemElementMods.TryAdd(item, new Dictionary<ItemVars, Elements>());
            elementModDict.TryAdd(var, element);
        }
    }
    #endregion

    #region get modded values
    public int GetModdedInt(Item_SO item, ItemVars var)
    {

        if (ItemIntMods.TryGetValue(item, out Dictionary<ItemVars, int> innerDict))
        {
            if (innerDict.TryGetValue(var, out int value))
            {
                return value;
            }
            else return 0;
        }
        else return 0;
    }
    public string GetModdedString(Item_SO item, ItemVars var)
    {

        if (ItemStringMods.TryGetValue(item, out Dictionary<ItemVars, string> innerDict))
        {
            if (innerDict.TryGetValue(var, out string value))
            {
                return value;
            }
            else return "None";
        }
        else return "None";
    }
    public Elements GetModdedElement(Item_SO item, ItemVars var)
    {

        if (ItemElementMods.TryGetValue(item, out Dictionary<ItemVars, Elements> innerDict))
        {
            if (innerDict.TryGetValue(var, out Elements value))
            {
                return value;
            }
            else return Elements.None;
        }
        else return Elements.None;
    }
    public ResourceTypes GetModdedResource(Item_SO item, ItemVars var)
    {

        if (ItemResourceMods.TryGetValue(item, out Dictionary<ItemVars, ResourceTypes> innerDict))
        {
            if (innerDict.TryGetValue(var, out ResourceTypes value))
            {
                return value;
            }
            else return ResourceTypes.None;
        }
        else return ResourceTypes.None;
    }
    public List<Buffs> GetModdedBuffs(Item_SO item, ItemVars var)
    {

        if (ItemBuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Buffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Buffs> value))
            {
                return value;
            }
            else return new List<Buffs>();
        }
        else return new List<Buffs>();
    }
    public List<Debuffs> GetModdedDebuffs(Item_SO item, ItemVars var)
    {

        if (ItemDebuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Debuffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Debuffs> value))
            {
                return value;
            }
            else return new List<Debuffs>();
        }
        else return new List<Debuffs>();
    }
    #endregion

}