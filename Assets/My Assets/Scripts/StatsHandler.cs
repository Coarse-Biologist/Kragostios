using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Accessibility;
using KragostiosAllEnums;
using AbilityEnums;
using System;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;
//using UnityEngine.UI;
//using UnityEditor.SceneManagement;
//using UnityEditor.Rendering;


public class StatsHandler : MonoBehaviour
{

    #region // class references
    private Ability_SO ability;
    [SerializeField] public AbilityLibrary abilityLibrary;
    #endregion

    #region // char description
    [SerializeField] public string characterName {private set; get;} = "Sqreegler";
    [SerializeField] public string description {private set; get;}
    [SerializeField] public Combatants charType{private set; get;}
    [SerializeField] public Difficulty difficulty{private set; get;}
    #endregion

    #region //  resources

    [SerializeField] public int MaxHealth {private set; get;} = 100;
    [SerializeField] public int MaxMana {private set; get;} = 100;
    [SerializeField] public int MaxStamina {private set; get;} = 100;
    [SerializeField] public int initiative {private set; get;} = 100;

    public int ActionPoints {private set; get;} = 1;
    public int ActionPointRegen {private set; get;} = 1;

    [SerializeField] public int currentHealth {private set; get;}= 100;
    [SerializeField] public int currentMana {private set; get;}= 100;
    [SerializeField] public int currentStamina  {private set; get;}= 100;
    #endregion

    #region // Elemental Affinityences
    public int ColdAffinity {private set; get;} = 0;
    //public int IceAffinity {private set; get;} = 0;
    public int WaterAffinity {private set; get;} = 0;
    public int EarthAffinity {private set; get;} = 0;
    public int HeatAffinity {private set; get;} = 0;
    //public int LavaAffinity {private set; get;} = 0;
    public int FireAffinity {private set; get;} = 0;
    public int AirAffinity {private set; get;} = 0;
    public int ElectrictyAffinity {private set; get;} = 0;
    public int LightAffinity {private set; get;} = 0;
    public int PsychicAffinity {private set; get;} = 0;
    public int FungiAffinity {private set; get;} = 0;
    public int PlantAffinity;// (private set; get;) = 0;
    public int PoisonAffinity {private set; get;} = 0;
    public int AcidAffinity {private set; get;} = 0;
    public int RadiationAffinity {private set; get;} = 0;
    public int BacteriaAffinity {private set; get;} = 0;
    public int VirusAffinity {private set; get;} = 0;

    public List<int> affinityList;
    public Dictionary<string, int> AffinityDict;

    public void Awake()
    {
        AffinityDict = GetAffinityDict();
    }


    #endregion

    #region // Physical Affinityences 
    public int BludgeoningResist {private set; get;} = 0;
    public int SlashingResist {private set; get;} = 0;
    public int PiercingResist {private set; get;} = 0;

    #endregion

    #region // resource regen
    [SerializeField] public int HealthRegen {private set; get;}= 1;
    [SerializeField] public int ManaRegen {private set; get;}= 1;
    [SerializeField] public int StaminaRegen  {private set; get;}= 1;

    #endregion


    #region // "Level Stats"

    [SerializeField] public int characterLevel  {private set; get;}= 1;
    [SerializeField] public int availableStatPoints {private set; get;}= 40;
    [SerializeField] public int currentXp  {private set; get;}= 0;
    [SerializeField] public int MaxXp {private set; get;} = 100;


    #endregion

    #region // inventory
    [SerializeField] public List<Rewards> rewards{private set; get;} = new List<Rewards>{ Rewards.Gold, Rewards.Xp};
    [SerializeField] public int characterGold {private set; get;} = 0;
    [SerializeField] public List<Ability_SO> knownAbilities {private set; get;} = new List<Ability_SO>();

    #endregion
    
    #region // getters
    private bool IsAlive()
    {
        if (currentHealth <= 0) return false;
        else return true;
    }    
    public string GetKnownAbilitiesString()
    {
        string knownAbilitiesString = "";
        foreach( Ability_SO ability in knownAbilities)
        {
            knownAbilitiesString += ability.AbilityName + ", ";
        }
        return knownAbilitiesString;
    }
    public string GetCharInfo()
    {
        string knownAbilitiesString = GetKnownAbilitiesString();
        string charInfo = $"Character Name: {characterName} || Description: {description} || Char Type: {charType} || Difficulty: {difficulty} Max Health: {MaxHealth} || Max Mana: {MaxMana} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Mana: {currentMana} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Mana Regen: {ManaRegen} || Stamina Regen: {StaminaRegen} || Character Level: {characterLevel} || Available Stat Points: {availableStatPoints} || Current XP: {currentXp} Max XP: {MaxXp} || Known Abilities: {string.Join(", ", knownAbilitiesString)}";
        return charInfo;
    }
    public string GetCharCreationStats()
    {
        string charInfo = $"Character Name: {characterName} || Description: {description} || Max Health: {MaxHealth} || Max Mana: {MaxMana} || Max Stamina: {MaxStamina} || Initiative: {initiative} || Current Health: {currentHealth} ||  Current Mana: {currentMana} || Current Stamina: {currentStamina} || Health Regen: {HealthRegen} || Mana Regen: {ManaRegen} || Stamina Regen: {StaminaRegen} || Action Points: {ActionPoints} || Action Point Regen Rate: {ActionPointRegen}";

        return charInfo;
    }
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
            { "Electricity Affinity", ElectrictyAffinity },
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

   

    public string GetAffinityString()
    {

        string affinityString = "";
        AffinityDict = GetAffinityDict();   
        
        foreach(KeyValuePair<string, int> kvp in AffinityDict)
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

        public void AddMaxHealth(int incrementValue, int cost)
    {
        MaxHealth += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddMaxMana(int incrementValue, int cost)
    {
        MaxMana += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddMaxStamina(int incrementValue, int cost)
    {
        MaxStamina += incrementValue;
        availableStatPoints -= cost;
    }

    public void AddActionPoint(int incrementValue, int cost)
    {
        ActionPoints += incrementValue;
        availableStatPoints -= cost;
    }

    public void AddActionPointRegen(int incrementValue, int cost)
    {
        ActionPointRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddHealthRegen(int incrementValue, int cost)
    {
        HealthRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddManaRegen(int incrementValue, int cost)
    {
        ManaRegen += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddStaminaRegen(int incrementValue, int cost)
    {
        StaminaRegen += incrementValue;
        availableStatPoints -= cost;
    }

    public void AddColdAffinity(int incrementValue, int cost)
    {
        ColdAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;

        availableStatPoints -= cost;
    }
    //public void AddIceAffinity(int incrementValue, int cost)
    //{
    //    IceAffinity += incrementValue;
//
    //    decimal splashIncrement = incrementValue/2;
    //    ColdAffinity += (int)Math.Round(splashIncrement, 2);
//
    //    decimal splashIncrement2 = incrementValue/2;
    //    WaterAffinity += (int)Math.Round(splashIncrement, 2);
//
    //    availableStatPoints -= cost;
    //}
    public void AddWaterAffinity(int incrementValue, int cost)
    {
        WaterAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        ColdAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddEarthAffinity(int incrementValue, int cost)
    {
        EarthAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        PlantAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddHeatAffinity(int incrementValue, int cost)
    {
        HeatAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        FireAffinity += (int)Math.Round(splashIncrement, 2);

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);



        availableStatPoints -= cost;
    }
    //public void AddLavaAffinity(int incrementValue, int cost)
    //{
    //    LavaAffinity += incrementValue;
//
    //    decimal splashIncrement = incrementValue/2;
    //    HeatAffinity += (int)Math.Round(splashIncrement, 2);
//
    //    EarthAffinity += (int)Math.Round(splashIncrement, 2);
//
    //    availableStatPoints -= cost;
    //}
    public void AddFireAffinity(int incrementValue, int cost)
    {
        FireAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        HeatAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddAirAffinity(int incrementValue, int cost)
    {
        AirAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        ElectrictyAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddElectricityAffinity(int incrementValue, int cost)
    {
        ElectrictyAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        AirAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddLightAffinity(int incrementValue, int cost)
    {
        LightAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);

        HeatAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }

    public void AddRadiationAffinity(int incrementValue, int cost)
    {
        RadiationAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;

        AirAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }

    public void AddPsychicAffinity(int incrementValue, int cost)
    {
        PsychicAffinity += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddFungiAffinity(int incrementValue, int cost)
    {
        FungiAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;

        PlantAffinity += (int)Math.Round(splashIncrement, 2);

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }
    public void AddPlantAffinity(int incrementValue, int cost)
    {
        PlantAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;

        FungiAffinity += (int)Math.Round(splashIncrement, 2);

        WaterAffinity += (int)Math.Round(splashIncrement, 2);
        
        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddPoisonAffinity(int incrementValue, int cost)
    {
        PoisonAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        
        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);

        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);


        availableStatPoints -= cost;
    }
    public void AddAcidAffinity(int incrementValue, int cost)
    {
        AcidAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        
        HeatAffinity += (int)Math.Round(splashIncrement, 2);

        RadiationAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    
    public void AddBacteriaAffinity(int incrementValue, int cost)
    {
        BacteriaAffinity += incrementValue;

        decimal splashIncrement = incrementValue/2;
        
        VirusAffinity += (int)Math.Round(splashIncrement, 2);
        
        PoisonAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddVirusAffinity(int incrementValue, int cost)
    {
        VirusAffinity += incrementValue;
        decimal splashIncrement = incrementValue/2;
        
        BacteriaAffinity += (int)Math.Round(splashIncrement, 2);
        
        PoisonAffinity += (int)Math.Round(splashIncrement, 2);

        availableStatPoints -= cost;
    }
    public void AddBludgeoningResist(int incrementValue, int cost)
    {
        BludgeoningResist += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddSlashingResist(int incrementValue, int cost)
    {
        SlashingResist += incrementValue;
        availableStatPoints -= cost;
    }
    public void AddPiercingResist(int incrementValue, int cost)
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
    }
    public void LearnAbility(Abilities newAbility)
    {
        knownAbilities.Add(abilityLibrary.abilityDict[newAbility]);
    }

    public void GainGold(int GoldAmount)
    {
        characterGold += GoldAmount;
    }

    private void GainLevel()
    {
        characterLevel ++;
        MaxXp = MaxXp * 2;
    }

    public void TakeDamage(int damageValue)
    {
        currentHealth = currentHealth - damageValue;

    }
    public void Heal(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
    }
    public void UpdateMana(int ManaChange)
    {
        currentHealth = currentHealth + ManaChange;
        if (currentMana > MaxMana) currentMana = MaxMana;
        else if (currentMana < 0 ) currentMana = 0;
    }
    
    private void GiveOverHealth(int overHealthAmount)
    {
        MaxHealth += overHealthAmount;
        currentHealth += overHealthAmount;
    }
    #endregion

    #region // make player and creatures;
    public GameObject MakePlayer()
    {
    characterName = "Borgauss";
    description = "World dominator currently in fetus-form";
    charType = Combatants.Player;
    difficulty = Difficulty.Easy;
    MaxHealth =10;
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
    ElectrictyAffinity = 0;
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
    MaxXp = 0;
    rewards = new List<Rewards>();
    characterGold = 0;
    knownAbilities = new List<Ability_SO>{
    abilityLibrary.Melee, abilityLibrary.FireBall, abilityLibrary.DivineStrike, abilityLibrary.HealingTouch, abilityLibrary.ColdLight, abilityLibrary.BrainDamage, abilityLibrary.ColdLight
    };

    return gameObject;

    }
    public GameObject MakeCreature(Difficulty difficultyLevel, Combatants combatantType)
    {
        Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel}");
        
        switch (combatantType)
        {
            case(Combatants.Enemy):
            charType = Combatants.Enemy;
            characterName = "Enemy";
            break;
            case(Combatants.Companion):
            charType = Combatants.Companion;
            characterName = "Companion";
            break;
            case(Combatants.Summon):
            charType = Combatants.Summon;
            characterName = "Summon";
            break;
        }
        

        switch(difficultyLevel)
        {
            case Difficulty.Easy:

            difficulty = Difficulty.Easy;
            Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel} into difficult {difficulty}");

            MaxHealth = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            MaxMana = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            MaxStamina = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            initiative = easyScaleFactor;
            HealthRegen = easyScaleFactor;
            ManaRegen = easyScaleFactor;
            StaminaRegen = easyScaleFactor;
            characterLevel = easyScaleFactor;
            knownAbilities = abilityLibrary.GetAbilities(easyScaleFactor);
            Debug.Log($"{knownAbilities}");

            
            
            break;

            case Difficulty.Medium:
            difficulty = Difficulty.Medium;
            MaxHealth = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            MaxMana = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            MaxStamina = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            initiative = mediumScaleFactor;
            HealthRegen = mediumScaleFactor;
            ManaRegen = mediumScaleFactor;
            StaminaRegen = mediumScaleFactor;
            characterLevel = mediumScaleFactor;
            knownAbilities = abilityLibrary.GetAbilities(mediumScaleFactor);
            
            break;
            case Difficulty.Hard:
            difficulty = Difficulty.Hard;
            MaxHealth = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            MaxMana = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            MaxStamina = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            initiative = hardScaleFactor;
            HealthRegen = hardScaleFactor;
            ManaRegen = hardScaleFactor;
            StaminaRegen = hardScaleFactor;
            characterLevel = hardScaleFactor;
            knownAbilities = abilityLibrary.GetAbilities(hardScaleFactor);
            Debug.Log($"{knownAbilities.ToString()}");
            break;

            case Difficulty.Brutal:
            difficulty = Difficulty.Brutal;
            MaxHealth = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            MaxMana = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            MaxStamina = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            initiative = brutalScaleFactor;
            HealthRegen = brutalScaleFactor;
            ManaRegen = brutalScaleFactor;
            StaminaRegen = brutalScaleFactor;
            characterLevel = brutalScaleFactor;
            knownAbilities = abilityLibrary.GetAbilities(brutalScaleFactor);
            break;

            case Difficulty.Nightmare:
            difficulty = Difficulty.Nightmare;
            Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel} into difficulty {difficulty}");

            MaxHealth = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            MaxMana = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            MaxStamina = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            initiative = nightmareScaleFactor;
            HealthRegen = nightmareScaleFactor;
            ManaRegen = nightmareScaleFactor;
            StaminaRegen = nightmareScaleFactor;
            characterLevel = nightmareScaleFactor;
            knownAbilities = abilityLibrary.GetAbilities(nightmareScaleFactor);
            break;
            
        }
            currentHealth = MaxHealth;
            currentStamina = MaxStamina;
            currentMana = MaxMana;
        Debug.Log($"Current Health at time of creation = {currentHealth}");
        return gameObject;
        }
    #endregion

}
