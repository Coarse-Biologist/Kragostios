using UnityEngine;
using System.Collections.Generic;
using KragostiosAllEnums;
using AbilityEnums;
using System;
using System.Linq;



public static class GeneralFunctions
{
    public static Item_SO GetItemFromItemName(string itemName)
    {
        Item_SO item_SO = null;
        foreach (KeyValuePair<string, Item_SO> kvp in WorldChest.allItems)
        {
            if (kvp.Key == itemName)
            {
                item_SO = kvp.Value;
            }
        }
        return item_SO;
    }

    public static Ability_SO GetAbilityFromEnum(Abilities abilityEnum)
    {
        Ability_SO ability_SO = null;
        foreach (KeyValuePair<Abilities, Ability_SO> kvp in AbilityLibrary.abilityDict)
        {
            if (kvp.Key == abilityEnum)
            {
                ability_SO = kvp.Value;
            }
        }
        return ability_SO;
    }
    public static Abilities GetEnumFromAbility(Ability_SO ability)
    {
        Abilities abilityEnum = Abilities.None;
        foreach (KeyValuePair<Ability_SO, Abilities> kvp in AbilityLibrary.reverseAbilityDict)
        {
            if (kvp.Key == ability)
            {
                abilityEnum = kvp.Value;
            }
        }
        return abilityEnum;
    }

    public static List<T> GetAllEnums<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    public static string AddSpaceToEnum(Enum enumString)
    {
        string spacedEnum = enumString.ToString();

        foreach (char letter in spacedEnum)
        {
            if (char.IsUpper(letter))//Contains(letter.ToString()))
            {
                int index = spacedEnum.IndexOf(letter);
                spacedEnum = spacedEnum.Insert(index, " ");
            }
        }
        return spacedEnum;
    }

    public static Tuple<int, int> GetStatIncrementValues(StatType stat)
    {
        var resourceList = new List<string>() { "Health", "Power", "Stamina" };
        string statString = stat.ToString();
        if (statString.Contains("Affinity") || statString.Contains("Resistance") || resourceList.Contains(statString)) return new Tuple<int, int>(1, 5);
        if (statString.Contains("Action")) return new Tuple<int, int>(20, 1);
        else return new Tuple<int, int>(3, 1);

    }


}
