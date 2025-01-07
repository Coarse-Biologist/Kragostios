using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using UnityEngine.Events;
using System.Net.NetworkInformation;

public class CombatFlow : MonoBehaviour
{
    private int turnNumber;
    public List<GameObject> combatants;
    private Dictionary<GameObject, int> uncookedList = new Dictionary<GameObject, int>();
    private IEnumerable<KeyValuePair<GameObject,int>> sortedturnOrder;

    public UnityEvent<string> NarrationRequest;

    public UnityEvent<List<AbilityScrollStorage.Abilities>> AbilityButtonRequest;
    public UnityEvent<List<GameObject>> TargetButtonRequest;

    private bool awaitingTargetSelection;
    private bool awaitingAbilitySelection;
    private int targetsExpected;

    

   

    private IEnumerable<KeyValuePair<GameObject,int>> DecideTurnOrder()
    {
        foreach (GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            int initiativeRoll = Random.Range(0, 20) + stats.initiative;
            Debug.Log($"{uncookedList} = uncooked list");
            uncookedList.Add(combatant, initiativeRoll);
        }
        
        sortedturnOrder = uncookedList.OrderByDescending(kvp => kvp.Value);
        return sortedturnOrder;
    }

    public void CombatCycle(List<GameObject> combatants)
    {
        sortedturnOrder = DecideTurnOrder();
        foreach (var kvp in sortedturnOrder)
        {
            GameObject combatant = kvp.Key;
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            if (stats.currentHealth > 0)
            {
                Debug.Log($"{combatant} completed his turn");
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
                    break;
                    }
                    

                }
            }
        }
    }

    private void ExecutePlayerTurn (GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string playerTurnIntro = $"{stats.characterName} Has big plans!... What are they?...";
        RequestNarration(playerTurnIntro);
        RequestAbilityButtons(stats.knownAbilities);

    }
    private void ExecuteEnemyTurn (GameObject combatant)
    {

    }
    private void ExecuteSummonTurn (GameObject combatant)
    {

    }
    private void ExecuteCompanionTurn (GameObject combatant)
    {

    }

    private void HandleDeath(GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        if (stats.charType == Combatants.Enemy)
        {
            CalculateXpandGold(combatant);
        }
        if (stats.charType == Combatants.Companion)
        {
            GivePlayerCompanionsInventory(combatant);
        }
        RemoveFromCombat(combatant);
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
        combatants.Remove(deadCombatant);   
        var list = sortedturnOrder.ToList();                // converts IEnumerator to list
        list.RemoveAll(kvp => kvp.Key == deadCombatant);    // removes combatant from list
        sortedturnOrder = list;                             // sets the list to be the new value of the sortedTurnOrder ienumerator
    }

    private void GivePlayerCompanionsInventory(GameObject companion)
    {

    }

    private void RequestNarration(string message)
    {
        NarrationRequest?.Invoke(message);
    }
    private void RequestAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
    {
        AbilityButtonRequest?.Invoke(abilities);
    }
}
