using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using UnityEngine.Events;
using System.Net.NetworkInformation;

public class CombatFlow : MonoBehaviour
{
    private int turnNumber;
    public List<GameObject> combatants {private set; get;}
    private Dictionary<GameObject, int> uncookedList = new Dictionary<GameObject, int>();
    private List<KeyValuePair<GameObject,int>> sortedturnOrder;

    public UnityEvent<string> NarrationRequest;

    public UnityEvent<List<AbilityScrollStorage.Abilities>> OptionButtonRequest;
    public UnityEvent<List<GameObject>> TargetButtonRequest;
    public UnityEvent ContinueButtonRequest; 

    private bool awaitingTargetSelection;
    private bool awaitingAbilitySelection;
    private int targetsExpected;
    private List<GameObject> selectedTargets = new List<GameObject>();
    private AbilityScrollStorage.Abilities selectedAbility;
    private int currentTurnIndex = 0;
    public GameObject caster {private set; get;}
    
  

    public void CombatCycle()
    {
            //sortedturnOrder = DecideTurnOrder();

            //var list = sortedturnOrder.ToList();

            if (currentTurnIndex > sortedturnOrder.Count - 1) currentTurnIndex = 0;

            GameObject combatant = sortedturnOrder[currentTurnIndex].Key;
            caster = combatant;
            Debug.Log($"{sortedturnOrder} = list of caombatants");

            StatsHandler stats = combatant.GetComponent<StatsHandler>();

            if (stats.currentHealth > 0) // hydn did this
            {
                Debug.Log($"{stats.characterName}, {stats.charType} started his turn");
                switch (stats.charType)
                {
                    case(Combatants.Player):
                    ExecutePlayerTurn(combatant);
                    break;
                    
                    case(Combatants.Enemy):
                    ExecuteEnemyTurn(combatant);
                    break;

                    case(Combatants.Summon):
                    ExecuteSummonTurn(combatant);
                    break;

                    case(Combatants.Companion):
                    ExecuteCompanionTurn(combatant);
                    break;


                    default:
                    {
                    Debug.Log($"{combatant} was none of the above character types");
                    break;
                    }
                    
                }
            }
            else{
            Debug.Log($"{combatant} has less than or 0 health");
            HandleDeath(combatant);
            }
           
    }

    private void ExecutePlayerTurn (GameObject combatant)
    {
        Debug.Log("Player turn executing method called");
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string playerTurnIntro = $"{stats.characterName} Has big plans!... What are they?...";
        RequestNarration(playerTurnIntro);
        RequestOptionButtons(stats.knownAbilities);
    }
    private void ExecuteEnemyTurn (GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        int numberKnownAbilities = stats.knownAbilities.Count();
        Debug.Log($"{numberKnownAbilities} = number of known abilities");
        int randomSkillIndex = Random.Range(0, numberKnownAbilities - 1);
        Debug.Log($"{randomSkillIndex} = ability selected");

        AbilityScrollStorage.Abilities selectedAbility = stats.knownAbilities[randomSkillIndex];
        
        List<GameObject> targets = new List<GameObject>();

        switch (selectedAbility.Type)
        {
            
            case AbilityCategories.Heal:
            targets = SelectRandomCharofType(Combatants.Enemy);
            break;

            case AbilityCategories.Buff:
            targets = SelectRandomCharofType(Combatants.Enemy);
            break;

            case AbilityCategories.BuffHeal:
            targets = SelectRandomCharofType(Combatants.Enemy);
            break;

            case AbilityCategories.Debuff:
            targets = SelectRandomCharofType(Combatants.Enemy);
            break;

            case AbilityCategories.Attack:
            targets = SelectRandomCharofType(Combatants.Enemy, true);
            break;

            case AbilityCategories.DebuffAttack:
            targets = SelectRandomCharofType(Combatants.Enemy, true);
            break;

            default:
            break;
        }
            
        HandleAbilityEffect(targets, selectedAbility);
        string EnemyTurnNarration = $"{combatant.name} used {selectedAbility.AbilityName} on {targets}for their turn.";
        RequestNarration(EnemyTurnNarration);
        RequestContinueButton();
    }
    private void ExecuteSummonTurn (GameObject combatant)
    {
        Debug.Log("Summon turn executing method called");
    }
    private void ExecuteCompanionTurn (GameObject combatant)
    {
        Debug.Log("Companion turn executing method called");

    }

    private List<GameObject> SelectRandomCharofType(Combatants charType, bool allAllies = false) //if true, will make a list of all combatants that are not of type chartype
    {
    List<GameObject> possibleTargets = new List<GameObject>();
    foreach (GameObject combatant in combatants) 
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        if (allAllies) 
        {
            if (stats.charType == Combatants.Player || 
                stats.charType == Combatants.Companion || 
                stats.charType == Combatants.Summon)
                {
                    possibleTargets.Add(combatant);
                }
        }
        else if (stats.charType == charType)
        {
            possibleTargets.Add(combatant);
        }
    }
        List<GameObject> selectedTargets = new List<GameObject>();

        Debug.Log($"targets = {selectedAbility.Targets}");
        int targets = selectedAbility.Targets;
        int numberOfEnemies = possibleTargets.Count();
        int targetsSelected = 0;

        while (targetsSelected < targets)
        {
            int randomTargetIndex = Random.Range(0, numberOfEnemies - 1);
            GameObject randomTarget = possibleTargets[randomTargetIndex];
            selectedTargets.Add(randomTarget);
            targetsSelected ++;
        }
       
        return selectedTargets;

    }
    private void HandleAbilityEffect(List<GameObject> targets, AbilityScrollStorage.Abilities selectedAbility)
    {   
        foreach(GameObject target in targets)
        {
            
            StatsHandler stats = target.GetComponent<StatsHandler>();
            stats.TakeDamage(selectedAbility.DamageValue);
            StatsHandler casterStats = caster.GetComponent<StatsHandler>();
            casterStats.UpdateMana(selectedAbility.AbilityCost);

            string narrationText = $"{stats.characterName} recieved {selectedAbility.DamageValue} damage from {selectedAbility.AbilityName}"; 
            RequestNarration(narrationText);
            if (stats.currentHealth <= 0) // From Hydn!!!
            {
                Debug.Log($"{target} died");
                HandleDeath(target);
            }
            else Debug.Log($"{stats.currentHealth} = targets current health");
        }

        RequestNarration($"{selectedAbility.AbilityName} ability effects have been handled");
        RequestContinueButton();
    }
    public void NextTurn()
    {
        selectedTargets = new List<GameObject>();
        currentTurnIndex ++;
        Invoke("CombatCycle", 5f);
    }

// Event functions
    private void RequestNarration(string message)
    {
        Debug.Log("Narration request sent?");
        NarrationRequest?.Invoke(message);
    }

    private void RequestContinueButton()
    {
        ContinueButtonRequest?.Invoke();
    }
    private void RequestOptionButtons(List<AbilityScrollStorage.Abilities> abilities)
    {
        Debug.Log("Ability Button request sent?");

        OptionButtonRequest?.Invoke(abilities);
    }

// Setup functions
    public void SetExpectedTargets(int targetNum)
    {
        targetsExpected = targetNum;
    }

    public void SetCombatants(List<GameObject> assignedCombatants)
    {
        combatants = assignedCombatants;
    }
    public void SetSelectedAbility(AbilityScrollStorage.Abilities ability)
    {
        selectedAbility = ability;
    }

    public void AddSelectedTarget(GameObject target)
    {
        selectedTargets.Add(target);
        if (selectedTargets.Count == targetsExpected)
        {
           HandleAbilityEffect(selectedTargets, selectedAbility);
        }
    }

    public IEnumerable<KeyValuePair<GameObject,int>> DecideTurnOrder()
    {
        foreach (GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            int initiativeRoll = Random.Range(0, 20) + stats.initiative;
            //Debug.Log($"{uncookedList} = uncooked list");
            uncookedList.Add(combatant, initiativeRoll);
        }
        
        sortedturnOrder = uncookedList.OrderByDescending(kvp => kvp.Value).ToList();
        return sortedturnOrder;
    }

// Death functions

    private void HandleDeath(GameObject deadCombatant)
    {
        Debug.Log($"{deadCombatant} will be removed from combat list, glaub");

        StatsHandler stats = deadCombatant.GetComponent<StatsHandler>();
        if (stats.charType == Combatants.Enemy)
        {
            CalculateXpandGold(deadCombatant);
        }
        if (stats.charType == Combatants.Companion)
        {
            GivePlayerCompanionsInventory(deadCombatant);
        }
        RemoveFromCombat(deadCombatant);
    }

    private void CalculateXpandGold(GameObject deadCombatant)
    {
        //calculate gold and xp
        StatsHandler stats = deadCombatant.GetComponent<StatsHandler>();
        Dictionary<Rewards, int> rewardDict = new Dictionary<Rewards, int>();
        foreach(Rewards reward in stats.rewards) // iterate through rewards in each deadCombatants reward list
        {   switch(reward)
            {
                case (Rewards.Gold):
                int goldGained = (int)stats.difficulty * Random.Range(1, 10);   //multiply difficulty level x random value
                rewardDict.TryAdd(Rewards.Gold, goldGained);
                break;

                case(Rewards.Xp):
                int XpGained = (int)stats.difficulty * Random.Range(1, 10);    //multiply difficulty level x random value
                rewardDict.TryAdd(Rewards.Xp, XpGained);
                break;
            }
            
        }
        //reward gold and Xp
        foreach (var kvp in sortedturnOrder)
        {
            GameObject combatant = kvp.Key;
            StatsHandler victorStats = combatant.GetComponent<StatsHandler>();
            if(victorStats.charType != Combatants.Enemy)
            {
                int goldGained = rewardDict[Rewards.Gold];
                int XpGained = rewardDict[Rewards.Xp];
                victorStats.GainGold(goldGained);
                victorStats.GainXp(XpGained);
            }
        }   
    }

    private void RemoveFromCombat(GameObject deadCombatant)
    {
        Debug.Log($"{deadCombatant.GetComponent<StatsHandler>().characterName} will be removed from combat list. HP: {deadCombatant.GetComponent<StatsHandler>().currentHealth}");
        combatants.Remove(deadCombatant);
        Debug.Log($"{combatants} = combatants list");
        //var list = sortedturnOrder.ToList();                // converts IEnumerator to list
        sortedturnOrder.RemoveAll(kvp => kvp.Key == deadCombatant);    // removes combatant from list  
        //sortedturnOrder = list.AsEnumerable();     
        Debug.Log($"{sortedturnOrder} = sorted turnOrder.");
        // sets the list to be the new value of the sortedTurnOrder ienumerator
    }

    private void GivePlayerCompanionsInventory(GameObject companion)
    {

    }

    public bool CheckEnemiesRemaining() // true if any enemy remains 
    {
        bool enemiesRemaining = false;
        foreach(GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            if (stats.charType == Combatants.Enemy)
            {
                enemiesRemaining = true;
                return enemiesRemaining;
            }
            else enemiesRemaining = false;

        }
        return enemiesRemaining;
    }


}
