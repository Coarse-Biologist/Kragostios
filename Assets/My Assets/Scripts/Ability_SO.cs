using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Abilities", menuName = "Crew's Abilities/ Ability")]
public class Ability_SO : ScriptableObject
{
    public string AbilityName;
    public string Description = "None";
    public ResourceTypes Resource;
    public AbilityCategories Type;
    public Elements ElementType;
    public PhysicalDamage PhysicalType;
    public int AbilityCost;
    public int HealValue;
    public int DamageValue;
    public int TurnDuration;
    public int Targets;
    public bool Summons;
    public float SyphonPercentage;
    public int AbilityLevel;

    public List<Buffs> BuffEffects;
    public List<Debuffs> DebuffEffects;

    public string GetAbilityInfo()
    {
        string abilityInfo = " ";
        return abilityInfo;
    }
}
