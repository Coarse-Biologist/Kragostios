using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Ether_SO : ScriptableObject
{
    [SerializeField] public Elements element = Elements.None;

    [SerializeField] public bool IsPpure = false;
}

public static class AlchemyHandler
{
    #region class variables
    [SerializeField] static Ether_SO PureEther;
    [SerializeField] static Ether_SO ImpureEther;

    public static Dictionary<AlchemyTools, bool> AvailableTools { private set; get; } = new Dictionary<AlchemyTools, bool>();
    public static Dictionary<Ether_SO, int> PlayerEther { private set; get; } = new Dictionary<Ether_SO, int> { };
    public static Dictionary<Elements, int> KnowledgeDict { private set; get; } = new Dictionary<Elements, int>();


    #endregion
    //void Awake()

    // make a list of all tools and add them to the dict, setting all values to false

    // make list of all elements, setting their value to 0 to represent a start knledge of 0

    // set player to have inventory of 0 pure and 0 impure ether, but containing the objects

    #region alchemy functions
    public static List<T> GetAllEnums<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }
    private static int GetNumToolsKnown()
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
    public static string GainKnowledge(Elements element)
    {
        KnowledgeDict.TryGetValue(element, out int PlayerKnowledge);
        int knowledgeGain = 1;
        int toolsKnown = GetNumToolsKnown();
        int knowledgeBonus = 2 ^ toolsKnown;
        knowledgeGain *= knowledgeBonus;

        KnowledgeDict[element] += knowledgeGain;
        return $"You have gained {knowledgeGain} knowledge.";
    }
    private static string AttemptPurification(Ether_SO ether)
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
    private static void PurifyEther(Ether_SO impureEther)
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
    public static void LoadData()
    {
        AlchemyData alchemyData = SaveSystem.LoadAlchemyData();
        AvailableTools = alchemyData.AvailableTools_SD;
        PlayerEther = alchemyData.PlayerEther_SD; // must be replaced with non-scriptable object data types
        KnowledgeDict = alchemyData.KnowledgeDict_SD;
    }
}

// playerEther must be replaced with non-scriptable object data types


