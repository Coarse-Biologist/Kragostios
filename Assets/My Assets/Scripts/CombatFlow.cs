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
    private IEnumerable<KeyValuePair<GameObject,int>> sortedturnOrder;

    public UnityEvent<string> NarrationRequest;

    public UnityEvent<List<AbilityScrollStorage.Abilities>> AbilityButtonRequest;
    public UnityEvent<List<GameObject>> TargetButtonRequest;
    public UnityEvent ContinueButtonRequest; 

    private bool awaitingTargetSelection;
    private bool awaitingAbilitySelection;
    private int targetsExpected;
    private List<GameObject> selectedTargets = new List<GameObject>();
    private AbilityScrollStorage.Abilities selectedAbility;
    private int currentTurnIndex = 0;
    private bool narratingTurn;
    
   //private void Awake()
   //{
   // RequestNarration("Squeal");
   //}

    public void CombatCycle()
    {
            //sortedturnOrder = DecideTurnOrder();

            var list = sortedturnOrder.ToList();

            if (currentTurnIndex > list.Count - 1) currentTurnIndex = 0;

            GameObject combatant = list[currentTurnIndex].Key;

            Debug.Log($"{list} = list of caombatants");

            StatsHandler stats = combatant.GetComponent<StatsHandler>();

            if (stats.currentHealth > 0)
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
            else HandleDeath(combatant);
            currentTurnIndex ++;
    }

    private void ExecutePlayerTurn (GameObject combatant)
    {
        Debug.Log("Player turn executing method called");
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string playerTurnIntro = $"{stats.characterName} Has big plans!... What are they?...";
        RequestNarration(playerTurnIntro);
        RequestAbilityButtons(stats.knownAbilities);
    }
    private void ExecuteEnemyTurn (GameObject combatant)
    {
        Debug.Log("Enemy turn executed");
        string EnemyTurnNarration = $"{combatant.name} is finished with their pointless turn.";
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


    private void HandleAbilityEffect(List<GameObject> targets, AbilityScrollStorage.Abilities selectedAbility)
    {
        RequestNarration($"{selectedAbility.AbilityName} ability effects have been handled");
        RequestContinueButton();
    }
    public void NextTurn()
    {
        turnNumber ++;
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
    private void RequestAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
    {
        Debug.Log("Ability Button request sent?");

        AbilityButtonRequest?.Invoke(abilities);
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
        
        sortedturnOrder = uncookedList.OrderByDescending(kvp => kvp.Value);
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
        Debug.Log($"{deadCombatant} will be removed from combat list");
        combatants.Remove(deadCombatant);   
        var list = sortedturnOrder.ToList();                // converts IEnumerator to list
        list.RemoveAll(kvp => kvp.Key == deadCombatant);    // removes combatant from list
        sortedturnOrder = list;                             // sets the list to be the new value of the sortedTurnOrder ienumerator
    }

    private void GivePlayerCompanionsInventory(GameObject companion)
    {

    }




}
