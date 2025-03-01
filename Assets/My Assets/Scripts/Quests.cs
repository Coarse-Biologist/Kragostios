using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Quests
{
    public static Dictionary<QuestName, bool> BoolAccomplishments;
    public static Dictionary<QuestName, int> IntAccomplishments;
    public static List<QuestName> boolQuests;
    public static List<QuestName> repeatableQuests;
    public static List<QuestName> questOrder = new List<QuestName> { QuestName.DefeatEnemies };
    public static Dictionary<QuestName, ValueTuple<List<QuestName>, Dictionary<QuestName, int>>> RequisiteDict;
    public static List<ValueTuple<QuestName, int>> QuestsInOrder = new List<ValueTuple<QuestName, int>>();

    //track progress method. stores and sets progress in different skillsand knowledges

    //qualification tracker. tracks what quests one is qualified for. if one is qualified for multiple, what happens? 

    // when does it matter that one is qualified for one  more more quests? 

    // when should i check whether a quest is complete?

    public static void SetOrderOfQuests()
    {
        QuestsInOrder = new List<ValueTuple<QuestName, int>>()
        {
            (QuestName.DefeatEnemies, 3),
            (QuestName.ExamineBodies, 5),
            (QuestName.AttemptCoreExtraction, 1),
            (QuestName.FindAKnife, 1),
            (QuestName.ExamineCores, 5),
            (QuestName.PracticalExperiments, 1 ),
            (QuestName.LearnGlassMaking, 1)

        };


    }

    public static string PresentQuest_DefeatEnemies()
    {
        string questIntro = "";
        return questIntro;
    }




    public static void IncrementIntQuests(QuestName quest, int increments = 1)
    {

        if (IntAccomplishments.TryGetValue(quest, out int num))
        {
            IntAccomplishments[quest] += increments;
        }
    }

    public static void CompleteQuest(QuestName quest)
    {
        if (BoolAccomplishments.TryGetValue(quest, out bool complete))
        {
            BoolAccomplishments[quest] = true;
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
        ExamineBodies,
        AttemptCoreExtraction,
        FindAKnife,
        ExamineCores,
        PracticalExperiments, // this will be a step in which the player is asked to use cores and attempt purification
        UseCores,
        AttemptPurifications,
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
        LearnPotionCrafting,
        LearnArrowCrafting,
        LearnArmorCrafting,
        LearnWeaponCrafting,
        AchieveKnowledge,


    }
}




//public static void SetRequisiteDict()
//{
//    // get all quests and add them with req dicts to the RequisiteDict
//    List<QuestName> allQuests = Enum.GetValues(typeof(QuestName)).Cast<QuestName>().ToList();
//    foreach (QuestName questName in allQuests)
//    {
//        List<QuestName> boolAccomplishmentDict = new List<QuestName>();
//        Dictionary<QuestName, int> intAccolpishmentDict = new Dictionary<QuestName, int>();
//
//        var reqValueTuple = new ValueTuple<List<QuestName>, Dictionary<QuestName, int>>(boolAccomplishmentDict, intAccolpishmentDict);
//
//        RequisiteDict.Add(questName, reqValueTuple);
//    }
//    RequisiteDict.TryGetValue(QuestName.LearnAlchemy, out ValueTuple<List<QuestName>, Dictionary<QuestName, int>> reqs); // important for later
//    //List<QuestName>
//    reqs.Item1.Add(QuestName.PerformEtherPurification);
//
//
//}