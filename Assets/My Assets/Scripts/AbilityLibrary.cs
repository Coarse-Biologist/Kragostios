using System.Collections.Generic;
using UnityEngine;
using AbilityEnums;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using KragostiosAllEnums;


public static class AbilityLibrary
{
    public static Ability_SO FireBall;
    public static Ability_SO HealingTouch;
    public static Ability_SO DivineStrike;
    public static Ability_SO Melee;
    public static Ability_SO Push;
    public static Ability_SO ColdLight;
    public static Ability_SO BrainDamage;
    public static Ability_SO LavaPortal;
    public static Ability_SO GlobalCooling;
    public static Dictionary<Abilities, Ability_SO> abilityDict { private set; get; } = new Dictionary<Abilities, Ability_SO>();
    public static Dictionary<Ability_SO, Abilities> reverseAbilityDict { private set; get; } = new Dictionary<Ability_SO, Abilities>();

    public static List<Ability_SO> allAbilities = new List<Ability_SO>();
    public static List<Abilities> abilityEnumsList = Enum.GetValues(typeof(Abilities)).Cast<Abilities>().ToList();



    // List of addresses to load (manually assigned or from an external source)
    public static List<string> allAddresses = new List<string> { "Melee", "FireBall", "BrainDamage", "DivineSmite", "HealingTouch", "LavaPortal", "Push", "GlobalCooling" };


    // returns a list of abilities based on the creature difficulty. # todo
    public static List<Abilities> GetAbilities(int creatureDifficulty, Elements element)
    {
        var abilities = new List<Abilities>();

        foreach (Ability_SO ability in allAbilities)
        {
            if (ability.ElementType == element && ability.AbilityLevel <= creatureDifficulty)
            {
                Debug.Log($"Creature was of type {element} and abiloty level {creatureDifficulty} and therefore has {ability.AbilityName}");
                abilities.Add(ability.AbilityEnum);
            }
        }

        abilities.Add(Abilities.Melee);
        return abilities;
    }



    public static void SetAbilityDict()
    {
        reverseAbilityDict = abilityDict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        foreach (Ability_SO ability in allAbilities)
        {
            abilityDict.TryAdd(ability.AbilityEnum, ability);
            reverseAbilityDict.TryAdd(ability, ability.AbilityEnum);
            Debug.Log($"Adding {ability.AbilityName} to dicts");
        }
    }
    // returns a string describing the ability
    public static string GetAbilityInfo(Ability_SO ability)
    {
        string abilityInfo =

        $"Ability Name: {ability.AbilityName}. Resource: {ability.Resource}. Type: {ability.Type}. Cost: {ability.AbilityCost}.Heal Amount: {ability.HealValue}. Damage: {ability.DamageValue}.Effect duration: {ability.TurnDuration}. Number of targets: {ability.Targets}. Summons?: {ability.Summons}. Sypon percentage: {ability.SyphonPercentage}. Ability level: {ability.AbilityLevel}.";

        return abilityInfo;
    }
    public static string GetAbilityName(Ability_SO ability)
    {
        return ability.AbilityName;
    }
    public static Ability_SO GetAbilityFromName(string name)
    {
        Ability_SO soughtAbility = allAbilities[0];
        foreach (Ability_SO ability in allAbilities)
        {
            if (ability.AbilityName == name)
                soughtAbility = ability;
        }
        return soughtAbility;
    }
    private static void AddToAbilityDicts(Abilities abilityEnum, Ability_SO loadedSO)
    {
        //reverseAbilityDict = abilityDict.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        if (abilityEnum != Abilities.None)
        {
            allAbilities.Add(loadedSO);
            abilityDict.Add(abilityEnum, loadedSO);
            Debug.Log($"enum {abilityEnum} added with the value {loadedSO.AbilityName}");
            reverseAbilityDict.Add(loadedSO, abilityEnum);
        }
        else Debug.Log("oops those had no Ability Enum and couldnt be added to the dicts");
    }

    public static void LoadAbilities(List<string> addressType, List<Ability_SO> destination)
    {
        foreach (string address in addressType)
        {

            Addressables.LoadAssetAsync<Ability_SO>("Assets/My Assets/Addressables/Abilities/" + address + ".asset").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Ability_SO loadedSO = handle.Result;
                    if (!destination.Contains(loadedSO))
                    {
                        destination.Add(loadedSO);
                        AddToAbilityDicts(loadedSO.AbilityEnum, loadedSO);
                        Debug.Log($"Loaded: {address}");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load ScriptableObject at address: {address}");
                }
            };
            //SetAbilityDict();

        }

    }

}


//{
//    List<Ability_SO> abilities = new List<Ability_SO>();
//    foreach (KeyValuePair<Abilities, Ability_SO> kvp in abilityDict)
//    {
//        Ability_SO ability = kvp.Value;
//        if (ability.AbilityLevel <= creatureDifficulty)
//        {
//            if (UnityEngine.Random.Range(0, 1) > .5)
//            {
//                abilities.Add(kvp.Value);
//                Debug.Log($"Adding ability {kvp.Value}");
//            }
//        }
//    }
//
//    //adds melee by default if empty
//    if (abilities.Count == 0)
//    {
//        Debug.Log($"combatant had 0 abilities and will therfore be given a complimentary main hand attack.");
//        abilities.Add(reverseAbilityDict[0]);
//    }
//
//    return abilities;
//}

