using System.Collections.Generic;
using KragostiosAllEnums;

using System;
using System.Diagnostics;
using TMPro;

public static class ModdedAbilities
{
    #region class variables
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, int>> AbilityIntMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, int>>();
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, string>> AbilityStringMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, string>>();
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>> AbilityResourceMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>>();
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>> AbilityElementMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>>();
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>> AbilityBuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>>();
    public static Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>> AbilityDebuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>>();

    #endregion

    #region save variables
    public static Dictionary<string, Dictionary<AbilityVars, int>> AbilityIntMods_save = new Dictionary<string, Dictionary<AbilityVars, int>>();
    public static Dictionary<string, Dictionary<AbilityVars, string>> AbilityStringMods_save = new Dictionary<string, Dictionary<AbilityVars, string>>();
    public static Dictionary<string, Dictionary<AbilityVars, ResourceTypes>> AbilityResourceMods_save = new Dictionary<string, Dictionary<AbilityVars, ResourceTypes>>();
    public static Dictionary<string, Dictionary<AbilityVars, Elements>> AbilityElementMods_save = new Dictionary<string, Dictionary<AbilityVars, Elements>>();
    public static Dictionary<string, Dictionary<AbilityVars, List<Buffs>>> AbilityBuffMods_save = new Dictionary<string, Dictionary<AbilityVars, List<Buffs>>>();
    public static Dictionary<string, Dictionary<AbilityVars, List<Debuffs>>> AbilityDebuffMods_save = new Dictionary<string, Dictionary<AbilityVars, List<Debuffs>>>();

    #endregion

    #region modify variables

    public static void DisplayModdedDictstring<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        foreach (KeyValuePair<TKey, TValue> kvp in dict)
        {
            UnityEngine.Debug.Log($"{kvp.Key}");
        }
    }
    public static Dictionary<string, TValue> ChangeDictKeyToString<Abilities_SO, TValue>(Dictionary<Ability_SO, TValue> dict, Func<Ability_SO, string> keyConverter)
    {
        var newDict = new Dictionary<string, TValue>();

        foreach (var kvp in dict)
        {
            newDict[keyConverter(kvp.Key)] = kvp.Value;
        }
        return newDict;
    }
    public static void ModifyIntAttribute(Ability_SO ability, AbilityVars var, int amount) // can be used for AbilityCost, HealValue, DamageValue, TurnDuration, Targets
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
    public static void ModifyStringAttribute(Ability_SO ability, AbilityVars var, string newString) // can be used for name and description
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
    public static void ModifyResourceAttribute(Ability_SO ability, AbilityVars var, ResourceTypes resource) // can be used for name and description
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
    public static void ModifyElementAttribute(Ability_SO ability, AbilityVars var, Elements element) // can be used for name and description
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
    public static void ModifyBuffsAttribute(Ability_SO ability, AbilityVars var, Buffs buff) // can be used for name and description
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
    public static void ModifyDebuffsAttribute(Ability_SO ability, AbilityVars var, Debuffs debuff) // can be used for name and description
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

    public static int GetModdedInt(Ability_SO ability, AbilityVars var)
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
    public static string GetModdedString(Ability_SO ability, AbilityVars var)
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
    public static Elements GetModdedElement(Ability_SO ability, AbilityVars var)
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
    public static ResourceTypes GetModdedResource(Ability_SO ability, AbilityVars var)
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

    public static List<Buffs> GetModdedBuffs(Ability_SO ability, AbilityVars var)
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

    public static List<Debuffs> GetModdedDebuffs(Ability_SO ability, AbilityVars var)
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

    public static void LoadData()
    {
        ModdedAbilitySaveData moddedData = SaveSystem.LoadModdedAbilityData();
        AbilityIntMods_save = ChangeDictKeyToString<string, Dictionary<AbilityVars, int>>(AbilityIntMods, AbilityLibrary.GetAbilityName);
        //AbilityStringMods_save = ChangeDictKeyToString(moddedData.AbilityStringMods_SD, AbilityLibrary.GetAbilityName);
        //AbilityResourceMods = moddedData.AbilityResourceMods_SD;
        //AbilityElementMods = moddedData.AbilityElementMods_SD;
        //AbilityBuffMods = moddedData.AbilityBuffMods_SD;
        //AbilityDebuffMods = moddedData.AbilityDebuffMods_SD;
    }
}

// all dicts have to be unpacked and repacked with data types replacing the scriptable objects.

//static void DisplayModdedDictString<TKey, TValue>(Dictionary<TKey, TValue> dict)
//    {
//        foreach (KeyValuePair<TKey, TValue> kvp in dict)
//        {
//            Console.WriteLine($"{kvp.Key}");
//      
//    }
//    static Dictionary<string, TValue> ChangeDictKeyToString<TOldKey, TValue>(Dictionary<TOldKey, TValue> dict, Func<TOldKey, string> keyConverter)
//    {
//        var newDict = new Dictionary<string, TValue>();
//
//        foreach (var kvp in dict)
//        {
//            newDict[keyConverter(kvp.Key)] = kvp.Value;
//        }
//        return newDict;
//    }
//Dictionary<int, Dictionary<int, string>> dict1 = new Dictionary<int, Dictionary<int, string>>();
//dict1.Add(2, new Dictionary<int, string>());
//
//DisplayModdedDictString(dict1);
//
//ChangeDictKeyToString(dict1, key => $"Key_{key}");
//
//DisplayModdedDictString(ChangeDictKeyToString(dict1, key => $"Key_{key}"));




