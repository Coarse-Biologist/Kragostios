using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using AbilityEnums;
using System;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;


public class StatsHandler : MonoBehaviour
{
    #region // class references
    private Ability_SO ability;
    [SerializeField] public AbilityLibrary abilityLibrary;
    #endregion

    #region // char description
    public string characterName { private set; get; } = "Sqreegler";
    [SerializeField] public string description { private set; get; }
    [SerializeField] public Combatants charType { private set; get; }
    [SerializeField] public Difficulty difficulty { private set; get; }
    #endregion

    #region //  resources

    [SerializeField] public int MaxHealth { private set; get; } = 100;
    [SerializeField] public int MaxMana { private set; get; } = 100;
    [SerializeField] public int MaxStamina { private set; get; } = 100;
    [SerializeField] public int initiative { private set; get; } = 100;

    public int ActionPoints { private set; get; } = 1;
    public int currentActionPoints { private set; get; } = 1;
    public int ActionPointRegen { private set; get; } = 1;

    [SerializeField] public int currentHealth { private set; get; } = 100;
    [SerializeField] public int currentMana { private set; get; } = 100;
    [SerializeField] public int currentStamina { private set; get; } = 100;
    [SerializeField] public int currentOverHealth { private set; get; } = 0;
    #endregion

    #region // Elemental Affinityences
    public int ColdAffinity { private set; get; } = 0;
    //public int IceAffinity {private set; get;} = 0;
    public int WaterAffinity { private set; get; } = 0;
    public int EarthAffinity { private set; get; } = 0;
    public int HeatAffinity { private set; get; } = 0;
    //public int LavaAffinity {private set; get;} = 0;
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

    public List<int> affinityList;
    public Dictionary<string, int> AffinityDict;
    public Dictionary<Elements, int> ElementAffinityDict;

    public void Awake()
    {
        AffinityDict = GetAffinityDict();
    }


    #endregion

    #region // Physical Affinityences 
    public int BludgeoningResist { private set; get; } = 0;
    public int SlashingResist { private set; get; } = 0;
    public int PiercingResist { private set; get; } = 0;

    #endregion

    #region // resource regen
    [SerializeField] public int HealthRegen { private set; get; } = 1;
    [SerializeField] public int ManaRegen { private set; get; } = 1;
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
    [SerializeField] public List<Ability_SO> knownAbilities { private set; get; } = new List<Ability_SO>();

    #endregion

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
        string knownAbilitiesString = "";
        foreach (Ability_SO ability in knownAbilities)
        {
            knownAbilitiesString += ability.AbilityName + ", ";
        }
        return knownAbilitiesString;
    }
    public string GetCharInfo()
    {
        string knownAbilitiesString = GetKnownAbilitiesString();
        string charInfo = "";
        if (charType == Combatants.Player)
        {
            charInfo = $"Character Name: {characterName} || Description: {description} || Max Health: {MaxHealth} || Max Mana: {MaxMana} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Mana: {currentMana} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Mana Regen: {ManaRegen} || Stamina Regen: {StaminaRegen} || Character Level: {characterLevel} || Available Stat Points: {availableStatPoints} || Current XP: {currentXp} Max XP: {MaxXp} || Known Abilities: {string.Join(", ", knownAbilitiesString)}";
            return charInfo;
        }
        else
        {
            charInfo = $"Character Name: {characterName} || Description: {description} || Char Type: {charType} || Difficulty: {difficulty} Max Health: {MaxHealth} || Max Mana: {MaxMana} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Mana: {currentMana} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Mana Regen: {ManaRegen} || Stamina Regen: {StaminaRegen} || Character Level: {characterLevel} || Available Stat Points: {availableStatPoints} || Current XP: {currentXp} Max XP: {MaxXp} || Known Abilities: {string.Join(", ", knownAbilitiesString)}";
        }

        return charInfo;
    }
    public string GetCharCreationStats()
    {
        string charInfo = $"Character Name: {characterName} || Description: {description} || Max Health: {MaxHealth} || Max Mana: {MaxMana} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Health Regen: {HealthRegen} || Mana Regen: {ManaRegen} || Stamina Regen: {StaminaRegen} || Action Points: {ActionPoints} || Action Point Regen Rate: {ActionPointRegen}";

        return charInfo;
    }
    public string GetStatCosts()
    {
        string statCosts = $"Stat Point cost per stat increase: 5 Max Health Mana or Stamina: 1  || 1 Health, Mana or Stamina Regen: 3 || 1 Max Action Point or Action Point per turn regeneration: 20 || 5% Elemental Affinity: 1 || 5% Physical Resistance: 1 ||";
        return statCosts;
    }

    public string getAvailableStatPoints()
    {
        string availableStatPointsString = $"Available Stat Points: {availableStatPoints}";
        return availableStatPointsString;
    }
    #endregion

    #region // GetDictionaries
    public Dictionary<string, int> GetAffinityDict()
    {
        // Create a new dictionary with string keys and int values
        AffinityDict = new Dictionary<string, int>
        {
            { "Cold Affinity", ColdAffinity },
            //{ "Ice Affinity", IceAffinity },
            { "Water Affinity", WaterAffinity },
            { "Earth Affinity", EarthAffinity },
            { "Heat Affinity", HeatAffinity },
            //{ "Lava Affinity", LavaAffinity },
            { "Fire Affinity", FireAffinity },
            { "Air Affinity", AirAffinity },
            { "Electricity Affinity", ElectricityAffinity },
            { "Light Affinity", LightAffinity },
            { "Psychic Affinity", PsychicAffinity },
            { "Fungi Affinity", FungiAffinity },
            { "Plant Affinity", PlantAffinity },
            { "Poison Affinity", PoisonAffinity },
            { "Acid Affinity", AcidAffinity },
            { "Radiation Affinity", RadiationAffinity },
            { "Bacteria Affinity", BacteriaAffinity },
            { "Virus Affinity", VirusAffinity }
        };
        return AffinityDict;

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
        if (resourceType == ResourceTypes.Mana)
        {
            return currentMana;
        }
        if (resourceType == ResourceTypes.Stamina)
        {
            return currentStamina;
        }
        else return 777;
    }
    private int AdjustValue(int value, Elements element = Elements.None, PhysicalDamage physicalType = PhysicalDamage.None)
    {
        //this method adjusts the value of an attack based on the affinity of the target
        int relevantAffinity = 0;
        if (element != Elements.None)
        {
            KDebug.SeekBug($"{value} = value. element type =  {element}");
            Dictionary<Elements, int> ElementAffinityDict = GetElementAffinityDict();
            relevantAffinity = ElementAffinityDict[element];
        }

        if (physicalType != PhysicalDamage.None)
        {
            KDebug.SeekBug($"{value} = value. element type =  {physicalType}");
            Dictionary<PhysicalDamage, int> physicalResistDict = GetPhysicalResistDict();
            relevantAffinity = physicalResistDict[physicalType];
        }

        if (relevantAffinity > 100) //if one should heal from an attack due to extreme resistence
        {
            value = (int)Math.Abs(Math.Round((value * ((relevantAffinity - 100) / 100.0)))); //calculate how much resistance above 100% they have and multiply the percentage by the value of the attack
        }

        if (relevantAffinity == 0) return value;

        if (relevantAffinity <= 100 && relevantAffinity > 0)
        {
            KDebug.SeekBug($"value prior to adjustment = {value}");
            value = (int)Math.Round(value * 1.0 - (value * (relevantAffinity / 100)));
            KDebug.SeekBug($"value after adjustment = {value}");
        }

        return value;
    }


    public string GetAffinityString()
    {

        string affinityString = "";
        AffinityDict = GetAffinityDict();

        foreach (KeyValuePair<string, int> kvp in AffinityDict)
        {
            affinityString += $"{kvp.Key}: {kvp.Value} \n";
        }
        return affinityString;
    }
    public string GetResistString()
    {
        string resistString = $"Bludgeoning resistance: {BludgeoningResist} \n Slashing resistance: {SlashingResist} \n Piercing resistance: {PiercingResist}";

        return resistString;
    }


    #endregion

    #region //Scaling factors
    private int easyScaleFactor = 2;
    private int mediumScaleFactor = 3;
    private int hardScaleFactor = 4;
    private int brutalScaleFactor = 5;
    private int nightmareScaleFactor = 6;
    #endregion

    #region // Setters

    public void RestoreResources()
    {
        currentHealth = MaxHealth;
        currentMana = MaxMana;
        currentStamina = MaxStamina;
        currentActionPoints = ActionPoints;
    }
    public void AddMaxHealth(int incrementValue, int cost = 0)
    {
        MaxHealth += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddMaxMana(int incrementValue, int cost = 0)
    {
        MaxMana += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddMaxStamina(int incrementValue, int cost = 0)
    {
        MaxStamina += incrementValue;
        availableStatPoints -= cost;
    }

    public void AddActionPoint(int incrementValue, int cost = 0)
    {
        ActionPoints += incrementValue;
        availableStatPoints -= cost;
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

    public void AddActionPointRegen(int incrementValue, int cost = 0)
    {
        ActionPointRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddHealthRegen(int incrementValue, int cost = 0)
    {
        HealthRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddManaRegen(int incrementValue, int cost = 0)
    {
        ManaRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddStaminaRegen(int incrementValue, int cost = 0)
    {
        StaminaRegen += incrementValue;
        availableStatPoints -= cost;
    }

    public void AddColdAffinity(int incrementValue, int cost = 0)
    {
        ColdAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        availableStatPoints -= cost;
    }

    public void AddWaterAffinity(int incrementValue, int cost = 0)
    {
        WaterAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        ColdAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddEarthAffinity(int incrementValue, int cost = 0)
    {
        EarthAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        PlantAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddHeatAffinity(int incrementValue, int cost = 0)
    {
        HeatAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        FireAffinity += (int)Math.Round(splashIncrement, 2);

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);



        availableStatPoints -= cost;
    }

    public void AddFireAffinity(int incrementValue, int cost = 0)
    {
        FireAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        HeatAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddAirAffinity(int incrementValue, int cost = 0)
    {
        AirAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        ElectricityAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddElectricityAffinity(int incrementValue, int cost = 0)
    {
        ElectricityAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;
        AirAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddLightAffinity(int incrementValue, int cost = 0)
    {
        LightAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);

        HeatAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }

    public void AddRadiationAffinity(int incrementValue, int cost = 0)
    {
        RadiationAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        AirAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }

    public void AddPsychicAffinity(int incrementValue, int cost = 0)
    {
        PsychicAffinity += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddFungiAffinity(int incrementValue, int cost = 0)
    {
        FungiAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        PlantAffinity += (int)Math.Round(splashIncrement, 2);

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }
    public void AddPlantAffinity(int incrementValue, int cost = 0)
    {
        PlantAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        FungiAffinity += (int)Math.Round(splashIncrement, 2);

        WaterAffinity += (int)Math.Round(splashIncrement, 2);

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddPoisonAffinity(int incrementValue, int cost = 0)
    {
        PoisonAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }
    public void AddAcidAffinity(int incrementValue, int cost = 0)
    {
        AcidAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        HeatAffinity += (int)Math.Round(splashIncrement, 2);

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }

    public void AddBacteriaAffinity(int incrementValue, int cost = 0)
    {
        BacteriaAffinity += incrementValue;

        decimal splashIncrement = incrementValue / 2;

        VirusAffinity += (int)Math.Round(splashIncrement, 2);

        PoisonAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddVirusAffinity(int incrementValue, int cost = 0)
    {
        VirusAffinity += incrementValue;
        decimal splashIncrement = incrementValue / 2;

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);

        PoisonAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddBludgeoningResist(int incrementValue, int cost = 0)
    {
        BludgeoningResist += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddSlashingResist(int incrementValue, int cost = 0)
    {
        SlashingResist += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddPiercingResist(int incrementValue, int cost = 0)
    {
        PiercingResist += incrementValue;
        availableStatPoints -= cost;
    }

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
        knownAbilities.Add(abilityLibrary.abilityDict[newAbility]);
    }

    public void GainGold(int GoldAmount)
    {
        KDebug.SeekBug($"{characterName} gained {GoldAmount}");
        characterGold += GoldAmount;
    }

    private void GainLevel()
    {
        characterLevel++;
        MaxXp = MaxXp * 2;
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
            case ResourceTypes.Mana:
                currentMana += value;
                if (currentMana > MaxMana) currentMana = MaxMana;
                break;
            case ResourceTypes.Stamina:
                currentStamina += value;
                if (currentStamina > MaxStamina) currentStamina = MaxStamina;
                break;
        }
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
        MaxMana = 10;
        MaxStamina = 10;
        initiative = 100;
        ActionPoints = 1;
        ActionPointRegen = 1;
        currentHealth = 10;
        currentMana = 10;
        currentStamina = 10;
        ColdAffinity = 0;
        //IceAffinity = 0;
        WaterAffinity = 0;
        EarthAffinity = 0;
        HeatAffinity = 0;
        //LavaAffinity = 0;
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
        ManaRegen = 0;
        StaminaRegen = 0;
        characterLevel = 0;
        availableStatPoints = 40;
        currentXp = 0;
        MaxXp = 30;
        rewards = new List<Rewards>();
        characterGold = 0;
        knownAbilities = new List<Ability_SO>{
    abilityLibrary.Melee, abilityLibrary.FireBall, abilityLibrary.DivineStrike, abilityLibrary.HealingTouch, abilityLibrary.ColdLight, abilityLibrary.BrainDamage, abilityLibrary.LavaPortal, abilityLibrary.GlobalCooling
    };

        return gameObject;

    }
    private void AlterStats(int scaleFactor, Combatants combatantType, Difficulty difficultyLevel)
    {
        MaxHealth = UnityEngine.Random.Range(1, 10) * scaleFactor;
        MaxMana = UnityEngine.Random.Range(1, 10) * scaleFactor;
        MaxStamina = UnityEngine.Random.Range(1, 10) * scaleFactor;
        initiative = scaleFactor;
        HealthRegen = scaleFactor;
        ManaRegen = scaleFactor;
        StaminaRegen = scaleFactor;
        characterLevel = scaleFactor;
        knownAbilities = abilityLibrary.GetAbilities(scaleFactor);
    }

    private Elements GetRandomCreatureElement()
    {
        Array elements = Enum.GetValues(typeof(Elements));
        System.Random random = new System.Random();
        Elements randomElement = (Elements)elements.GetValue(random.Next(elements.Length));
        return randomElement;
    }
    private string GetElementRelatedName(Elements element, Difficulty difficultyLevel)
    {
        //returns a random adjective concatenated to a random name
        Dictionary<Elements, string[]> elementDict = Vocabulary.MakeElementAdjectiveDict();
        string elementAdjective = elementDict[element][UnityEngine.Random.Range(0, elementDict[element].Length)];
        string elementName = "";
        switch (difficultyLevel)
        {
            case Difficulty.Easy:
                elementName = Vocabulary.GetRandomlowLevelVillainousCreatures();
                break;
            case Difficulty.Medium:
                elementName = Vocabulary.GetRandomlowLevelVillainousCreatures();
                break;
            case Difficulty.Hard:
                elementName = Vocabulary.GetRandomMidLevelVillainousCreatures();
                break;
            case Difficulty.Brutal:
                elementName = Vocabulary.GetRandomHighLevelVillainousCreatures();
                break;
            case Difficulty.Nightmare:
                elementName = Vocabulary.GetRandomHighLevelVillainousCreatures();
                break;
            default:
                elementName = Vocabulary.GetRandomlowLevelVillainousCreatures();
                break;
        }

        string creatureName = $"{elementAdjective} {elementName}";
        return creatureName;
    }
    private void SetCreatureAffinities(Elements element, Difficulty difficultyLevel)
    {
        KDebug.SeekBug($"setting creature affinities for {element}");
        int added = 0;
        switch (element)
        {
            case Elements.None:
                while (added < (((int)difficultyLevel + 1) * 3))
                {
                    AddBludgeoningResist(5);
                    AddPiercingResist(5);
                    AddSlashingResist(5);
                    added++;
                }
                KDebug.SeekBug($"setting creature affinities for {element}. BludgeoningResist = {BludgeoningResist} PiercingResist = {PiercingResist} SlashingResist = {SlashingResist}");
                break;
            case Elements.Cold:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddColdAffinity(5);
                    added++;
                }
                KDebug.SeekBug($"setting creature affinities for {element}. ColdResist = {ColdAffinity}");

                break;
            case Elements.Water:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddWaterAffinity(5);
                    added++;
                }
                break;
            case Elements.Earth:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddEarthAffinity(5);
                    added++;
                }
                break;
            case Elements.Heat:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddHeatAffinity(5);
                    added++;
                }
                break;
            case Elements.Fire:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddFireAffinity(5);
                    added++;
                }
                break;
            case Elements.Air:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddAirAffinity(5);
                    added++;
                }
                break;
            case Elements.Electricity:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddElectricityAffinity(5);
                    added++;
                }
                break;
            case Elements.Light:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddLightAffinity(5);
                    added++;
                }
                break;
            case Elements.Psychic:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddPsychicAffinity(5);
                    added++;
                }
                break;
            case Elements.Fungi:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddFungiAffinity(5);
                    added++;
                }
                break;
            case Elements.Plant:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddPlantAffinity(5);
                    added++;
                }
                break;
            case Elements.Poison:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddPoisonAffinity(5);
                    added++;
                }
                break;
            case Elements.Acid:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddAcidAffinity(5);
                    added++;
                }
                break;
            case Elements.Radiation:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddRadiationAffinity(5);
                    added++;
                }
                break;
            case Elements.Bacteria:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddBacteriaAffinity(5);
                    added++;
                }
                break;
            case Elements.Virus:
                while (added < (((int)difficultyLevel + 1) * 6))
                {
                    AddVirusAffinity(5);
                    added++;
                }
                break;
        }
    }


    public GameObject MakeCreature(Difficulty difficultyLevel, Combatants combatantType)
    {
        Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel}");
        difficulty = difficultyLevel;
        switch (combatantType)
        {
            case Combatants.Enemy:
                charType = Combatants.Enemy;
                Elements element = GetRandomCreatureElement();
                characterName = GetElementRelatedName(element, difficultyLevel);
                SetCreatureAffinities(element, difficultyLevel);
                break;
            case Combatants.Companion:
                charType = Combatants.Companion;
                characterName = "Companion";
                break;
            case Combatants.Summon:
                charType = Combatants.Summon;
                characterName = "Summon";
                break;
        }

        switch (difficultyLevel)
        {
            case Difficulty.Easy:

                AlterStats(easyScaleFactor, combatantType, difficultyLevel);
                break;

            case Difficulty.Medium:
                AlterStats(mediumScaleFactor, combatantType, difficultyLevel);


                break;
            case Difficulty.Hard:
                AlterStats(hardScaleFactor, combatantType, difficultyLevel);
                break;

            case Difficulty.Brutal:
                AlterStats(brutalScaleFactor, combatantType, difficultyLevel);
                break;

            case Difficulty.Nightmare:
                AlterStats(nightmareScaleFactor, combatantType, difficultyLevel);
                break;

        }
        RestoreResources();
        return gameObject;
    }
    #endregion
    private void Summon(GameObject summon)
    {

    }

}
