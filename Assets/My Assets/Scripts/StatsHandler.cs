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

    #region // Elemental Affinityences
    public int ColdAffinity {private set; get;} = 0;
    public int IceAffinity {private set; get;} = 0;
    public int WaterAffinity {private set; get;} = 0;
    public int EarthAffinity {private set; get;} = 0;
    public int HeatAffinity {private set; get;} = 0;
    public int LavaAffinity {private set; get;} = 0;
    public int FireAffinity {private set; get;} = 0;
    public int AirAffinity {private set; get;} = 0;
    public int ElectrictyAffinity {private set; get;} = 0;
    public int LightAffinity {private set; get;} = 0;
    public int PsychicAffinity {private set; get;} = 0;
    public int FungiAffinity {private set; get;} = 0;
    public int PoisonAffinity {private set; get;} = 0;
    public int AcidAffinity {private set; get;} = 0;
    public int RadiationAffinity {private set; get;} = 0;
    public int BacteriaAffinity {private set; get;} = 0;
    public int VirusAffinity {private set; get;} = 0;


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
        if (knownAbilities.Count != 0)
        {
            foreach( AbilityScrollStorage.Abilities ability in knownAbilities)
        {
            knownAbilitiesString += ability.AbilityName + ", ";
        }
        }
        
        return knownAbilitiesString;
    }
    public string GetCharInfo()
    {
        string knownAbilitiesString = GetKnownAbilitiesString();
        string charInfo = $"Character Name: {characterName} \n Description: {description} \n Char Type: {charType} \n  Difficulty: {difficulty} \n Max Health: {MaxHealth} \n  Max Mana: {MaxMana} \n  Max Stamina: {MaxStamina} \n  Initiative: {initiative} \n  Current Health: {currentHealth} \n Current Mana: {currentMana} Current Stamina: {currentStamina}, Health Regen: {HealthRegen} \n  Mana Regen: {ManaRegen} \n  Stamina Regen: {StaminaRegen} \n  Character Level: {characterLevel} \n Available Stat Points: {availableStatPoints} \n  Current XP: {currentXp} \n Max XP: {MaxXp} \n  Known Abilities: {string.Join(", ", knownAbilitiesString)}";
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
    public void AddColdAffinity(int incrementValue) => ColdAffinity += incrementValue;
    public void AddIceAffinity(int incrementValue) => IceAffinity += incrementValue;
    public void AddWaterAffinity(int incrementValue) => WaterAffinity += incrementValue;
    public void AddEarthAffinity(int incrementValue) => EarthAffinity += incrementValue;
    public void AddHeatAffinity(int incrementValue) => HeatAffinity += incrementValue;
    public void AddLavaAffinity(int incrementValue) => LavaAffinity += incrementValue;
    public void AddFireAffinity(int incrementValue) => FireAffinity += incrementValue;
    public void AddAirAffinity(int incrementValue) => AirAffinity += incrementValue;
    public void AddElectricityAffinity(int incrementValue) => ElectrictyAffinity += incrementValue;
    public void AddLightAffinity(int incrementValue) => LightAffinity += incrementValue;
    public void AddPsychicAffinity(int incrementValue) => PsychicAffinity += incrementValue;
    public void AddFungiAffinity(int incrementValue) => FungiAffinity += incrementValue;
    public void AddPoisonAffinity(int incrementValue) => PoisonAffinity += incrementValue;
    public void AddAcidAffinity(int incrementValue) => AcidAffinity += incrementValue;
    public void AddRadiationAffinity(int incrementValue) => RadiationAffinity += incrementValue;
    public void AddBacteriaAffinity(int incrementValue) => BacteriaAffinity += incrementValue;
    public void AddVirusAffinity(int incrementValue) => BacteriaAffinity += incrementValue;
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
