using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using KragostiosAllEnums;
using AbilityEnums;
[System.Serializable]
public class PlayerSaveData
{
    public List<Ability_SO> knownAbilities_SD;
    public Dictionary<Item_SO, int> inventory_SD;
    public string characterName_SD = "Sqreegler";

    public string description_SD;
    public Combatants charType_SD;
    public Difficulty difficulty_SD;

    #region //  resources

    public int MaxHealth_SD = 100;
    public int MaxMana_SD = 100;
    public int MaxStamina_SD = 100;
    public int initiative_SD = 100;

    public int ActionPoints_SD = 1;
    public int currentActionPoints_SD = 1;
    public int ActionPointRegen_SD = 1;

    public int currentHealth_SD = 100;
    public int currentMana_SD = 100;
    public int currentStamina_SD = 100;
    public int currentOverHealth_SD = 0;
    #endregion

    #region // Elemental Affinityences
    public int ColdAffinity_SD = 0;
    public int WaterAffinity_SD = 0;
    public int EarthAffinity_SD = 0;
    public int HeatAffinity_SD = 0;
    public int FireAffinity_SD = 0;
    public int AirAffinity_SD = 0;
    public int ElectricityAffinity_SD = 0;
    public int LightAffinity_SD = 0;
    public int PsychicAffinity_SD = 0;
    public int FungiAffinity_SD = 0;
    public int PlantAffinity_SD = 0;
    public int PoisonAffinity_SD = 0;
    public int AcidAffinity_SD = 0;
    public int RadiationAffinity_SD = 0;
    public int BacteriaAffinity_SD = 0;
    public int VirusAffinity_SD = 0;

    #endregion


    #region // Physical Affinityences 
    public int BludgeoningResist_SD = 0;
    public int SlashingResist_SD = 0;
    public int PiercingResist_SD = 0;
    public Elements Element_SD = Elements.None;
    #endregion

    #region // resource regen
    public int HealthRegen_SD = 1;
    public int ManaRegen_SD = 1;
    public int StaminaRegen_SD = 1;

    #endregion

    #region // "Level Stats"

    public int characterLevel_SD = 1;
    public int availableStatPoints_SD = 40;
    public int currentXp_SD = 0;
    public int MaxXp_SD = 100;

    #endregion

    #region // inventory
    public List<Rewards> rewards_SD = new List<Rewards> { Rewards.Gold, Rewards.Xp };
    public int characterGold_SD = 0;

    #endregion

    public PlayerSaveData(StatsHandler stats)
    {
        characterName_SD = stats.characterName;
        description_SD = stats.description;

        MaxHealth_SD = stats.MaxHealth;
        MaxMana_SD = stats.MaxMana;
        MaxStamina_SD = stats.MaxStamina;

        currentHealth_SD = stats.currentHealth;
        currentOverHealth_SD = stats.currentOverHealth;
        currentMana_SD = stats.currentMana;
        currentStamina_SD = stats.currentStamina;

        HealthRegen_SD = stats.HealthRegen;
        ManaRegen_SD = stats.ManaRegen;
        StaminaRegen_SD = stats.StaminaRegen;

        ActionPoints_SD = stats.ActionPoints;
        currentActionPoints_SD = stats.currentActionPoints;
        ActionPointRegen_SD = stats.ActionPointRegen;

        initiative_SD = stats.initiative;


        knownAbilities_SD = stats.knownAbilities;
        inventory_SD = stats.Inventory;
        characterGold_SD = stats.characterGold;

        ColdAffinity_SD = stats.ColdAffinity;
        WaterAffinity_SD = stats.WaterAffinity;
        EarthAffinity_SD = stats.EarthAffinity;
        HeatAffinity_SD = stats.HeatAffinity;
        FireAffinity_SD = stats.FireAffinity;
        AirAffinity_SD = stats.AirAffinity;
        ElectricityAffinity_SD = stats.LightAffinity;
        LightAffinity_SD = stats.LightAffinity;
        PsychicAffinity_SD = stats.PsychicAffinity;
        FungiAffinity_SD = stats.FungiAffinity;
        PlantAffinity_SD = stats.PlantAffinity;
        PoisonAffinity_SD = stats.PlantAffinity;
        AcidAffinity_SD = stats.AcidAffinity;
        RadiationAffinity_SD = stats.RadiationAffinity;
        BacteriaAffinity_SD = stats.BacteriaAffinity;
        VirusAffinity_SD = stats.VirusAffinity;

        BludgeoningResist_SD = stats.BludgeoningResist;
        SlashingResist_SD = stats.SlashingResist;
        PiercingResist_SD = stats.PiercingResist;

        characterLevel_SD = stats.characterLevel;
        availableStatPoints_SD = stats.availableStatPoints;
        currentXp_SD = stats.currentXp;
        MaxXp_SD = stats.MaxXp;
    }

}
