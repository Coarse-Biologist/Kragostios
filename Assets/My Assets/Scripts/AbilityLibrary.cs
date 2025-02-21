using System.Collections.Generic;
using UnityEngine;
using AbilityEnums;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


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


    // List of addresses to load (manually assigned or from an external source)
    public static List<string> allAddresses = new List<string> { "Melee", "FireBall", "BrainDamage", "DivineSmite", "HealingTouch", "LavaPortal", "Push", "GlobalCooling" };


    // returns a list of abilities based on the creature difficulty. # todo
    public static List<Ability_SO> GetAbilities(int creatureDifficulty)
    {
        List<Ability_SO> abilities = new List<Ability_SO>();
        foreach (KeyValuePair<Abilities, Ability_SO> kvp in abilityDict)
        {
            Ability_SO ability = kvp.Value;
            if (ability.AbilityLevel <= creatureDifficulty)
            {
                if (Random.Range(0, 1) > .5)
                {
                    abilities.Add(kvp.Value);
                }
            }
        }

        //adds melee by default if empty
        if (abilities.Count == 0)
        {
            abilities.Add(Melee);
        }
        return abilities;
    }
    // returns a string describing the ability
    public static string GetAbilityInfo(Ability_SO ability)
    {
        string abilityInfo =

        $"Ability Name: {ability.AbilityName}. Resource: {ability.Resource}. Type: {ability.Type}. Cost: {ability.AbilityCost}.Heal Amount: {ability.HealValue}. Damage: {ability.DamageValue}.Effect duration: {ability.TurnDuration}. Number of targets: {ability.Targets}. Summons?: {ability.Summons}. Sypon percentage: {ability.SyphonPercentage}. Ability level: {ability.AbilityLevel}.";

        return abilityInfo;
    }
    private static void AddToAbilityDicts(Abilities abilityEnum, Ability_SO loadedSO)
    {
        if (abilityEnum != Abilities.None)
        {
            abilityDict.Add(abilityEnum, loadedSO);
            reverseAbilityDict.Add(loadedSO, abilityEnum);
        }
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

        }

    }

}
