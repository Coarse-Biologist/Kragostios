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
    [SerializeField] private ResourceTypes resource; // this is the resource which will be subtracted when the ability is used
    public ResourceTypes Resource => resource;
    [SerializeField] private AbilityCategories type; // this type will be used when combat flow is deciding whether to apply damage/ heal/buff/debuff
    public AbilityCategories Type => type;
    [SerializeField] private Elements elementType; // this is used to determine whether a targets resistences are relevant to the affects of the ability
    public Elements ElementType => elementType;
    [SerializeField] private PhysicalDamage physicalType; // this is used to determine whether a targets resistences are relevant to the affects of the ability

    public PhysicalDamage PhysicalType => physicalType;
    [SerializeField] private int abilityCost;
    public int AbilityCost => abilityCost; // this is used to caluclate whether the caster can use an ability or not an calculate a casters resulting mana/health/stamina after using an ability
    [SerializeField] private int healValue; // used to calculate a targets health after an abiloty is used on a target
    public int HealValue => healValue;
    [SerializeField] private int damageValue;// used to calculate a targets health after an ability is used on a target
    public int DamageValue => damageValue;
    [SerializeField] private int turnDuration; // used to determine the number of turns during which an ability lingers and the buffs/debuffs/ persist or are reapplied
    public int TurnDuration => turnDuration;
    [SerializeField] private int targets; // used to determine the number of targets to which the buffs/ debuffs? damage are applied
    public int Targets => targets;
    [SerializeField] private bool summons; // bool which states whether an ability summons a creature
    public bool Summons => summons;
    [SerializeField] private bool damageOverTime = false; // if true damage value is reappplied every turn for the turnv duration
    public bool DamageOverTime => damageOverTime;
    [SerializeField] private float syphonPercentage; // value is used to determine what percentage of the damage value is to be applied as heal to the caster
    public float SyphonPercentage => syphonPercentage;
    [SerializeField] private int abilityLevel; // value may be used to determine the level one must be to cast this ability
    public int AbilityLevel => abilityLevel;
    [SerializeField] private List<Buffs> buffEffects; // list of the buff effects which are applied by ability for each turn of length turn duration
    public List<Buffs> BuffEffects => buffEffects;
    [SerializeField] private List<Debuffs> debuffEffects; // list of the buff effects which are applied by ability for each turn of length turn duration
    public List<Debuffs> DebuffEffects => debuffEffects;

    public string GetBuffListString() // returns a string which is the list of all buffs applied by an ability
    {
        string buffListString = "";
        foreach (Buffs buff in BuffEffects)
        {
            buffListString += $"{buff}, ";
        }
        return buffListString;
    }
    public string GetDebuffListString()  // returns a string which is the list of all buffs applied by an ability

    {
        string debuffListString = "";
        foreach (Debuffs debuff in DebuffEffects)
        {
            debuffListString += $"{debuff}, ";
        }
        return debuffListString;
    }


}
