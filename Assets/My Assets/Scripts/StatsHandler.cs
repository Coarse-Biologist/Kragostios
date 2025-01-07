using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Accessibility;
using KragostiosAllEnums;
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
    [SerializeField] public int manaregen {private set; get;}= 1;
    [SerializeField] public int staminaRegen  {private set; get;}= 1;

    [Header("Level Stats")]

    [SerializeField] public int characterLevel  {private set; get;}= 1;
    [SerializeField] public int availableStatPoints {private set; get;}= 0;
    [SerializeField] public int currentXp  {private set; get;}= 0;
    [SerializeField] public int maxXp {private set; get;} = 100;

    [SerializeField] public List<AbilityScrollStorage.Abilities> knownAbilities {private set; get;}


    [Header("Rewards")]
    [SerializeField] public List<Rewards> rewards{private set; get;}

    [Header("Inventory")]
    [SerializeField] public int characterGold {private set; get;} = 0;
    private void Awake()
    {
        scrollStorage = GetComponent<AbilityScrollStorage>();
    }

    
    private void UpdateHealth(int healthChange)
    {
        
        currentHealth = currentHealth + healthChange;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
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


    public GameObject MakeCreature(Difficulty difficulty, Combatants combatantType)
    {
        
        StatsHandler stats = GetComponent<StatsHandler>();
        //Abilities abilities = GetComponent<Abilities>();
        //Abilities.AbilityScrollStorage scrolls = GetComponent<Abilities.AbilityScrollStorage>(); 
        List<AbilityScrollStorage.Abilities> creatureAbilities = new List<AbilityScrollStorage.Abilities>();
        switch (combatantType)
        {
            case (Combatants.Player):
            charType = Combatants.Player;
            creatureAbilities = scrollStorage.GetWeakAbilities();
            stats.knownAbilities = creatureAbilities;
            return gameObject;

            case(Combatants.Enemy):
            charType = Combatants.Enemy;
            break;
            case(Combatants.Companion):
            charType = Combatants.Companion;
            break;
            case(Combatants.Summon):
            charType = Combatants.Summon;
            break;
        }
        

        switch(difficulty)
        {
            case Difficulty.Easy:
            difficulty = Difficulty.Easy;
            maxHealth = Random.Range(1, 10) * easyScaleFactor;
            maxMana = Random.Range(1, 10) * easyScaleFactor;
            maxStamina = Random.Range(1, 10) * easyScaleFactor;
            initiative = easyScaleFactor;
            healthRegen = easyScaleFactor;
            manaregen = easyScaleFactor;
            staminaRegen = easyScaleFactor;
            characterLevel = easyScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            knownAbilities = creatureAbilities;
            break;

            case Difficulty.Medium:
            stats.difficulty = Difficulty.Medium;
            stats.maxHealth = Random.Range(1, 10) * mediumScaleFactor;
            stats.maxMana = Random.Range(1, 10) * mediumScaleFactor;
            stats.maxStamina = Random.Range(1, 10) * mediumScaleFactor;
            stats.initiative = mediumScaleFactor;
            stats.healthRegen = mediumScaleFactor;
            stats.manaregen = mediumScaleFactor;
            stats.staminaRegen = mediumScaleFactor;
            stats.characterLevel = mediumScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            stats.knownAbilities = creatureAbilities;
            break;
            case Difficulty.Hard:
            stats.difficulty = Difficulty.Hard;
            stats.maxHealth = Random.Range(1, 10) * hardScaleFactor;
            stats.maxMana = Random.Range(1, 10) * hardScaleFactor;
            stats.maxStamina = Random.Range(1, 10) * hardScaleFactor;
            stats.initiative = hardScaleFactor;
            stats.healthRegen = hardScaleFactor;
            stats.manaregen = hardScaleFactor;
            stats.staminaRegen = hardScaleFactor;
            stats.characterLevel = hardScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            stats.knownAbilities = creatureAbilities;
            break;
            case Difficulty.Brutal:
            stats.difficulty = Difficulty.Brutal;
            stats.maxHealth = Random.Range(1, 10) * brutalScaleFactor;
            stats.maxMana = Random.Range(1, 10) * brutalScaleFactor;
            stats.maxStamina = Random.Range(1, 10) * brutalScaleFactor;
            stats.initiative = brutalScaleFactor;
            stats.healthRegen = brutalScaleFactor;
            stats.manaregen = brutalScaleFactor;
            stats.staminaRegen = brutalScaleFactor;
            stats.characterLevel = brutalScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            stats.knownAbilities = creatureAbilities;
            break;
            case Difficulty.Nightmare:
            stats.difficulty = Difficulty.Nightmare;
            stats.maxHealth = Random.Range(1, 10) * nightmareScaleFactor;
            stats.maxMana = Random.Range(1, 10) * nightmareScaleFactor;
            stats.maxStamina = Random.Range(1, 10) * nightmareScaleFactor;
            stats.initiative = nightmareScaleFactor;
            stats.healthRegen = nightmareScaleFactor;
            stats.manaregen = nightmareScaleFactor;
            stats.staminaRegen = nightmareScaleFactor;
            stats.characterLevel = nightmareScaleFactor;
            //creatureAbilities = abilities.GetWeakAbilities();
            stats.knownAbilities = creatureAbilities;
            break;
            
        }
        return gameObject;
        }
    }



