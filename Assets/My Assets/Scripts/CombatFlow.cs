using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using UnityEngine.Events;
using System.Net.NetworkInformation;
using System;
using Unity.VisualScripting;

public class CombatFlow : MonoBehaviour
{
#region // class variables

    #region // combatant variables
    public List<GameObject> combatants {private set; get;}
    public GameObject caster {private set; get;}
    private Dictionary<GameObject, int> uncookedList = new Dictionary<GameObject, int>();
    private List<KeyValuePair<GameObject,int>> sortedturnOrder;

    #endregion

    #region // event variables
    public UnityEvent<string> NarrationRequest;
    public UnityEvent<List<Ability_SO>> OptionButtonRequest;
    public UnityEvent<List<GameObject>> TargetButtonRequest;
    public UnityEvent ContinueButtonRequest; 
    public UnityEvent CombatEnded;

    #endregion
    
    #region // turn variables
    private int turnNumber;
    private int currentTurnIndex = 0;

    #endregion
    
    #region // ability/ target variables
    private int targetsExpected;
    private List<GameObject> selectedTargets = new List<GameObject>();
    private Ability_SO selectedAbility;

    #endregion
    
    #region // buff debuff variables
    public Dictionary<GameObject, Dictionary<Buffs, int>> combatantBuffDurations = new Dictionary<GameObject, Dictionary<Buffs, int>>();
    public Dictionary<GameObject, Dictionary<Debuffs, int>> combatantDebuffDurations = new Dictionary<GameObject, Dictionary<Debuffs, int>>();
    public List<Debuffs> abilitySelectionDebuffs = new List<Debuffs>{Debuffs.Retarted};
    public List<Debuffs> targetSelectionDebuffs = new List<Debuffs>{Debuffs.Retarted};
    public List<Debuffs> turnStartDebuffs = new List<Debuffs>{Debuffs.Stun};
    public List<Debuffs> abilityUsedDebuffs = new List<Debuffs>{Debuffs.InefficientHeart, Debuffs.InefficientStrength, Debuffs.InefficientSpirit};
    public List<Debuffs> attackRecievedDebuffs = new List<Debuffs>
    {
        Debuffs.WaterWeakness,
        Debuffs.ColdWeakness,
        Debuffs.EarthWeakness,
        Debuffs.FireWeakness,
        Debuffs.HeatWeakness,
        Debuffs.AirWeakness,
        Debuffs.ElectrictyWeakness,
        Debuffs.PoisonWeakness,
        Debuffs.AcidWeakness,
        Debuffs.BacteriaWeakness,
        Debuffs.FungiWeakness,
        Debuffs.PlantWeakness,
        Debuffs.VirusWeakness,
        Debuffs.RadiationWeakness,
        Debuffs.LightWeakness,
        Debuffs.PsychiWeakness,
        Debuffs.BludgeoningWeakness,
        Debuffs.SlashingWeakness,
        Debuffs.PiercingWeakness
    };

    #endregion
    #endregion

#region // combat loop
    public void CombatCycle()
    {
            
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
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string playerTurnIntro = $"{stats.characterName} Has big plans!... What are they?...";

        RequestNarration(playerTurnIntro);

        RequestOptionButtons(stats.knownAbilities);
        
    }

    private void ExecuteEnemyTurn (GameObject combatant)
    {
        List<Debuffs> relevantDebuffs = GetRelevantDebuffs(turnStartDebuffs, combatant);

        bool enemiesRemaining = CheckEnemiesRemaining(); // boolean statement // true or false
        bool alliesRemaining = CheckAlliesRemaining();

        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        List<Ability_SO> usableAbilities = new List<Ability_SO>();

        foreach (Ability_SO ability in stats.knownAbilities)
        {
            if (enemiesRemaining)
            {
                if (ability.Type == AbilityCategories.Buff
                || ability.Type == AbilityCategories.Heal
                || ability.Type == AbilityCategories.BuffHeal)
                {
                    {
                        usableAbilities.Add(ability);
                    }
                }
            }
            if (alliesRemaining)
            {
                if (ability.Type == AbilityCategories.Attack
                || ability.Type == AbilityCategories.Debuff
                || ability.Type == AbilityCategories.DebuffAttack)
                {
                    usableAbilities.Add(ability);
                }
            }
            
        }
        int numberKnownAbilities = usableAbilities.Count();

        int randomSkillIndex = UnityEngine.Random.Range(0, numberKnownAbilities);
        
        Debug.Log($"{numberKnownAbilities} = number of known abilities");
        Ability_SO selectedAbility = usableAbilities[randomSkillIndex];
        
        List<GameObject> targets = new List<GameObject>();

        switch (selectedAbility.Type)
        {
            
            case AbilityCategories.Heal:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
            break;

            case AbilityCategories.Buff:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
            break;

            case AbilityCategories.BuffHeal:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
            break;

            case AbilityCategories.Debuff:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets, true);
            break;

            case AbilityCategories.Attack:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets, true);
            break;

            case AbilityCategories.DebuffAttack:
            targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets, true);
            break;

            default:
            break;
        }
        if (targets.Count == 0)
        {
            Debug.Log($"No targets possible fopr {combatant} turn");
            RequestNarration($"No targets possible for {combatant} turn");
            ResetCombat(); 
            CombatEnded?.Invoke();
        }
        else 
        {
            HandleAbilityEffect(targets, selectedAbility);
            string targetNames = "";
            foreach(GameObject target in targets)
            {
                StatsHandler targetStats = target.GetComponent<StatsHandler>();
                targetNames += $"{targetStats.characterName}, ";
            }
            string EnemyTurnNarration = $"{combatant.name} used {selectedAbility.AbilityName} on {targetNames} for their turn.";
            RequestNarration(EnemyTurnNarration);
        }
        
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

#endregion

#region // ability management
    private List<GameObject> SelectRandomCharofType(Combatants charType, int targetNum, bool allAllies = false) //if true, will make a list of all combatants that are not of type chartype
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

        //Debug.Log($"targets = {selectedAbility.Targets}");
        int targets = targetNum;
        int numberOfEnemies = possibleTargets.Count();
        int targetsSelected = 0;
        if (possibleTargets.Count == 0)
        {
            return possibleTargets;
        }
        else 
        {
            while (targetsSelected < targets)
        {
            int randomTargetIndex = UnityEngine.Random.Range(0, numberOfEnemies - 1);
            GameObject randomTarget = possibleTargets[randomTargetIndex];
            selectedTargets.Add(randomTarget);
            targetsSelected ++;
        }

       
            return selectedTargets;
        }
        

    }

    private void HandleAbilityEffect(List<GameObject> targets, Ability_SO selectedAbility)
    {   
        foreach(GameObject target in targets)
        {
            
            StatsHandler stats = target.GetComponent<StatsHandler>();
            stats.TakeDamage(selectedAbility.DamageValue);
            StatsHandler casterStats = caster.GetComponent<StatsHandler>();
            casterStats.UpdateMana(selectedAbility.AbilityCost);
            string narrationText = "";
            switch(selectedAbility.Type)
            {
                case(AbilityCategories.Heal):
                narrationText = $"{stats.characterName} recieved {selectedAbility.HealValue} hitpoints from {selectedAbility.AbilityName}"; 
                break;

                case (AbilityCategories.Attack):
                narrationText = $"{stats.characterName} recieved {selectedAbility.DamageValue} damage from {selectedAbility.AbilityName}"; 
                break;
            }

            //string narrationText = $"{stats.characterName} recieved {selectedAbility.DamageValue} damage from {selectedAbility.AbilityName}"; 
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

   public void AddSelectedTarget(GameObject target)
    {
        selectedTargets.Add(target);
        if (selectedTargets.Count == targetsExpected)
        {
            HandleAbilityEffect(selectedTargets, selectedAbility);
        }

    }

    public void NextTurn()
    {
        selectedTargets = new List<GameObject>();
        currentTurnIndex ++;
        Invoke("CombatCycle", 1f);
    }

#endregion

#region // Event functions
    private void RequestNarration(string message)
    {
        Debug.Log("Narration request sent?");
        NarrationRequest?.Invoke(message);
    }

    private void RequestContinueButton()
    {
        ContinueButtonRequest?.Invoke();
    }

    private void RequestOptionButtons(List<Ability_SO> abilities)
    {
        Debug.Log("Ability Button request sent?");

        OptionButtonRequest?.Invoke(abilities);
    }

#endregion

#region // Setup functions
    public void SetExpectedTargets(int targetNum)
    {
        targetsExpected = targetNum;
    }

    public void SetCombatants(List<GameObject> assignedCombatants)
    {
        combatants = assignedCombatants;
    }
    public void SetSelectedAbility(Ability_SO ability)
    {
        selectedAbility = ability;
    }

    public IEnumerable<KeyValuePair<GameObject,int>> DecideTurnOrder()
    {
        ResetCombat();
        
        foreach (GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            int initiativeRoll = UnityEngine.Random.Range(0, 20) + stats.initiative;
            //Debug.Log($"{uncookedList} = uncooked list");
            uncookedList.Add(combatant, initiativeRoll);
        }
        
        sortedturnOrder = uncookedList.OrderByDescending(kvp => kvp.Value).ToList();
        return sortedturnOrder;
    }

#endregion

#region // Death functions

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
                case Rewards.Gold:
                int goldGained = ((int)stats.difficulty + 1) * UnityEngine.Random.Range(1, 10);   //multiply difficulty level x random value
                rewardDict.TryAdd(Rewards.Gold, goldGained);
                break;

                case Rewards.Xp:
                int XpGained = ((int)stats.difficulty + 1) * 10 * UnityEngine.Random.Range(1, 10);    //multiply difficulty level x random value
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
                RequestNarration($"{victorStats.characterName} gained {goldGained} gold and {XpGained} XP!");
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

#endregion

#region // getters
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
    public bool CheckAlliesRemaining() // true if any enemy remains 
    {
        bool alliesRemaining = false;
        foreach(GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            switch (stats.charType)
            {

                case Combatants.Player:
                alliesRemaining = true;
                return alliesRemaining;

                case Combatants.Summon:
                alliesRemaining = true;
                return alliesRemaining;

                case Combatants.Companion:
                alliesRemaining = true;
                return alliesRemaining;

                default:
                break;
                
            }


        }
        return alliesRemaining;
    }

#endregion

#region //buff Debuff functions
    private Dictionary<Debuffs, int> GetCombatantsDebuffs(GameObject combatant)
    {
        
        if (combatantDebuffDurations.TryGetValue(combatant, out Dictionary<Debuffs, int> combatantDebuffs))
        {
            return combatantDebuffs;
        }
        else return new Dictionary<Debuffs, int>();
    }

    private List<Debuffs> GetRelevantDebuffs(List<Debuffs> desiredDebuffType, GameObject combatant)
    {
        List<Debuffs> relevantDebuffs = new List<Debuffs>();
        Dictionary <Debuffs, int> debuffDict = GetCombatantsDebuffs(combatant);
        foreach(KeyValuePair<Debuffs, int> kvp in debuffDict)
        {
            Debuffs debuff = kvp.Key;
            if(debuffDict.TryGetValue(debuff, out int duration))
            {
                if(desiredDebuffType.Contains(debuff))
                {
                    relevantDebuffs.Add(debuff);
                }
            }
        }
        return relevantDebuffs;
    }
    private Dictionary<Buffs, int> GetCombatantsBuffs(GameObject combatant)
    {
        
        if (combatantBuffDurations.TryGetValue(combatant, out Dictionary<Buffs, int> combatantBuffs))
        {
            return combatantBuffs;
        }
        else return new Dictionary<Buffs, int>();
    }

    private void DecrementBuffsandDebuffs(GameObject combatant)
    {
        Dictionary<Debuffs, int> combatantDebuffs = GetCombatantsDebuffs(combatant);
        Dictionary<Buffs, int> combatantBuffs = GetCombatantsBuffs(combatant);
        foreach (KeyValuePair<Debuffs, int> kvp in combatantDebuffs)
        {
            Debuffs debuff = kvp.Key;
            int remainingDuration = kvp.Value;
            if(remainingDuration > 0)      //if remaining duration is not 0
            {
              combatantDebuffs.Remove(kvp.Key);
              combatantDebuffs.Add(debuff , remainingDuration - 1);     // remove and readd decremented duration 
            }
            if (remainingDuration == 0)
            {
                combatantDebuffs.Remove(debuff);     // else simply remove the debuff from active debuff list
            }
        }
        foreach (KeyValuePair<Buffs, int> kvp in combatantBuffs)
        {
            Buffs buff = kvp.Key;
            int remainingDuration = kvp.Value;
            if(remainingDuration > 0)
            {
              combatantBuffs.Remove(kvp.Key);
              combatantBuffs.Add(buff, remainingDuration - 1);
            }
            if (remainingDuration == 0)
            {
                combatantBuffs.Remove(buff);
            }
        }
    }


    private void ApplyDebufftoCombatants(Debuffs debuff, int duration, List<GameObject> combatants)
    {

        foreach (GameObject combatant in combatants)
        {
            if (!combatantDebuffDurations.TryGetValue(combatant, out Dictionary<Debuffs, int> debuffDict))
            {
                debuffDict = new Dictionary<Debuffs, int>{{debuff, duration}};
                combatantDebuffDurations.Add(combatant, debuffDict);

            }
            if (!debuffDict.TryGetValue(debuff, out duration))
            {
                debuffDict.Add(debuff, duration);
            }
            else
            {
                debuffDict.Remove(debuff);
                debuffDict.Add(debuff, duration);
            }
        }
    }

    private void ApplyBufftoCombatants(Buffs buff, int duration, List<GameObject> combatants)
    {
        foreach (GameObject combatant in combatants)
        {
            if (!combatantBuffDurations.TryGetValue(combatant, out Dictionary<Buffs, int> buffDict))
            {
                buffDict = new Dictionary<Buffs, int>{{buff, duration}};
                combatantBuffDurations.Add(combatant, buffDict);
            }
            if (!buffDict.TryGetValue(buff, out duration))
            {
                buffDict.Add(buff, duration);
            }
            else
            {
                buffDict.Remove(buff);
                buffDict.Add(buff, duration);
            }
        }
    }

#endregion

#region // cycle maintainance
    public void ResetCombat()
    {     
        selectedTargets.Clear();
        uncookedList.Clear();
    }

#endregion

#region // enemy turn based on Debuffs
private List<GameObject> GetAbility(List<Debuffs> debuffs, GameObject combatant)
    {
        bool enemiesRemaining = CheckEnemiesRemaining(); // boolean statement // true or false
        bool alliesRemaining = CheckAlliesRemaining();

        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        List<Ability_SO> usableAbilities = new List<Ability_SO>();

        foreach (Ability_SO ability in stats.knownAbilities)
        {
            if (enemiesRemaining)
            {
                if (ability.Type == AbilityCategories.Buff
                || ability.Type == AbilityCategories.Heal
                || ability.Type == AbilityCategories.BuffHeal)
                {
                    {
                        usableAbilities.Add(ability);
                    }
                }
            }
            if (alliesRemaining)
            {
                if (ability.Type == AbilityCategories.Attack
                || ability.Type == AbilityCategories.Debuff
                || ability.Type == AbilityCategories.DebuffAttack)
                {
                    usableAbilities.Add(ability);
                }
            }
            
        }
        int numberKnownAbilities = usableAbilities.Count();

        int randomSkillIndex = UnityEngine.Random.Range(0, numberKnownAbilities);
        
        Debug.Log($"{numberKnownAbilities} = number of known abilities");
        Ability_SO selectedAbility = usableAbilities[randomSkillIndex];
        
        List<GameObject> targets = new List<GameObject>();
        return targets;
}
#endregion 
}
