using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using UnityEngine.Animations;

[CreateAssetMenu(fileName = "Abilities", menuName = "Crew's Abilities/ Ability")]
public class Ability_SO : ScriptableObject
{
    [SerializeField] private string abilityName;
    public string AbilityName => abilityName;
    [SerializeField] private string description = "None";
    public string Description => description;
    [SerializeField] private ResourceTypes resource;
    public ResourceTypes Resource => resource;
    [SerializeField] private AbilityCategories type;
    public AbilityCategories Type => type;
    [SerializeField] private Elements elementType;
    public Elements ElementType => elementType;
    [SerializeField] private PhysicalDamage physicalType;
    public PhysicalDamage PhysicalType => physicalType;
    [SerializeField] private int abilityCost;
    public int AbilityCost => abilityCost;
    [SerializeField] private int healValue;
    public int HealValue => healValue;
    [SerializeField] private int damageValue;
    public int DamageValue => damageValue;
    [SerializeField] private int turnDuration;
    public int TurnDuration => turnDuration;
    [SerializeField] private int targets;
    public int Targets => targets;
    [SerializeField] private bool summons;
    public bool Summons => summons;
    [SerializeField] private bool damageOverTime = false;
    public bool DamageOverTime => damageOverTime;
    [SerializeField] private float syphonPercentage;
    public float SyphonPercentage => syphonPercentage;
    [SerializeField] private int abilityLevel;
    public int AbilityLevel => abilityLevel;
    [SerializeField] private List<Buffs> buffEffects;
    public List<Buffs> BuffEffects => buffEffects;
    [SerializeField] private List<Debuffs> debuffEffects;
    public List<Debuffs> DebuffEffects => debuffEffects;

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
