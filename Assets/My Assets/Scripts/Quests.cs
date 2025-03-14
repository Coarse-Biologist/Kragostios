using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KragostiosAllEnums;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public static class Quests
{
    #region class vars
    public static Dictionary<QuestName, bool> BoolAccomplishments = new Dictionary<QuestName, bool>();
    public static Dictionary<QuestName, int> IntAccomplishments = new Dictionary<QuestName, int>();
    public static List<QuestName> boolQuests = new List<QuestName>();
    public static List<QuestName> repeatableQuests = new List<QuestName>();
    public static List<QuestName> questOrder = new List<QuestName> { QuestName.DefeatEnemies };
    public static Dictionary<QuestName, ValueTuple<List<QuestName>, Dictionary<QuestName, int>>> RequisiteDict;
    public static List<ValueTuple<QuestName, int>> QuestsInOrder = new List<ValueTuple<QuestName, int>>();

    public static Dictionary<QuestName, string> QuestStringDict = new Dictionary<QuestName, string>();

    public static Dictionary<QuestName, LocationType> QuestLocationDict = new Dictionary<QuestName, LocationType>();
    public static Dictionary<QuestName, Biomes> QuestBiomeDict = new Dictionary<QuestName, Biomes>();

    public static List<Quest_SO> AllQuest_SOs = new List<Quest_SO>();

    #endregion

    #region prologue vars
    public static Dictionary<Elements, List<string>> colorWords = new Dictionary<Elements, List<string>>();
    public static int prologueStep { private set; get; } = 0;

    public static int questIndex { private set; get; } = 0;
    [SerializeField] public static List<Quest_SO> Quest_SO_OrderList { private set; get; } = new List<Quest_SO>();
    #endregion

    //track progress method. stores and sets progress in different skillsand knowledges

    //qualification tracker. tracks what quests one is qualified for. if one is qualified for multiple, what happens? 

    // when does it matter that one is qualified for one  more more quests? 

    // when should i check whether a quest is complete?

    public static void SetStartOrderOfQuests()
    {
        QuestsInOrder = new List<ValueTuple<QuestName, int>>()
        {
            (QuestName.DefeatEnemies, 3),
            (QuestName.ExamineBodies, 5),
            (QuestName.AttemptCoreExtraction, 1),
            (QuestName.AcquireWeapon, 1),
            //(QuestName.ExamineCores, 5),
            //(QuestName.PracticalExperiments, 3),
            //(QuestName.LearnGlassMaking, 1),
            //(QuestName.CollectFireCores, 5),
            //(QuestName.MakeFurnace, 5),
            //(QuestName.CollectSand, 1),
            //(QuestName.MakeGlass, 1),
            //(QuestName.UseCores, 5),
            //(QuestName.AttemptPurifications, 5),

        };
    }
    public static void Setup()
    {
        LoadAllQuest_Sos(AllQuest_SOs);
        SetStartOrderOfQuests();
        SetQuestStringDict();
        SetColorWordsDict();
    }

    public static void SetQuestStringDict()
    {
        //string defeatEnemiesString = "You've spent long enough watching these horrible monsters from afar, and have witnessed enough death and suffering while you peered on from a distance. You may be able to understand better if you can get a closer look. To inspect monsters up close... you'll probably have to kill some.";
        //
        //QuestStringDict.Add(QuestName.DefeatEnemies, defeatEnemiesString);
        //
        //string examineBodiesString = "$Button$ Examine the bodies.$ What is wrong with these things?! Once you get over the smell and disgust, you should take a closer look.";

        //QuestStringDict.Add(QuestName.ExamineBodies, examineBodiesString);
    }

    public static Quest_SO GetCurrentQuest()
    {
        if (Quest_SO_OrderList[questIndex] != null) return Quest_SO_OrderList[questIndex];
        else return AllQuest_SOs[1];
        //return null;
    }

    public static Item_SO QuestItemAtTrader()
    {
        Quest_SO currentQuest = GetCurrentQuest();
        return currentQuest.QuestItem;
        //return WorldChest.allItemsList[1];
    }


    public static string GetCurrentQuestString()
    {
        QuestName currentQuest = QuestsInOrder[0].Item1;
        QuestStringDict.TryGetValue(currentQuest, out string questString);
        return questString;
    }
    public static Quest_SO GetQuest_SO(QuestName questname)
    {
        Quest_SO soughtQuest = null;
        foreach (Quest_SO quest in AllQuest_SOs)
        {
            if (quest.QuestEnum == questname) soughtQuest = quest;
        }
        return soughtQuest;

    }


    #region progress on quests
    public static void RemoveQuestFromOrderDict(QuestName quest)
    {
        foreach (ValueTuple<QuestName, int> touplee in QuestsInOrder)
        {
            if (touplee.Item1 == quest)
            {
                QuestsInOrder.Remove(touplee);
            }
        }
    }

    public static void IncrementIntQuests(QuestName quest, int increments = 1)
    {
        if (IntAccomplishments.TryGetValue(quest, out int num))
        {
            IntAccomplishments[quest] += increments;
        }
        else IntAccomplishments.Add(quest, increments);

        EnableEarnedAlchemy();

        Debug.Log($"Quest: {quest} accomplishment value increased by {increments}!");
    }

    private static void EnableEarnedAlchemy()
    {
        if (IntAccomplishments.TryGetValue(QuestName.ExamineBodies, out int value))
        {
            if (AlchemyHandler.CanExtractCores != true && value > 3)
            {
                AlchemyHandler.EnableExtraction();
            }
        }
        else IntAccomplishments.Add(QuestName.ExamineBodies, 0);
        if (BoolAccomplishments.TryGetValue(QuestName.LearnGlassMaking, out bool isTrue))
        {
            if (AlchemyHandler.CanPurifyEther != true && isTrue)
            {
                AlchemyHandler.EnablePurification();
            }
        }
        else BoolAccomplishments.Add(QuestName.LearnGlassMaking, false);

    }

    public static void CompleteBoolQuest(QuestName quest)
    {
        if (BoolAccomplishments.TryGetValue(quest, out bool complete))
        {
            BoolAccomplishments[quest] = true;
            Debug.Log($"Quest: {quest} has been completed!");
        }
        else BoolAccomplishments.Add(quest, true);

        EnableEarnedAlchemy();
    }
    #endregion


    public static void LoadAllQuest_Sos(List<Quest_SO> destination)
    {
        List<QuestName> allQuestEnums = GeneralFunctions.GetAllEnums<QuestName>();

        foreach (QuestName questEnum in allQuestEnums)
        {
            string address = questEnum.ToString();
            Addressables.LoadAssetAsync<Quest_SO>("Assets/My Assets/Addressables/Quests/" + address + ".asset").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {

                    Quest_SO loadedSO = handle.Result;
                    if (loadedSO.Repeatable)
                    {
                        repeatableQuests.Add(loadedSO.QuestEnum);
                        IntAccomplishments.Add(loadedSO.QuestEnum, 0);

                    }
                    else boolQuests.Add(loadedSO.QuestEnum);

                    if (!destination.Contains(loadedSO))
                    {
                        destination.Add(loadedSO);
                        Debug.Log($"Loaded: {address}");
                    }

                    Quest_SO_OrderList.Add(loadedSO);
                    Debug.Log($"Quest order list = {Quest_SO_OrderList.Count}");

                }

                else

                {
                    Debug.LogError($"Failed to load ScriptableObject at address: {address}");
                }
            };

        }
    }
    #region enums

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
        AcquireWeapon,
        ExamineCores,
        //PracticalExperiments, // this will be a step in which the player is asked to use cores and attempt purification
        LearnGlassMaking,
        //CollectFireCores,
        //MakeFurnace,
        //CollectSand,
        //MakeGlass,
        //UseCores,
        //AttemptPurifications,
        //UsePotions,
        //UseAbilities,
        //UseHealAbilities,
        //PerformCoreExtraction,
        //PerformEtherPurification,
        //LearnWelding,
        //LearnSewing,
        //LearnAlchemy,
        //LearnChemistry,
        //LearnPotionCrafting,
        //LearnArrowCrafting,
        //LearnArmorCrafting,
        //LearnWeaponCrafting,
        //AchieveKnowledge,
    }
    #endregion

    #region prologue functions
    public static Dictionary<Elements, List<string>> SetColorWordsDict()
    {
        List<Elements> allElements = GeneralFunctions.GetAllEnums<Elements>();
        //foreach (Elements element in allElements)
        //{
        //    colorWords.Add(element, new List<string>());
        //}
        colorWords.Add(Elements.Cold, new List<string> { "icy blue", "glacial turquoise", "shredded and replaced by an icy pit" });
        colorWords.Add(Elements.Water, new List<string> { "deep, dark blue", "ocean blue", "eroded and erased, becoming a drenched, wave-pummeled pit" });
        colorWords.Add(Elements.Acid, new List<string> { "semi-transparent green", "toxic greeeeeen", "melted into a green, sludge pit" });
        colorWords.Add(Elements.Heat, new List<string> { "warm red", "glowing reeeeeeeeed", "has been broiled into charcoaled, ash pit" });
        colorWords.Add(Elements.Fire, new List<string> { "firey red and orange", "red inferno", "has been incinerated and left a scorched pit" });
        colorWords.Add(Elements.Electricity, new List<string> { "shocking, yellow-white", "yellow flash", "struck violently, cahnged into a vibrating, electrified pit" });
        colorWords.Add(Elements.Bacteria, new List<string> { "scattered, living green", "putrid, dark green", "covered in a slimy, horrifying mucous" });
        colorWords.Add(Elements.Air, new List<string> { "transparent, flowing swirl", "pressurized gas", "whiped away, leaving an empty, windblown pit" });
        colorWords.Add(Elements.Virus, new List<string> { "viscous fluid of light blue and green", "vile, pool of sickening cyan fluid", "ice pit" });
        colorWords.Add(Elements.Earth, new List<string> { "rich, soil-brown", "earthy chocolate", "eviscerated into a meteoric pit" });
        colorWords.Add(Elements.Poison, new List<string> { "cloud of venomous green", "toxic greeeeeen", "tainted, blasted pit" });
        colorWords.Add(Elements.Fungi, new List<string> { "mass of yellow-green tendrils", "cloud of sporey particles", "covered in a thick dust of menacingly orange dust" });
        colorWords.Add(Elements.Plant, new List<string> { "tangle of forest-green veins", "mass of vines and leaves", "transformed into a treacherous patch of jungle" });
        colorWords.Add(Elements.Radiation, new List<string> { "radiant, warm, yellow-orange", "suncore", "scorched and mutated into a dry, foreign surface" });
        colorWords.Add(Elements.Light, new List<string> { "radiant, pleasant, yellow-white", "beam of heaven", "warmly alighted and transformed as though by years in the most powerful sunshine" });
        colorWords.Add(Elements.Psychic, new List<string> { "galaxy of warping purples", "spiraling, orchestra of hypnotizing colors and thoughts", "replaced by?... the impossible?  a mirage? an illusion? but to you, somehow completely comprehensible" });


        return colorWords;
    }

    public static string ParsePrologueString(string prologueItem, Elements elementalColor)
    {
        Debug.Log($"{prologueItem}");
        if (colorWords.TryGetValue(elementalColor, out List<string> items))
        {
            //foreach (KeyValuePair<Elements, List<string>> kvp in colorWords)
            //{
            //    foreach (string stroge in kvp.Value)
            //    {
            //        Debug.Log($"{stroge}");
            //    }
            //}
            if (items.Count >= 3)
            {
                string item1 = items[0];
                string item2 = items[1];
                string item3 = items[2];

                prologueItem = prologueItem.Replace("$COLOR$", item1);
                prologueItem = prologueItem.Replace("$ELONGATEDCOLOR$", item2);
                prologueItem = prologueItem.Replace("$CRATERDESCRIPTION$", item3);

            }

            // increments the number of times ive parsed every time i use it. hopefully this is right
            Debug.Log($"step = {prologueStep}");

        }

        return prologueItem;
    }
    public static void IncrementPrologueStep()
    {
        prologueStep++;

    }

    public static List<string> GetPrologue()
    {
        return new List<string> { "Pain... Screaming pain... You look about yourself. Where are you? Your arm throbs with insatiable pain and your vision is blurred with a confusion and nausea as from a nightmare. Were you sleeping? Just a little more... Impossible, you groan hoarsely as you exert yourself to sit up. Noticing nothing familiar in your environment you take to examining your body. Whence comes this evil pain? Your vision focuses and you behold the state of your hands, feet, ankles... Your feet and ankles are bruised wretchedly. It appears as though you had run and walked a great distance barefoot. Your ankles have hard, regularly shaped bruising all around their circumference as though you had been restrained. The same is true of your wrists. Your fingers and nails show signs of having clawed at something unfavorable beyong them on the Mohs Hardness scale. A sudden intense stab originating from your shoulder draws your attention. it is not at all how you remembered, and the alteration is hard to describe. Perhaps you can only give ONE WORD or impression even to describe that which you percieve. What is the one word?", "Rising more and more you can see that a strangely beautiful, $COLOR$ is inflating from deep within your right shoulder and the lateral cavity of your chest. What in the devils has happened? Are you poisoned?", "Again a terrible pain thrashes at your body and skull like a caged animal from within. You stretch and extend your arms and back to somehow alleviate the pain and feel in elated frenzy that the tortuous sensation in your chest, shoulder and arm are being unspeakably, marvelously transformed into a glowing $ELONGATEDCOLOR$! You are again thrust forcefully onto your back - but the pain is entirely gone. Lifting your head you percieve the effects of what you only beheld in a flash. The ground at your feet has been $CRATERDESCRIPTION$. It seems all of the tortured energy of your body has found a new victim.", "You rise again to get your bearings, relieved but pregnant with questions, uncertainty, curiosity." };
    }
    #endregion
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

//public static bool CheckQualification(QuestName questName)
//{
//    bool qualified = false;
//    if (boolQuests.Contains(questName))
//    {
//        if (BoolAccomplishments[questName] == true)
//        {
//            qualified = true;
//        }
//        else return false;
//    }
//    else if (repeatableQuests.Contains(questName))
//    {
//        if (BoolAccomplishments[questName] == true)
//        {
//            qualified = true;
//        }
//        else return false;
//    }
//}
//
//    return qualified;
// Tuple<string, LocationType> 


