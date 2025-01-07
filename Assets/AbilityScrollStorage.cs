using KragostiosAllEnums;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScrollStorage : MonoBehaviour
{
    public Abilities Fireball { get; private set; }
    public Abilities Melee { get; private set; }
    public Abilities HealingTouch { get; private set; }

    private void Awake()
    {
        Fireball = new Abilities("Fireball", AbilityCategories.Attack, damageValue: 5, resource: ResourceTypes.Mana, abilityCost: 1);
        Melee = new Abilities("Melee", AbilityCategories.Attack, damageValue: 1, resource: ResourceTypes.Stamina, abilityCost: 0);
        HealingTouch = new Abilities("Healing Touch", AbilityCategories.Heal, healValue: 5, resource: ResourceTypes.Mana, abilityCost: 1);
    }

    public List<Abilities> GetWeakAbilities()
    {
        return new List<Abilities> { Fireball, Melee, HealingTouch };
    }

/// <summary>
/// ////
/// </summary>

    public class Abilities
{
    public string AbilityName { get; private set; }
    public AbilityCategories Type { get; private set; }
    public int HealValue { get; private set; }
    public int DamageValue { get; private set; }
    public int TurnDuration { get; private set; }
    public int Targets { get; private set; }
    public bool Summons { get; private set; }
    public float SyphonPercentage { get; private set; }
    public int AbilityLevel { get; private set; }
    public ResourceTypes Resource { get; private set; }
    public int AbilityCost { get; private set; }

    public Abilities(string abilityName, AbilityCategories type, int damageValue = 0, int healValue = 0,
        int targets = 1, ResourceTypes resource = ResourceTypes.Mana, int abilityCost = 0)
    {
        AbilityName = abilityName;
        Type = type;
        DamageValue = damageValue;
        HealValue = healValue;
        Targets = targets;
        Resource = resource;
        AbilityCost = abilityCost;
    }

    }

}
