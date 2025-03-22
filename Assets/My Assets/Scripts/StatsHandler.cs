using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using AbilityEnums;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.ComponentModel;



public class StatsHandler : MonoBehaviour
{
    #region All class variables

    #region // char description
    public string characterName { private set; get; } = "Sqreegler";
    [SerializeField] public string description { private set; get; }
    [SerializeField] public Combatants charType { private set; get; }
    [SerializeField] public Difficulty difficulty { private set; get; }
    #endregion

    #region //  resources
    [SerializeField] public int MaxHealth { private set; get; } = 5;
    [SerializeField] public int MaxPower { private set; get; } = 5;
    [SerializeField] public int MaxStamina { private set; get; } = 5;
    [SerializeField] public int initiative { private set; get; } = 1;

    public int ActionPoints { private set; get; } = 1;
    public int currentActionPoints { private set; get; } = 1;
    public int ActionPointRegen { private set; get; } = 1;

    [SerializeField] public int currentHealth { private set; get; } = 5;
    [SerializeField] public int currentPower { private set; get; } = 5;
    [SerializeField] public int currentStamina { private set; get; } = 5;
    [SerializeField] public int currentOverHealth { private set; get; } = 0;
    #endregion

    #region // Elemental Affinities
    public int ColdAffinity { private set; get; } = 0;
    public int WaterAffinity { private set; get; } = 0;
    public int EarthAffinity { private set; get; } = 0;
    public int HeatAffinity { private set; get; } = 0;
    public int FireAffinity { private set; get; } = 0;
    public int AirAffinity { private set; get; } = 0;
    public int ElectricityAffinity { private set; get; } = 0;
    public int LightAffinity { private set; get; } = 0;
    public int PsychicAffinity { private set; get; } = 0;
    public int FungiAffinity { private set; get; } = 0;
    public int PlantAffinity;// (private set; get;) = 0;
    public int PoisonAffinity { private set; get; } = 0;
    public int AcidAffinity { private set; get; } = 0;
    public int RadiationAffinity { private set; get; } = 0;
    public int BacteriaAffinity { private set; get; } = 0;
    public int VirusAffinity { private set; get; } = 0;
    #endregion
    #region dataStorageDicts
    public List<int> affinityList;
    public Dictionary<string, int> AffinityDict;
    public Dictionary<Elements, int> ElementAffinityDict;
    public Dictionary<Elements, StatType> ElementStatDict = new Dictionary<Elements, StatType>();
    public Dictionary<StatType, (Func<int> Get, Action<int> Set)> CharVarsDict = new Dictionary<StatType, (Func<int> Get, Action<int> Set)>();
    #endregion

    #region // Physical Affinityences 
    public int BludgeoningResist { private set; get; } = 0;
    public int SlashingResist { private set; get; } = 0;
    public int PiercingResist { private set; get; } = 0;
    public Elements Element { private set; get; } = Elements.None;
    #endregion

    #region // resource regen
    [SerializeField] public int HealthRegen { private set; get; } = 1;
    [SerializeField] public int PowerRegen { private set; get; } = 1;
    [SerializeField] public int StaminaRegen { private set; get; } = 1;

    #endregion

    #region // "Level Stats"

    [SerializeField] public int characterLevel { private set; get; } = 1;
    [SerializeField] public int availableStatPoints { private set; get; } = 40;
    [SerializeField] public int currentXp { private set; get; } = 0;
    [SerializeField] public int MaxXp { private set; get; } = 100;


    #endregion

    #region // inventory
    [SerializeField] public List<Rewards> rewards { private set; get; } = new List<Rewards> { Rewards.Gold, Rewards.Xp };
    [SerializeField] public int characterGold { private set; get; } = 0;
    public Dictionary<Item_SO, int> Inventory { private set; get; } = new Dictionary<Item_SO, int>();
    public Dictionary<string, int> Inventory_save { private set; get; } = new Dictionary<string, int>();

    [SerializeField] public List<Ability_SO> knownAbilities { private set; get; } = new List<Ability_SO>();
    [SerializeField] public List<Abilities> knownAbilities_save { private set; get; } = new List<Abilities>();

    #endregion
    #endregion
    public void Awake()
    {
        //AffinityDict = GetAffinityDict();
        CharVarsDict = GetCharVarDict();
        ElementAffinityDict = GetElementAffinityDict();
        ElementStatDict = GetElementToStatDict();
    }

    #region // getters
    private bool IsAlive()
    {
        if (currentHealth <= 0) return false;
        else return true;
    }
    #endregion

    #region //Get Strings
    public string GetKnownAbilitiesString()
    {
        return string.Join(", ", knownAbilities.Select(a => a.AbilityName));
    }
    public List<Abilities> SetKnownAbilities_Save()
    {
        if (knownAbilities != null)
        {
            foreach (Ability_SO ability in knownAbilities)
            {
                if (ability != null)
                {
                    KDebug.SeekBug($"checking for {ability}. length is {knownAbilities.Count}");
                    if (AbilityLibrary.reverseAbilityDict.TryGetValue(ability, out Abilities abilityEnum))
                    {
                        if (!knownAbilities_save.Contains(abilityEnum))
                        {
                            knownAbilities_save.Add(abilityEnum);
                        }
                        else KDebug.SeekBug($"{abilityEnum} already exists in save list");
                    }
                    else KDebug.SeekBug($"{ability.name} not found in the reverse dict");
                }
                else Debug.Log("You have a null ability in your known abilities list");

            }
        }
        else knownAbilities = new List<Ability_SO> { AbilityLibrary.allAbilities[0], AbilityLibrary.allAbilities[1] };
        return knownAbilities_save;
    }

    public List<Ability_SO> ConvertLoadedAbilities(List<Abilities> knownAbilityEnums)
    {
        knownAbilities = new List<Ability_SO>();
        foreach (Abilities abilityEnum in knownAbilityEnums)
        {
            Ability_SO ability = AbilityLibrary.abilityDict[abilityEnum];
            knownAbilities.Add(ability);
        }
        return knownAbilities;
    }
    public string GetCharInfo()
    {
        string knownAbilitiesString = GetKnownAbilitiesString();
        string charInfo = "";
        if (charType == Combatants.Player)
        {
            charInfo = $"Character Name: {characterName} || Description: {description} || Max Health: {MaxHealth} || Max Power: {MaxPower} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Power: {currentPower} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Power Regen: {PowerRegen} || Stamina Regen: {StaminaRegen} || Character Level: {characterLevel} || Available Stat Points: {availableStatPoints} || Current XP: {currentXp} Max XP: {MaxXp} || Known Abilities: {string.Join(", ", knownAbilitiesString)}";
            return charInfo;
        }
        else
        {
            charInfo = $"Character Name: {characterName} || Description: {description} || Char Type: {charType} || Difficulty: {difficulty} Max Health: {MaxHealth} || Max Power: {MaxPower} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Power: {currentPower} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Power Regen: {PowerRegen} || Stamina Regen: {StaminaRegen} || Character Level: {characterLevel} || Available Stat Points: {availableStatPoints} || Current XP: {currentXp} Max XP: {MaxXp} || Known Abilities: {string.Join(", ", knownAbilitiesString)}";
        }

        return charInfo;
    }
    public string GetCharCreationStats()
    {
        string charInfo = $"Character Name: {characterName} || Description: {description} || Max Health: {MaxHealth} || Max Power: {MaxPower} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Health Regen: {HealthRegen} || Power Regen: {PowerRegen} || Stamina Regen: {StaminaRegen} || Action Points: {ActionPoints} || Action Point Regen Rate: {ActionPointRegen}";

        return charInfo;
    }
    public string GetStatCosts()
    {
        string statCosts = $"Stat Point cost per stat increase: 5 Max Health Power or Stamina: 1  || 1 Health, Power or Stamina Regen: 3 || 1 Max Action Point or Action Point per turn regeneration: 20 || 5% Elemental Affinity: 1 || 5% Physical Resistance: 1 ||";
        return statCosts;
    }

    public string GetAvailableStatPoints()
    {
        string availableStatPointsString = $"Available Stat Points: {availableStatPoints}";
        return availableStatPointsString;
    }
    #endregion

    #region // GetDictionaries

    public void IncrementAttribute(StatType stat, int increment, int cost, bool overrideCost = false)
    {
        if (availableStatPoints > cost || overrideCost)
        {
            if (!overrideCost)
            {
                availableStatPoints -= cost;
            }
            int currentStatValue = CharVarsDict[stat].Get();
            int newValue = currentStatValue + increment;
            CharVarsDict[stat].Set(newValue);
            Debug.Log($"your attribute ({stat}) is {CharVarsDict[stat]}");
            //Debug.Log($"Where as your cold affinity is {ColdAffinity}");
        }
        else Debug.Log("the increment attribute function while the target lacked sufficent talent points or there was no override for the increment");
    }
    private Dictionary<Elements, int> GetElementAffinityDict()
    {
        ElementAffinityDict = new Dictionary<Elements, int>
        {
            { Elements.Cold, ColdAffinity },
            { Elements.Water, WaterAffinity },
            { Elements.Earth, EarthAffinity },
            { Elements.Heat, HeatAffinity },
            { Elements.Fire, FireAffinity },
            { Elements.Air, AirAffinity },
            { Elements.Electricity, ElectricityAffinity },
            { Elements.Light, LightAffinity },
            { Elements.Psychic, PsychicAffinity },
            { Elements.Fungi, FungiAffinity },
            { Elements.Plant, PlantAffinity },
            { Elements.Poison, PoisonAffinity },
            { Elements.Acid, AcidAffinity },
            { Elements.Radiation, RadiationAffinity },
            { Elements.Bacteria, BacteriaAffinity },
            { Elements.Virus, VirusAffinity }

        };
        return ElementAffinityDict;
    }
    private Dictionary<Elements, StatType> GetElementToStatDict()
    {
        ElementStatDict = new Dictionary<Elements, StatType>
        {
            { Elements.Cold, StatType.ColdAffinity },
            { Elements.Water, StatType.WaterAffinity },
            { Elements.Earth, StatType.EarthAffinity },
            { Elements.Heat, StatType.HeatAffinity },
            { Elements.Fire, StatType.FireAffinity },
            { Elements.Air, StatType.AirAffinity },
            { Elements.Electricity, StatType.ElectricityAffinity },
            { Elements.Light, StatType.LightAffinity },
            { Elements.Psychic, StatType.PsychicAffinity },
            { Elements.Fungi, StatType.FungiAffinity },
            { Elements.Plant, StatType.PlantAffinity },
            { Elements.Poison, StatType.PoisonAffinity },
            { Elements.Acid, StatType.AcidAffinity },
            { Elements.Radiation, StatType.RadiationAffinity },
            { Elements.Bacteria, StatType.BacteriaAffinity },
            { Elements.Virus, StatType.VirusAffinity }

        };
        return ElementStatDict;
    }

    private Dictionary<StatType, (Func<int> Get, Action<int> Set)> GetCharVarDict()
    {
        Dictionary<StatType, (Func<int> Get, Action<int> Set)> CharVarsDict = new Dictionary<StatType, (Func<int> Get, Action<int> Set)>
        {
            { StatType.Health, (() => MaxHealth, v => MaxHealth = v)},
            { StatType.Power, (() => MaxPower, v => MaxPower = v) },
            { StatType.Stamina, (() => MaxStamina, v => MaxStamina = v) },

            { StatType.HealthRegen, (() => HealthRegen, v => HealthRegen = v) }, // Initiative might not fit here; check if it should be HealthRegen
            { StatType.PowerRegen, (() => PowerRegen, v => PowerRegen = v) }, // Same concern, should it be something else?
            { StatType.StaminaRegen, (() => StaminaRegen, v => StaminaRegen = v) },

            { StatType.ActionPoints, (() => ActionPoints, v => ActionPoints = v) },
            { StatType.ActionRegen, (() => ActionPointRegen, v => ActionPointRegen = v) },

            { StatType.ColdAffinity, (() => ColdAffinity, v => ColdAffinity = v) },
            { StatType.WaterAffinity, (() => WaterAffinity, v => WaterAffinity = v) },
            { StatType.EarthAffinity, (() => EarthAffinity, v => EarthAffinity = v) },
            { StatType.HeatAffinity, (() => HeatAffinity, v => HeatAffinity = v) },
            { StatType.FireAffinity, (() => FireAffinity, v => FireAffinity = v) },
            { StatType.AirAffinity, (() => AirAffinity, v => AirAffinity = v) },
            { StatType.ElectricityAffinity, (() => ElectricityAffinity, v => ElectricityAffinity = v) },
            { StatType.LightAffinity, (() => LightAffinity, v => LightAffinity = v) },
            { StatType.FungiAffinity, (() => FungiAffinity, v => FungiAffinity = v) },
            { StatType.PlantAffinity, (() => PlantAffinity, v => PlantAffinity = v) },
            { StatType.PoisonAffinity, (() => PoisonAffinity, v => PoisonAffinity = v) },
            { StatType.AcidAffinity, (() => AcidAffinity, v => AcidAffinity = v) },
            { StatType.RadiationAffinity, (() => RadiationAffinity, v => RadiationAffinity = v) },
            { StatType.BacteriaAffinity, (() => BacteriaAffinity, v => BacteriaAffinity = v) },
            { StatType.VirusAffinity, (() => VirusAffinity, v => VirusAffinity = v) },
            { StatType.PsychicAffinity, (() => PsychicAffinity, v => PsychicAffinity = v) },

            { StatType.BludgeoningResistance, (() => BludgeoningResist, v => BludgeoningResist = v) },
            { StatType.SlashingResistance, (() => SlashingResist, v => SlashingResist = v) },
            { StatType.PiercingResistance, (() => PiercingResist, v => PiercingResist = v) }
        };
        return CharVarsDict;
    }


    private Dictionary<PhysicalDamage, int> GetPhysicalResistDict()
    {
        Dictionary<PhysicalDamage, int> PhysicalResistDict = new Dictionary<PhysicalDamage, int>
        {
            { PhysicalDamage.Bludgeoning, BludgeoningResist },
            { PhysicalDamage.Slashing, SlashingResist },
            { PhysicalDamage.Piercing, PiercingResist }
        };
        return PhysicalResistDict;
    }
    public int GetResourceAmount(ResourceTypes resourceType)
    {
        if (resourceType == ResourceTypes.Health)
        {
            return currentHealth;
        }
        if (resourceType == ResourceTypes.Power)
        {
            return currentPower;
        }
        if (resourceType == ResourceTypes.Stamina)
        {
            return currentStamina;
        }
        else return 777;
    }
    private int AdjustValue(int value, Elements element = Elements.None, PhysicalDamage physicalType = PhysicalDamage.None)
    {
        KDebug.SeekBug($"{value} = value. element type =  {element}");
        //this method adjusts the value of an attack based on the affinity of the target
        int relevantAffinity = 0;
        if (element != Elements.None)
        {
            Dictionary<Elements, int> ElementAffinityDict = GetElementAffinityDict();
            relevantAffinity = ElementAffinityDict[element];
        }

        if (physicalType != PhysicalDamage.None)
        {
            Dictionary<PhysicalDamage, int> physicalResistDict = GetPhysicalResistDict();
            relevantAffinity = physicalResistDict[physicalType];
        }

        if (relevantAffinity > 100)
        {
            value = (int)Math.Abs(Math.Round((value * ((relevantAffinity - 100) / 100.0))));
        }

        if (relevantAffinity == 0) return value;

        if (relevantAffinity <= 100 && relevantAffinity > 0)
        {
            value = (int)Math.Round(value * 1.0 - (value * (relevantAffinity / 100)));
            KDebug.SeekBug($"value after adjustment = {value}");
        }

        return value;
    }
    public void SetElement(Elements element)
    {
        if (ElementStatDict.TryGetValue(element, out StatType stat))
        {
            CharVarsDict[stat].Set(25);
            Debug.Log($"creatures element {element} has been raised to {CharVarsDict[stat].Get()}");

            Element = element;
        }
        else
        {
            Debug.Log($"{element} not found in elements Dict");
            Element = Elements.Bacteria;
            CharVarsDict[StatType.BacteriaAffinity].Set(25);
        }
    }

    public string GetAffinityString()
    {
        string affinityString = "";
        foreach (KeyValuePair<StatType, (Func<int> Get, Action<int> Set)> kvp in CharVarsDict)
        {
            if (kvp.Key.ToString().Contains("Affinity") || kvp.Key.ToString().Contains("Resistance"))
            {
                if (kvp.Value.Get() > 0)
                {
                    affinityString += $"{GeneralFunctions.AddSpaceToEnum(kvp.Key)}: {kvp.Value.Get()} \n";
                }
            }
        }
        return affinityString;
    }



    #endregion



    #region // Setters
    #region // stat point usage
    public void RestoreResources()
    {
        currentHealth = MaxHealth;
        currentPower = MaxPower;
        currentStamina = MaxStamina;
        currentActionPoints = ActionPoints;
    }


    #endregion

    public void SetName(string name)
    {
        characterName = name;
    }
    public void SetDescription(string newDesciption)
    {
        description = newDesciption;
    }

    public void GainXp(int XpGain)
    {
        currentXp = currentXp + XpGain;
        if (currentXp > MaxXp)
        {
            currentXp -= MaxXp;
            GainLevel();
        }
        KDebug.SeekBug($"{characterName} gained {XpGain}");
    }
    public void LearnAbility(Abilities newAbility)
    {
        if (AbilityLibrary.abilityDict.ContainsKey(newAbility))
        {
            if (!knownAbilities.Contains(AbilityLibrary.abilityDict[newAbility]))
            {
                knownAbilities.Add(AbilityLibrary.abilityDict[newAbility]);
                Debug.Log($"{newAbility} added to your known abilities list");
            }
            else Debug.Log($"You already know {newAbility}?");
            //SetKnownAbilities_Save();
        }
        else Debug.Log($"{newAbility} does not exist in the AbilityLibrary and can therefore not be added to your known abilities list");

    }
    #region // inventory
    public void ChangeGold(int GoldAmount)
    {
        KDebug.SeekBug($"{characterName} cahnge amount of gold by: {GoldAmount}");
        characterGold += GoldAmount;
    }

    public void AddToInventory(Item_SO item, int amount = 1)
    {
        if (Inventory.TryGetValue(item, out int num))
        {
            Inventory[item] += amount;
        }
        else
        {
            Inventory.Add(item, amount);
        }
    }
    public void RemoveFromInventory(Item_SO item, int amount = 1)
    {
        if (Inventory.TryGetValue(item, out int num))
        {
            Inventory[item] -= amount;
            if (Inventory[item] == 0)
            {
                Inventory.Remove(item);
            }
        }
        else
        {
            Inventory.Remove(item);
        }
    }
    public int GetNumItemsInInventory(Item_SO item)
    {
        int itemNum = 0;
        if (Inventory.TryGetValue(item, out int num))
        {
            itemNum = num;
        }
        return itemNum;
    }

    public Dictionary<string, int> SetInventory_save()
    {
        Debug.Log(GetInventoryString());
        Debug.Log($"SetInventory_Save func will here convert inv of length: {Inventory.Count} into string form");
        foreach (KeyValuePair<Item_SO, int> invSlot in Inventory)
        {
            if (!Inventory_save.TryGetValue(invSlot.Key.ItemName, out int num))
            {
                Inventory_save.Add(invSlot.Key.ItemName, invSlot.Value);
                Debug.Log($"SetInventory-Save method: Adding {num} of the item {invSlot.Key.ItemName} to inventory save variable in playerStats");
            }
        }
        return Inventory_save;
    }
    public string GetInventoryString()
    {
        string inv = "";
        foreach (KeyValuePair<Item_SO, int> kvp in Inventory)
        {
            inv += $"item : {kvp.Key}. num owned: {kvp.Value}";
        }
        return inv;
    }

    public Dictionary<Item_SO, int> ConvertLoadedInventory(Dictionary<string, int> loadedInv) // searches World Chest's items for an item with the specified name
    {
        //Debug.Log($"Converting loaded inventory with item Count: {loadedInv.Count}.");
        Inventory = new Dictionary<Item_SO, int>();
        foreach (KeyValuePair<string, int> kvp in loadedInv)
        {
            Item_SO newItem = WorldChest.GetItemFromName(kvp.Key);
            Inventory.Add(newItem, kvp.Value);
            //Debug.Log($"ConvertLoadedInventory method: Adding {kvp.Value} of item {newItem} to players Inv. Items in inv = {Inventory.Count}/ Num of this  item: {kvp.Value}");
        }
        return Inventory;
    }
    #endregion
    private void GainLevel()
    {
        characterLevel++;
        MaxXp = MaxXp * 2;
    }
    public bool CheckSuffientResource(ResourceTypes resource, int value)
    {
        if (resource == ResourceTypes.Health) return value <= currentHealth;
        if (resource == ResourceTypes.Power) return value <= currentPower;
        if (resource == ResourceTypes.Stamina) return value <= currentStamina;
        else return false;
    }
    public void ChangeResource(ResourceTypes resource, int value, Elements element = Elements.None, PhysicalDamage physicalType = PhysicalDamage.None)

    {
        value = AdjustValue(value, element, physicalType); //adjusts damage based on resistances

        switch (resource)
        {
            case ResourceTypes.Health:
                currentHealth += value;
                if (currentHealth > MaxHealth + currentOverHealth) currentHealth = MaxHealth;
                break;
            case ResourceTypes.Power:
                currentPower += value;
                if (currentPower > MaxPower) currentPower = MaxPower;
                break;
            case ResourceTypes.Stamina:
                currentStamina += value;
                if (currentStamina > MaxStamina) currentStamina = MaxStamina;
                break;
        }
    }
    public void RegenActionPoints()
    {
        currentActionPoints += ActionPointRegen;
        if (currentActionPoints > ActionPoints) currentActionPoints = ActionPoints;
    }
    public void SpendActionPoints()
    {
        currentActionPoints -= 1;
    }
    private void GiveOverHealth(int overHealthAmount)
    {
        MaxHealth += overHealthAmount;
        currentHealth += overHealthAmount;
        currentOverHealth += overHealthAmount;
    }
    #endregion

    #region // make player and creatures;
    public GameObject MakePlayer()
    {
        characterName = "Borgauss";
        description = "World dominator currently in fetus-form";
        charType = Combatants.Player;
        difficulty = Difficulty.Easy;
        MaxHealth = 10;
        MaxPower = 10;
        MaxStamina = 10;
        initiative = 100;
        ActionPoints = 1;
        ActionPointRegen = 1;
        currentHealth = 10;
        currentPower = 10;
        currentStamina = 10;
        ColdAffinity = 0;
        WaterAffinity = 0;
        EarthAffinity = 0;
        HeatAffinity = 0;
        FireAffinity = 0;
        AirAffinity = 0;
        ElectricityAffinity = 0;
        LightAffinity = 0;
        PsychicAffinity = 0;
        FungiAffinity = 0;
        PlantAffinity = 0;
        PoisonAffinity = 0;
        AcidAffinity = 0;
        RadiationAffinity = 0;
        BacteriaAffinity = 0;
        VirusAffinity = 0;
        BludgeoningResist = 0;
        SlashingResist = 0;
        PiercingResist = 0;
        HealthRegen = 0;
        PowerRegen = 0;
        StaminaRegen = 0;
        characterLevel = 0;
        availableStatPoints = 40;
        currentXp = 0;
        MaxXp = 30;
        rewards = new List<Rewards>();
        characterGold = 100;
        return gameObject;

    }
    private void AlterStats(Combatants combatantType, int scaler, Elements element)
    {
        if (combatantType != Combatants.Summon)
        {
            MaxHealth = UnityEngine.Random.Range(1, 10) * scaler;
            MaxPower = UnityEngine.Random.Range(1, 10) * scaler;
            MaxStamina = UnityEngine.Random.Range(1, 10) * scaler;
            initiative = scaler;
            HealthRegen = scaler;
            PowerRegen = scaler;
            StaminaRegen = scaler;
            characterLevel = scaler;
        }

    }
    public void LearnApplicableAbilities(Elements element, int scaler)
    {
        foreach (Abilities ability in AbilityLibrary.GetAbilities(scaler, element))
        {
            Debug.Log($"Checking whether {ability} can be learned");
            if (AbilityLibrary.reverseAbilityDict != null && AbilityLibrary.allAbilities.Contains(AbilityLibrary.abilityDict[ability]))
            {
                LearnAbility(ability);
            }
            else Debug.Log("the reverse dict struggled");
        }
    }

    private Elements GetRandomCreatureElement()
    {
        Array elements = Enum.GetValues(typeof(Elements));
        System.Random random = new System.Random();
        Elements randomElement = Elements.None;
        while (randomElement == Elements.None)
        {
            randomElement = (Elements)elements.GetValue(random.Next(elements.Length));
        }
        SetElement(randomElement);
        return randomElement;
    }
    private string GetElementRelatedName(Elements element, Difficulty difficultyLevel)
    {
        //returns a random adjective concatenated to a random name
        Dictionary<Elements, string[]> elementDict = Vocabulary.MakeElementAdjectiveDict();
        string elementAdjective = elementDict[element][UnityEngine.Random.Range(0, elementDict[element].Length)];
        string baseName = Vocabulary.GetRandomVillainousCreatures((int)difficultyLevel);

        string creatureName = $"{elementAdjective} {baseName}";
        return creatureName;
    }
    public GameObject MakeCreature(Difficulty difficultyLevel, Combatants combatantType)
    {
        Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel}");
        difficulty = difficultyLevel;
        int scaler = (int)difficultyLevel + 1;
        Elements element = Elements.None;
        charType = combatantType;
        if (charType != Combatants.Enemy) characterName = combatantType.ToString();
        else
        {
            element = GetRandomCreatureElement();
            characterName = GetElementRelatedName(element, difficultyLevel);
            IncrementAttribute(ElementStatDict[element], 6 * scaler, 0, true);
        }
        AlterStats(combatantType, scaler, element);
        LearnApplicableAbilities(element, scaler);
        RestoreResources();
        return gameObject;
    }
    #endregion

    public void LoadStats()
    {
        PlayerSaveData saveData = SaveSystem.LoadPlayerData();

        characterName = saveData.characterName_SD;
        description = saveData.description_SD;

        MaxHealth = saveData.MaxHealth_SD;
        MaxPower = saveData.MaxPower_SD;
        MaxStamina = saveData.MaxStamina_SD;

        currentHealth = saveData.currentHealth_SD;
        currentOverHealth = saveData.currentOverHealth_SD;
        currentPower = saveData.currentPower_SD;
        currentStamina = saveData.currentStamina_SD;

        HealthRegen = saveData.HealthRegen_SD;
        PowerRegen = saveData.PowerRegen_SD;
        StaminaRegen = saveData.StaminaRegen_SD;

        ActionPoints = saveData.ActionPoints_SD;
        currentActionPoints = saveData.currentActionPoints_SD;
        ActionPointRegen = saveData.ActionPointRegen_SD;

        initiative = saveData.initiative_SD;
        knownAbilities = ConvertLoadedAbilities(saveData.knownAbilities_SD); // must be replaced with non-scriptable object data types
        Debug.Log(GetInventoryString());
        Inventory = ConvertLoadedInventory(saveData.inventory_SD); //this is empty when passed in
        Debug.Log(GetInventoryString());

        characterGold = saveData.characterGold_SD;

        ColdAffinity = saveData.ColdAffinity_SD;
        WaterAffinity = saveData.WaterAffinity_SD;
        EarthAffinity = saveData.EarthAffinity_SD;
        HeatAffinity = saveData.HeatAffinity_SD;
        FireAffinity = saveData.FireAffinity_SD;
        AirAffinity = saveData.AirAffinity_SD;
        ElectricityAffinity = saveData.LightAffinity_SD;
        LightAffinity = saveData.LightAffinity_SD;
        PsychicAffinity = saveData.PsychicAffinity_SD;
        FungiAffinity = saveData.FungiAffinity_SD;
        PlantAffinity = saveData.PlantAffinity_SD;
        PoisonAffinity = saveData.PlantAffinity_SD;
        AcidAffinity = saveData.AcidAffinity_SD;
        RadiationAffinity = saveData.RadiationAffinity_SD;
        BacteriaAffinity = saveData.BacteriaAffinity_SD;
        VirusAffinity = saveData.VirusAffinity_SD;

        BludgeoningResist = saveData.BludgeoningResist_SD;
        SlashingResist = saveData.SlashingResist_SD;
        PiercingResist = saveData.PiercingResist_SD;

        characterLevel = saveData.characterLevel_SD;
        availableStatPoints = saveData.availableStatPoints_SD;
        currentXp = saveData.currentXp_SD;
        MaxXp = saveData.MaxXp_SD;
    }

}

