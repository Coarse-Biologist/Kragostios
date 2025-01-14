using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Accessibility;
using KragostiosAllEnums;
using System;
//using UnityEngine.UI;
//using UnityEditor.SceneManagement;
//using UnityEditor.Rendering;


public class StatsHandler : MonoBehaviour
{

    #region // class references
    private AbilityScrollStorage scrollStorage;
    private AbilityScrollStorage.Abilities abilities;
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

    #region // Elemental resistences
    public int ColdResist {private set; get;} = 0;
    public int IceResist {private set; get;} = 0;
    public int WaterResist {private set; get;} = 0;
    public int EarthResist {private set; get;} = 0;
    public int HeatResist {private set; get;} = 0;
    public int LavaResist {private set; get;} = 0;
    public int FireResist {private set; get;} = 0;
    public int AirResist {private set; get;} = 0;
    public int ElectrictyResist {private set; get;} = 0;
    public int LightResist {private set; get;} = 0;
    public int PsychicResist {private set; get;} = 0;
    public int FungiResist {private set; get;} = 0;
    public int PoisonResist {private set; get;} = 0;
    public int AcidResist {private set; get;} = 0;
    public int RadiationResist {private set; get;} = 0;
    public int BacteriaResist {private set; get;} = 0;
    public int VirusResist {private set; get;} = 0;


    #endregion

    #region // Physical resistences 
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
    [SerializeField] public int availableStatPoints {private set; get;}= 0;
    [SerializeField] public int currentXp  {private set; get;}= 0;
    [SerializeField] public int MaxXp {private set; get;} = 100;


    #endregion

    #region // inventory
    [SerializeField] public List<Rewards> rewards{private set; get;} = new List<Rewards>{ Rewards.Gold, Rewards.Xp};
    [SerializeField] public int characterGold {private set; get;} = 0;
    [SerializeField] public List<AbilityScrollStorage.Abilities> knownAbilities {private set; get;}

    #endregion
    private void Awake()
    {
        scrollStorage = GetComponent<AbilityScrollStorage>();
    }
    #region // getters
    private bool IsAlive()
    {
        if (currentHealth <= 0) return false;
        else return true;
    }    
    private string GetKnownAbilitiesString()
    {
        string knownAbilitiesString = "";
        foreach( AbilityScrollStorage.Abilities ability in knownAbilities)
        {
            knownAbilitiesString += ability.AbilityName + ", ";
        }
        return knownAbilitiesString;
    }
    public string GetCharInfo()
    {
        string knownAbilitiesString = GetKnownAbilitiesString();
        string charInfo = $"Character Name: {characterName}, Description: {description}, Char Type: {charType}, Difficulty: {difficulty}, " +
        $"Max Health: {MaxHealth}, Max Mana: {MaxMana}, Max Stamina: {MaxStamina}, Initiative: {initiative}, " +
        $"Current Health: {currentHealth}, Current Mana: {currentMana}, Current Stamina: {currentStamina}, " +
        $"Health Regen: {HealthRegen}, Mana Regen: {ManaRegen}, Stamina Regen: {StaminaRegen}, " +
        $"Character Level: {characterLevel}, Available Stat Points: {availableStatPoints}, Current XP: {currentXp}, Max XP: {MaxXp}, " +
        $"Known Abilities: {string.Join(", ", knownAbilitiesString)}";
        return charInfo;

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

    public void AddMaxHealth(int incrementValue) => MaxHealth += incrementValue;
    public void AddMaxMana(int incrementValue) => MaxMana += incrementValue;
    public void AddMaxStamina(int incrementValue) => MaxStamina += incrementValue;
    public void AddActionPoint(int incrementValue) => MaxStamina += incrementValue;
    public void AddActionPointRegen(int incrementValue) => MaxStamina += incrementValue;

    public void AddHealthRegen(int incrementValue) => HealthRegen += incrementValue;
    public void AddManaRegen(int incrementValue) => ManaRegen += incrementValue;
    public void AddStaminaRegen(int incrementValue) => StaminaRegen += incrementValue;
    public void AddColdResist(int incrementValue) => ColdResist += incrementValue;
    public void AddIceResist(int incrementValue) => IceResist += incrementValue;
    public void AddWaterResist(int incrementValue) => WaterResist += incrementValue;
    public void AddEarthResist(int incrementValue) => EarthResist += incrementValue;
    public void AddHeatResist(int incrementValue) => HeatResist += incrementValue;
    public void AddLavaResist(int incrementValue) => LavaResist += incrementValue;
    public void AddFireResist(int incrementValue) => FireResist += incrementValue;
    public void AddAirResist(int incrementValue) => AirResist += incrementValue;
    public void AddElectricityResist(int incrementValue) => ElectrictyResist += incrementValue;
    public void AddLightResist(int incrementValue) => LightResist += incrementValue;
    public void AddPsychicResist(int incrementValue) => PsychicResist += incrementValue;
    public void AddFungiResist(int incrementValue) => FungiResist += incrementValue;
    public void AddPoisonResist(int incrementValue) => PoisonResist += incrementValue;
    public void AddAcidResist(int incrementValue) => AcidResist += incrementValue;
    public void AddRadiationResist(int incrementValue) => RadiationResist += incrementValue;
    public void AddBacteriaResist(int incrementValue) => BacteriaResist += incrementValue;
    public void AddVirusResist(int incrementValue) => BacteriaResist += incrementValue;
    public void AddBludgeoningResist(int incrementValue) => BludgeoningResist += incrementValue;
    public void AddSlashingResist(int incrementValue) => SlashingResist += incrementValue;
    public void AddPiercingResist(int incrementValue) => PiercingResist += incrementValue;

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
    public void LearnAbility(AbilityScrollStorage.Abilities newAbility)
    {
        knownAbilities.Add(newAbility);
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

    public GameObject MakeCreature(Difficulty difficultyLevel, Combatants combatantType)
    {
        Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel}");
        //StatsHandler stats = GetComponent<StatsHandler>();
        //Abilities abilities = GetComponent<Abilities>();
        //Abilities.AbilityScrollStorage scrolls = GetComponent<Abilities.AbilityScrollStorage>(); 
        List<AbilityScrollStorage.Abilities> creatureAbilities = new List<AbilityScrollStorage.Abilities>();
        creatureAbilities = scrollStorage.GetWeakAbilities();
        switch (combatantType)
        {
            case (Combatants.Player):
            charType = Combatants.Player;
            characterName = "Player";
            creatureAbilities = scrollStorage.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            //return gameObject;
            break;
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
            knownAbilities = creatureAbilities;
            
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
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
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
            knownAbilities = creatureAbilities;
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
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
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
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            break;
            
        }
            currentHealth = MaxHealth;
            currentStamina = MaxStamina;
            currentMana = MaxMana;
        Debug.Log($"Current Health at time of creation = {currentHealth}");
        return gameObject;
        }


}
