using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public static class Quests
{
    public static Dictionary<QuestName, bool> BoolAccomplishments;
    public static Dictionary<QuestName, int> IntAccomplishments;
    public static List<QuestName> boolQuests;
    public static List<QuestName> repeatableQuests;
    public static List<QuestName> questOrder = new List<QuestName> { QuestName.DefeatEnemies };
    public static Dictionary<QuestName, Tuple<List<QuestName>, Dictionary<QuestName, int>>> RequisiteDict;

    //track progress method. stores and sets progress in different skillsand knowledges

    //qualification tracker. tracks what quests one is qualified for. if one is qualified for multiple, what happens? 

    // when does it matter that one is qualified for one  more more quests? 

    // when should i check whether a quest is complete?

    public static void SetRequisiteDict()
    {
        List<QuestName> boolAccolpishment1 = new List<QuestName> { QuestName.PerformCoreExtraction, QuestName.PerformEtherPurification };
        Dictionary<QuestName, int> intAccolpishment1 = new Dictionary<QuestName, int>();

        var reqTuple1 = new Tuple<List<QuestName>, Dictionary<QuestName, int>>(boolAccolpishment1, intAccolpishment1);

        RequisiteDict.Add(QuestName.LearnGlassMaking, reqTuple1);
    }

    public static void IncrementIntQuests(QuestName quest, int increments = 1)
    {
        if (IntAccomplishments.TryGetValue(quest, out int num))
        {
            num += increments;
        }
    }

    public static void CompleteQuest(QuestName quest)
    {
        if (BoolAccomplishments.TryGetValue(quest, out bool complete))
        {
            complete = true;
        }
    }

    public static bool CheckQualification(QuestName questName)
    {
        bool qualified = false;
        if (boolQuests.Contains(questName))
        {
            if (BoolAccomplishments[questName] == true)
            {
                qualified = true;
            }
            else return false;
        }
        else if (repeatableQuests.Contains(questName))
        {
            if (BoolAccomplishments[questName] == true)
            {
                qualified = true;
            }
            else return false;
        }


        return qualified;
    }
    public enum Skills
    {
        Welding,
        GlassMaking,
        Sewing,
        Alchemy,
        Chemistry,
        MechanicalEngineering
    }
    public enum QuestName
    {
        DefeatEnemies,
        UsePotions,
        UseAbilities,
        UseHealAbilities,
        PerformCoreExtraction,
        PerformEtherPurification,
        LearnGlassMaking,
        LearnWelding,
        LearnSewing,
        LearnAlchemy,
        LearnChemistry,
        LearnCoreExtractiion,
        LearnPotionCrafting,
        LearnArrowCrafting,
        LearnArmorCrafting,
        LearnWeaponCrafting,
        AchieveKnowledge,


    }
}
