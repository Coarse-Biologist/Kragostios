using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;
using System;

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
