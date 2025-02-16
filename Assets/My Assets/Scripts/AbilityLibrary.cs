using System.Collections.Generic;
using UnityEngine;
using AbilityEnums;

public class AbilityLibrary : MonoBehaviour
{
    public Ability_SO FireBall;
    public Ability_SO HealingTouch;
    public Ability_SO DivineStrike;
    public Ability_SO Melee;
    public Ability_SO Push;
    public Ability_SO ColdLight;
    public Ability_SO BrainDamage;
    public Ability_SO LavaPortal;
    public Ability_SO GlobalCooling;

    public Dictionary<Abilities, Ability_SO> abilityDict { private set; get; } = new Dictionary<Abilities, Ability_SO>();
    //During awake, the dictionary will be filled with kvp of ability enums and references to the enums.
    public void Awake()
    {
        abilityDict.Add(Abilities.Fireball, FireBall);
        abilityDict.Add(Abilities.HealingTouch, HealingTouch);
        abilityDict.Add(Abilities.DivineStrike, DivineStrike);
        abilityDict.Add(Abilities.Melee, Melee);
        abilityDict.Add(Abilities.Push, Push);
        abilityDict.Add(Abilities.ColdLight, ColdLight);
        abilityDict.Add(Abilities.BrainDamage, BrainDamage);
        abilityDict.Add(Abilities.LavaPortal, LavaPortal);
        abilityDict.Add(Abilities.GlobalCooling, GlobalCooling);
    }

    // returns a list of abilities based on the creature difficulty. # todo
    public List<Ability_SO> GetAbilities(int creatureDifficulty)
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
    public string GetAbilityInfo(Ability_SO ability)
    {
        string abilityInfo =

        $"Ability Name: {ability.AbilityName}. Resource: {ability.Resource}. Type: {ability.Type}. Cost: {ability.AbilityCost}.Heal Amount: {ability.HealValue}. Damage: {ability.DamageValue}.Effect duration: {ability.TurnDuration}. Number of targets: {ability.Targets}. Summons?: {ability.Summons}. Sypon percentage: {ability.SyphonPercentage}. Ability level: {ability.AbilityLevel}.";

        return abilityInfo;
    }

}
