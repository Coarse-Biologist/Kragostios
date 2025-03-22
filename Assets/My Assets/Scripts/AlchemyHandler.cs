using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Globalization;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Item", menuName = "CrewObject/ Item")]
public class Ether_SO : ScriptableObject
{
    [SerializeField] public Elements element = Elements.None;

    [SerializeField] public bool IsPure = false;
}

public static class AlchemyHandler
{
    #region class variables
    [SerializeField] static Ether_SO PureEther;
    [SerializeField] static Ether_SO ImpureEther;

    public static Dictionary<AlchemyTools, bool> AvailableAlchemyTools { private set; get; } = new Dictionary<AlchemyTools, bool>();
    public static Dictionary<AutopsyTools, bool> AvailableAutopsyTools { private set; get; } = new Dictionary<AutopsyTools, bool>();

    public static Dictionary<Ether_SO, int> PlayerEther { private set; get; } = new Dictionary<Ether_SO, int> { };
    public static Dictionary<Elements, int> KnowledgeDict { private set; get; } = new Dictionary<Elements, int>();
    public static Dictionary<Elements, int> ElementalCoresDict { private set; get; } = new Dictionary<Elements, int>();

    public static bool CanExtractCores { private set; get; } = false;
    public static bool CanPurifyEther { private set; get; } = false;



    #endregion
    //void Awake()

    // make a list of all tools and add them to the dict, setting all values to false

    // make list of all elements, setting their value to 0 to represent a start knledge of 0

    // set player to have inventory of 0 pure and 0 impure ether, but containing the objects

    #region all alchemy functions

    private static int GetNumAlchemyToolsKnown()
    {
        int toolsKnown = 0;
        foreach (KeyValuePair<AlchemyTools, bool> kvp in AvailableAlchemyTools)
        {
            if (kvp.Value == true)
            {
                toolsKnown++;
            }
        }
        return toolsKnown;
    }
    private static int GetNumAutopsyToolsKnown()
    {
        int toolsKnown = 0;
        foreach (KeyValuePair<AutopsyTools, bool> kvp in AvailableAutopsyTools)
        {
            if (kvp.Value == true)
            {
                toolsKnown++;
            }
        }
        return toolsKnown;
    }
    public static void GainTool<T>(T toolofType) where T : Enum
    {
        if (typeof(T) == typeof(AutopsyTools))
        {
            AutopsyTools tool = (AutopsyTools)(object)toolofType; // Safe cast

            if (AvailableAutopsyTools.TryGetValue(tool, out bool has))
            {
                AvailableAutopsyTools[tool] = true;
            }
            else AvailableAutopsyTools.Add(tool, true);

        }
        if (typeof(T) == typeof(AlchemyTools))
        {
            AlchemyTools tool2 = (AlchemyTools)(object)toolofType; // Safe cast

            if (AvailableAlchemyTools.TryGetValue(tool2, out bool has))
            {
                AvailableAlchemyTools[tool2] = true;
            }
            else AvailableAlchemyTools.Add(tool2, true);
        }
    }
    public static string GainKnowledge(Elements element, bool fromAlchemy = true)
    {
        int toolsKnown = 0;
        if (fromAlchemy)
        {
            toolsKnown = GetNumAlchemyToolsKnown();
        }
        else toolsKnown = GetNumAutopsyToolsKnown();
        KnowledgeDict.TryGetValue(element, out int PlayerKnowledge);
        int knowledgeGain = 1;
        int knowledgeBonus = 2 ^ toolsKnown;
        knowledgeGain *= knowledgeBonus;

        if (KnowledgeDict.TryGetValue(element, out int knowledge)) KnowledgeDict[element] += knowledgeGain;
        else KnowledgeDict.Add(element, knowledgeGain);

        return $"You have gained {knowledgeGain} knowledge.";
    }
    #region core extraction functions
    public static void AlterCoreAmount(Elements element, int num = 1)
    {
        bool hasCores = ElementalCoresDict.TryGetValue(element, out int coresOwned);
        if (hasCores)
        {
            if (coresOwned + num > 0) ElementalCoresDict[element] += num;
        }
        else if (num > 0) ElementalCoresDict.Add(element, num);
        Debug.Log($"Player now has {coresOwned} cores of type {element}");
    }
    public static void EnableExtraction()
    {
        CanExtractCores = true;
    }
    private static bool AttemptExtractions(Tuple<Difficulty, Elements, string> enemyCombatantTuple)
    {
        float successChance = .3f;
        foreach (KeyValuePair<AutopsyTools, bool> toolsKvp in AvailableAutopsyTools)
        {

            if (toolsKvp.Value == true)
            {
                successChance += .1f;
            }
        }
        if (UnityEngine.Random.Range(0, 1) < successChance)
        {
            AlterCoreAmount(enemyCombatantTuple.Item2);
            return true;
        }
        else return false;
    }

    public static string HandleExtraction(Tuple<Difficulty, Elements, string> enemyCombatantTuple)
    {
        string stringResult = "";
        bool result = AttemptExtractions(enemyCombatantTuple);
        int numToolsKnown = GetNumAutopsyToolsKnown();
        string knowledgeGained = GainKnowledge(enemyCombatantTuple.Item2);
        if (result)
        {
            stringResult = $"With {numToolsKnown} extraction tools at your disposal, you succeeded in extracting the {enemyCombatantTuple.Item2} core from the {enemyCombatantTuple.Item3}. {knowledgeGained}";
        }
        else
        {
            stringResult = $"Having {numToolsKnown} extraction tools at your disposal, you failed in extracting the {enemyCombatantTuple.Item2} core from the {enemyCombatantTuple.Item3}. {knowledgeGained}";
        }
        return stringResult;
    }

    public static string HandleExamination(Tuple<Difficulty, Elements, string> enemyCombatantTuple)
    {
        string stringResult = "";
        //bool result = AttemptExtractions(enemyCombatantTuple);
        int numToolsKnown = GetNumAutopsyToolsKnown();
        string knowledgeGained = GainKnowledge(enemyCombatantTuple.Item2);
        stringResult = $"Having {numToolsKnown} examination tools at your disposal, you examined the {enemyCombatantTuple.Item3}. {knowledgeGained}";

        return stringResult;
    }

    public static string GetCoresOwnedString()
    {
        return string.Join(", ", ElementalCoresDict.Select(a => a.Key.ToString() + ":" + a.Value.ToString()));
    }
    public static string GetElementalKnowledgeString()
    {
        return string.Join(", ", KnowledgeDict.Select(a => a.Key.ToString() + ":" + a.Value.ToString()));
    }


    #endregion

    #region purification
    public static void EnablePurification()
    {
        CanPurifyEther = true;
    }



    private static string AttemptPurification(Ether_SO ether)
    {
        string result = "";
        if (!ether.IsPure)
        {
            float successChance = .2f;
            int toolsKnown = GetNumAlchemyToolsKnown();
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

    #endregion
    public static void LoadData()
    {
        AlchemyData alchemyData = SaveSystem.LoadAlchemyData();
        AvailableAlchemyTools = alchemyData.AvailableTools_SD;
        PlayerEther = alchemyData.PlayerEther_SD; // must be replaced with non-scriptable object data types
        KnowledgeDict = alchemyData.KnowledgeDict_SD;
    }
}




//if (Quests.IntAccomplishments.TryGetValue(Quests.QuestName.AttemptCoreExtraction, out int attempts))
//  {
//      if (attempts == 0)
//      {
//return $"Hmmmm, let me see if I can extract this strange {enemyCombatantTuple.Item2} object. Can i just pull it out? \n Ack! it //just shreds in my hands at the pull.";
//      }
//      if (attempts == 1)
//return $"Let's give it another try. I need a better look at this {enemyCombatantTuple.Item2} thing. Gentler this time... \n //Damn... That worked a little better, but it is still quite fallen apart.";
//
//      if (attempts == 2)
//      {
//return $"One last try. Slow and steady. \n Pull, now here, now there... No! It really doesnt like to be pulled on. I need a way that doesnt put it under such tension. A shame, it would have been wondrous to examine this {enemyCombatantTuple.Item2} variant \n //maybe I should try a knife or something.";
//      }
//      else
//      {
//          return $"Your knowledge and tools allowed you to successfully extract the {enemyCombatantTuple.Item2} core!";
//      }
//  }
//else return "You dont have the mind or experience yet to begin extracting";
// playerEther must be replaced with non-scriptable object data types



