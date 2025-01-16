using UnityEngine;
using KragostiosAllEnums;

[CreateAssetMenu(fileName = "Abilities", menuName = "Crew's Abilities/ Ability")]
public class Ability_SO : ScriptableObject
{
    public string AbilityName;
    public string Description = "None";
    public ResourceTypes Resource;
    public AbilityCategories Type;
    public int AbilityCost;
    public int HealValue;
    public int DamageValue;
    public int TurnDuration;
    public int Targets;
    public bool Summons;
    public float SyphonPercentage;
    public int AbilityLevel;

    public string GetAbilityInfo()
    {
        string abilityInfo = " ";
        return abilityInfo;
    }
}
