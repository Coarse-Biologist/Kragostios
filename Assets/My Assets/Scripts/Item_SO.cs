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

//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;
//using KragostiosAllEnums;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Ether_SO : ScriptableObject
{
    [SerializeField] public Elements element = Elements.None;

    [SerializeField] public bool IsPpure = false;
}

public class AlchemyHandler : MonoBehaviour
{
    [SerializeField] Ether_SO PureEther;
    [SerializeField] Ether_SO ImpureEther;

    private readonly StatsHandler PlayerStats;
    private readonly Dictionary<AlchemyTools, bool> AvailableTools = new Dictionary<AlchemyTools, bool>();
    private Dictionary<Ether_SO, int> PlayerEther = new Dictionary<Ether_SO, int> { };
    private Dictionary<Elements, int> KnowledgeDict = new Dictionary<Elements, int>();

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

    private void AttemptPurification(Ether_SO ether)
    {
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
            }
            else GainKnowledge(ether.element);
        }
        else KDebug.SeekBug("Ether is already pure brah");
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
}