using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using UnityEngine.Animations;

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
    public bool DamageOverTime = false;
    public float SyphonPercentage;
    public int AbilityLevel;

    public List<Buffs> BuffEffects;
    public List<Debuffs> DebuffEffects;

    public string GetBuffListString()
    {
        string buffListString = "";
        foreach (Buffs buff in BuffEffects)
        {
            buffListString += $"{buff}, ";
        }
        return buffListString;
    }
    public string GetDebuffListString()
    {
        string debuffListString = "";
        foreach (Debuffs debuff in DebuffEffects)
        {
            debuffListString += $"{debuff}, ";
        }
        return debuffListString;
    }


}
