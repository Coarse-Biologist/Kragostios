using System.Collections.Generic;
using UnityEngine;
using AbilityEnums;

public class AbilityLibrary : MonoBehaviour
{
    public Ability_SO FireBall;

    public Ability_SO HealingTouch;

    public Ability_SO DivineStrike;

    public Ability_SO Melee;

    public Dictionary<Abilities, Ability_SO> abilityDict {private set; get;} = new Dictionary<Abilities, Ability_SO>();

    public void Awake()
    {
        abilityDict.Add(Abilities.Fireball, FireBall);
        abilityDict.Add(Abilities.HealingTouch, HealingTouch);
        abilityDict.Add(Abilities.DivineStrike, DivineStrike);
        abilityDict.Add(Abilities.Melee, Melee);
    }

    public List<Ability_SO> GetAbilities (int creatureDifficulty)
    {
        List<Ability_SO> abilities = new List<Ability_SO>();
        foreach(KeyValuePair<Abilities, Ability_SO>  kvp in abilityDict)
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
        if(abilities.Count == 0)
        {
            abilities.Add(Melee);
        }
        return abilities;
    }

    public string GetAbilityInfo(Ability_SO ability)
    {
        string abilityInfo = 
        
        $"Ability Name: {ability.AbilityName}. Resource: {ability.Resource}. Resource: {ability.Resource}. Type: {ability.Type}. Cost: {ability.AbilityCost}.Heal Amount: {ability.HealValue}. Damage: {ability.DamageValue}.Effect duration: {ability.TurnDuration}. Number of targets: {ability.Targets}. Summons?: {ability.Summons}. Sypon percentage: {ability.SyphonPercentage}. Ability level: {ability.AbilityLevel}.";


        return abilityInfo;
    }

}
