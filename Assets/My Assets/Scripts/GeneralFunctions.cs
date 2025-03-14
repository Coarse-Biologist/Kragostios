using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
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


}
