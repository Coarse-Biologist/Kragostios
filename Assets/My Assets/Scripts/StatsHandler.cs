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

    [SerializeField] public int maxHealth {private set; get;} = 100;
    [SerializeField] public int maxMana {private set; get;} = 100;
    [SerializeField] public int maxStamina {private set; get;} = 100;
    [SerializeField] public int initiative {private set; get;} = 100;

    [Header("Current Stats")]

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
    public int AirResist {private set; get;} = 0;
    public int ElectrictyResist {private set; get;} = 0;
    public int LightResist {private set; get;} = 0;
    public int PsychicResist {private set; get;} = 0;
    public int FungiResist {private set; get;} = 0;
    public int PoisonResist {private set; get;} = 0;
    public int AcidResist {private set; get;} = 0;
    public int RadiationResist {private set; get;} = 0;
    public int BacteriaResist {private set; get;} = 0;

    #endregion

    #region // Physical resistences 
    public int BludgeoningResist {private set; get;} = 0;
    public int SlashingResist {private set; get;} = 0;
    public int PiercingResist {private set; get;} = 0;

    #endregion

    #region // resourcen regen
    [SerializeField] public int healthRegen {private set; get;}= 1;
    [SerializeField] public int manaRegen {private set; get;}= 1;
    [SerializeField] public int staminaRegen  {private set; get;}= 1;

    #endregion

    #region // Elemental resistences

    #endregion



    #region // "Level Stats"

    [SerializeField] public int characterLevel  {private set; get;}= 1;
    [SerializeField] public int availableStatPoints {private set; get;}= 0;
    [SerializeField] public int currentXp  {private set; get;}= 0;
    [SerializeField] public int maxXp {private set; get;} = 100;


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
        $"Max Health: {maxHealth}, Max Mana: {maxMana}, Max Stamina: {maxStamina}, Initiative: {initiative}, " +
        $"Current Health: {currentHealth}, Current Mana: {currentMana}, Current Stamina: {currentStamina}, " +
        $"Health Regen: {healthRegen}, Mana Regen: {manaRegen}, Stamina Regen: {staminaRegen}, " +
        $"Character Level: {characterLevel}, Available Stat Points: {availableStatPoints}, Current XP: {currentXp}, Max XP: {maxXp}, " +
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

    public void AddMaxHealth(int incrementValue) => maxHealth += incrementValue;
    public void AddMaxMana(int incrementValue) => maxMana += incrementValue;
    public void AddMaxStamina(int incrementValue) => maxStamina += incrementValue;
    public void AddCurrentHealth(int incrementValue) => currentHealth += incrementValue;
    public void AddCurrentMana(int incrementValue) => currentMana += incrementValue;
    public void AddCurrentStamina(int incrementValue) => currentStamina += incrementValue;

    public void AddHealthRegen(int incrementValue) => healthRegen += incrementValue;
    public void AddManaRegen(int incrementValue) => manaRegen += incrementValue;
    public void AddStaminaRegen(int incrementValue) => staminaRegen += incrementValue;
    public void AddColdResist(int incrementValue) => coldResist += incrementValue;
    public void AddIceResist(int incrementValue) => iceResist += incrementValue;
    public void AddWaterResist(int incrementValue) => waterResist += incrementValue;
    public void AddEarthResist(int incrementValue) => earthResist += incrementValue;
    public void AddHeatResist(int incrementValue) => heatResist += incrementValue;
    public void AddAirResist(int incrementValue) => airResist += incrementValue;
    public void AddElectricityResist(int incrementValue) => electricityResist += incrementValue;
    public void AddLightResist(int incrementValue) => lightResist += incrementValue;
    public void AddPsychicResist(int incrementValue) => psychicResist += incrementValue;
    public void AddFungiResist(int incrementValue) => fungiResist += incrementValue;
    public void AddPoisonResist(int incrementValue) => poisonResist += incrementValue;
    public void AddAcidResist(int incrementValue) => acidResist += incrementValue;
    public void AddRadiationResist(int incrementValue) => radiationResist += incrementValue;
    public void AddBacteriaResist(int incrementValue) => bacteriaResist += incrementValue;
    public void AddBludgeoningResist(int incrementValue) => bludgeoningResist += incrementValue;
    public void AddSlashingResist(int incrementValue) => slashingResist += incrementValue;
    public void AddPiercingResist(int incrementValue) => piercingResist += incrementValue;

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
        if (currentXp > maxXp)
        {
            currentXp -= maxXp;
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
        maxXp = maxXp * 2;
    }

        public void TakeDamage(int damageValue)
    {
        currentHealth = currentHealth - damageValue;

    }
    public void Heal(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void UpdateMana(int manaChange)
    {
        currentHealth = currentHealth + manaChange;
        if (currentMana > maxMana) currentMana = maxMana;
        else if (currentMana < 0 ) currentMana = 0;
    }
    
    private void GiveOverHealth(int overhealthAmount)
    {
        maxHealth += overhealthAmount;
        currentHealth += overhealthAmount;
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

            maxHealth = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            maxMana = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            maxStamina = UnityEngine.Random.Range(1, 10) * easyScaleFactor;
            initiative = easyScaleFactor;
            healthRegen = easyScaleFactor;
            manaRegen = easyScaleFactor;
            staminaRegen = easyScaleFactor;
            characterLevel = easyScaleFactor;
            knownAbilities = creatureAbilities;
            
            break;

            case Difficulty.Medium:
            difficulty = Difficulty.Medium;
            maxHealth = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            maxMana = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            maxStamina = UnityEngine.Random.Range(1, 10) * mediumScaleFactor;
            initiative = mediumScaleFactor;
            healthRegen = mediumScaleFactor;
            manaRegen = mediumScaleFactor;
            staminaRegen = mediumScaleFactor;
            characterLevel = mediumScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            break;
            case Difficulty.Hard:
            difficulty = Difficulty.Hard;
            maxHealth = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            maxMana = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            maxStamina = UnityEngine.Random.Range(1, 10) * hardScaleFactor;
            initiative = hardScaleFactor;
            healthRegen = hardScaleFactor;
            manaRegen = hardScaleFactor;
            staminaRegen = hardScaleFactor;
            characterLevel = hardScaleFactor;
            knownAbilities = creatureAbilities;
            break;

            case Difficulty.Brutal:
            difficulty = Difficulty.Brutal;
            maxHealth = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            maxMana = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            maxStamina = UnityEngine.Random.Range(1, 10) * brutalScaleFactor;
            initiative = brutalScaleFactor;
            healthRegen = brutalScaleFactor;
            manaRegen = brutalScaleFactor;
            staminaRegen = brutalScaleFactor;
            characterLevel = brutalScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            break;

            case Difficulty.Nightmare:
            difficulty = Difficulty.Nightmare;
            Debug.Log($"making combatant type {combatantType} of difficulty {difficultyLevel} into difficulty {difficulty}");

            maxHealth = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            maxMana = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            maxStamina = UnityEngine.Random.Range(1, 10) * nightmareScaleFactor;
            initiative = nightmareScaleFactor;
            healthRegen = nightmareScaleFactor;
            manaRegen = nightmareScaleFactor;
            staminaRegen = nightmareScaleFactor;
            characterLevel = nightmareScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            break;
            
        }
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentMana = maxMana;
        Debug.Log($"Current health at time of creation = {currentHealth}");
        return gameObject;
        }


}
