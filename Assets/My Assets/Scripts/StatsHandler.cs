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
    private AbilityScrollStorage scrollStorage;
    private AbilityScrollStorage.Abilities abilities;
    [SerializeField] public string characterName {private set; get;} = "Sqreegler";
    [SerializeField] public string description {private set; get;}
    [SerializeField] public Combatants charType{private set; get;}

    [SerializeField] public Difficulty difficulty{private set; get;}
    [Header("Max Stats")]

    [SerializeField] public int maxHealth {private set; get;} = 100;
    [SerializeField] public int maxMana {private set; get;} = 100;
    [SerializeField] public int maxStamina {private set; get;} = 100;
    [SerializeField] public int initiative {private set; get;} = 100;

    [Header("Current Stats")]

    [SerializeField] public int currentHealth {private set; get;}= 100;
    [SerializeField] public int currentMana {private set; get;}= 100;
    [SerializeField] public int currentStamina  {private set; get;}= 100;

    [Header("Regeneration")]
    [SerializeField] public int healthRegen {private set; get;}= 1;
    [SerializeField] public int manaRegen {private set; get;}= 1;
    [SerializeField] public int staminaRegen  {private set; get;}= 1;

    [Header("Level Stats")]

    [SerializeField] public int characterLevel  {private set; get;}= 1;
    [SerializeField] public int availableStatPoints {private set; get;}= 0;
    [SerializeField] public int currentXp  {private set; get;}= 0;
    [SerializeField] public int maxXp {private set; get;} = 100;

    [SerializeField] public List<AbilityScrollStorage.Abilities> knownAbilities {private set; get;}


    [Header("Rewards")]
    [SerializeField] public List<Rewards> rewards{private set; get;} = new List<Rewards>{ Rewards.Gold, Rewards.Xp};

    [Header("Inventory")]
    [SerializeField] public int characterGold {private set; get;} = 0;
    private void Awake()
    {
        scrollStorage = GetComponent<AbilityScrollStorage>();
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

    private bool IsAlive()
    {
        if (currentHealth <= 0) return false;
        else return true;
    }    

    /////// XP RELATED
    
    public void GainXp(int XpGain)
    {
        currentXp = currentXp + XpGain;
        if (currentXp > maxXp)
        {
            currentXp -= maxXp;
            GainLevel();
        } 
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
    
        
        private int easyScaleFactor = 2;
        private int mediumScaleFactor = 3;
        private int hardScaleFactor = 4;
        private int brutalScaleFactor = 5;
        private int nightmareScaleFactor = 6;


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

        public void LearnAbility(AbilityScrollStorage.Abilities newAbility)
        {
            knownAbilities.Add(newAbility);
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

    // Setters

    public void SetName(string name)
    {
        characterName = name;
    }
    public void SetDescription(string newDesciption)
    {
        description = newDesciption;
    }
    }



