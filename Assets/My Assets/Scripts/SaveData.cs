using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using KragostiosAllEnums;
using AbilityEnums;
using System;
using JetBrains.Annotations;

[System.Serializable]
public class PlayerSaveData
{
    #region // player stats
    public List<Ability_SO> knownAbilities_SD;
    public Dictionary<Item_SO, int> inventory_SD;
    public string characterName_SD = "Sqreegler";

    public string description_SD;
    public Combatants charType_SD;
    public Difficulty difficulty_SD;

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


    public int BludgeoningResist_SD = 0;
    public int SlashingResist_SD = 0;
    public int PiercingResist_SD = 0;
    public Elements Element_SD = Elements.None;
    public int HealthRegen_SD = 1;
    public int ManaRegen_SD = 1;
    public int StaminaRegen_SD = 1;

    public int characterLevel_SD = 1;
    public int availableStatPoints_SD = 40;
    public int currentXp_SD = 0;
    public int MaxXp_SD = 100;


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

[System.Serializable]
public class MapData
{
    public Dictionary<float[], LocationType> locationTypeDict_SD;
    public Dictionary<float[], Biomes> biomeDict_SD;
    public Dictionary<float[], Kingdoms> kingdomDict_SD;
    public MapData(Map mapData)
    {
        locationTypeDict_SD = new Dictionary<float[], LocationType>();
        Dictionary<Vector2, LocationType> mapDict = mapData.mapDict;
        if (mapData != null)
        {
            foreach (KeyValuePair<Vector2, LocationType> kvp in mapDict)
            {
                float arrayX = kvp.Key.x;
                float arrayY = kvp.Key.y;
                float[] array = new float[] { arrayX, arrayY };
                locationTypeDict_SD.Add(array, kvp.Value);
            }
        }
        biomeDict_SD = new Dictionary<float[], Biomes>();
        Dictionary<Vector2, Biomes> biomeMapDict = mapData.biomesMapDict;
        if (mapData != null)
        {
            foreach (KeyValuePair<Vector2, Biomes> kvp in biomeMapDict)
            {
                float arrayX = kvp.Key.x;
                float arrayY = kvp.Key.y;
                float[] array = new float[] { arrayX, arrayY };
                biomeDict_SD.Add(array, kvp.Value);
            }
        }

        kingdomDict_SD = new Dictionary<float[], Kingdoms>();
        Dictionary<Vector2, Kingdoms> kingdomDict = mapData.kingdomMapDict;
        if (mapData != null)
        {
            foreach (KeyValuePair<Vector2, Kingdoms> kvp in kingdomDict)
            {
                float arrayX = kvp.Key.x;
                float arrayY = kvp.Key.y;
                float[] array = new float[] { arrayX, arrayY };
                kingdomDict_SD.Add(array, kvp.Value);
            }
        }

    }

}

[System.Serializable]
public class ModdedItemSaveData
{
    public Dictionary<Item_SO, Dictionary<ItemVars, int>> ItemIntMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, int>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, string>> ItemStringMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, string>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>> ItemResourceMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, Elements>> ItemElementMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, Elements>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>> ItemBuffMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>> ItemDebuffMods_SD = new Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>>();

    public ModdedItemSaveData(ModdedItems moddedItems)
    {
        ItemIntMods_SD = moddedItems.ItemIntMods;
        ItemStringMods_SD = moddedItems.ItemStringMods;
        ItemResourceMods_SD = moddedItems.ItemResourceMods;
        ItemElementMods_SD = moddedItems.ItemElementMods;
        ItemBuffMods_SD = moddedItems.ItemBuffMods;
        ItemDebuffMods_SD = moddedItems.ItemDebuffMods;
    }

}


[System.Serializable]
public class ModdedAbilitySaveData
{
    public Dictionary<Ability_SO, Dictionary<AbilityVars, int>> AbilityIntMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, int>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, string>> AbilityStringMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, string>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>> AbilityResourceMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>> AbilityElementMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>> AbilityBuffMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>> AbilityDebuffMods_SD = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>>();


    public ModdedAbilitySaveData(ModdedAbilities moddedAbilities)
    {
        AbilityIntMods_SD = moddedAbilities.AbilityIntMods;
        AbilityStringMods_SD = moddedAbilities.AbilityStringMods;
        AbilityResourceMods_SD = moddedAbilities.AbilityResourceMods;
        AbilityElementMods_SD = moddedAbilities.AbilityElementMods;
        AbilityBuffMods_SD = moddedAbilities.AbilityBuffMods;
        AbilityDebuffMods_SD = moddedAbilities.AbilityDebuffMods;
    }
}


[System.Serializable]
public class AlchemyData
{
    public Dictionary<AlchemyTools, bool> AvailableTools_SD = new Dictionary<AlchemyTools, bool>();
    public Dictionary<Ether_SO, int> PlayerEther_SD = new Dictionary<Ether_SO, int> { };
    public Dictionary<Elements, int> KnowledgeDict_SD = new Dictionary<Elements, int>();

    public AlchemyData(AlchemyHandler alchemyHandler)
    {
        AvailableTools_SD = alchemyHandler.AvailableTools;
        PlayerEther_SD = alchemyHandler.PlayerEther;
        KnowledgeDict_SD = alchemyHandler.KnowledgeDict;
    }
}

public class EquipmentData
{
    public Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>> allEquipmentDicts_SD = new Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>>();
    public List<Item_SO> playerEquippedItems_SD = new List<Item_SO>();

    public EquipmentData(EquipmentHandler equipmentHandler)

    {
        allEquipmentDicts_SD = equipmentHandler.allEquipmentDicts;
        playerEquippedItems_SD = equipmentHandler.playerEquippedItems;
    }
}

