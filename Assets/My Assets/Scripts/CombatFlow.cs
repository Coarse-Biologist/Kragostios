using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using UnityEngine.Events;
using System.Net.NetworkInformation;
using System;
using Unity.VisualScripting;
using AbilityEnums;
using UnityEditor.Build.Pipeline;
using UnityEngine.InputSystem;

public class CombatFlow : MonoBehaviour
{
    #region // class variables

    #region // combatant variables
    public List<GameObject> combatants { private set; get; } // list of objects with the StatsHandler script attached. these are the fight participants. This is set at the beginning of s combat by dungeon master script and modified in the Handle death function
    public GameObject caster { private set; get; } //single object whose turn is currently activve
    private Dictionary<GameObject, int> uncookedList = new Dictionary<GameObject, int>(); //dict for storing s yet undorted kvp of combstsnts and an undetermined initiative roll
    private List<KeyValuePair<GameObject, int>> sortedturnOrder; // dict dtoring turn order, decided by initistive roll in DecideTurn Order function

    #endregion

    #region // event variables
    public UnityEvent<string> NarrationRequest; //ask Dm for narration which would then be sent to narrator script
    public UnityEvent<List<Ability_SO>> OptionButtonRequest; // ask Dm for a continue button to be spawned with the playeroptions script.The options are abilities which can be selected.
    public UnityEvent<List<GameObject>> TargetButtonRequest; // ask Dm for a target buttons to be spawned with the playeroptions script.
    public UnityEvent ContinueButtonRequest; // ask Dm for a con tinue button to be spawned. used to add separation between player and enemyt turns
    public UnityEvent CombatEnded; // indicates to DM that combat has ended. The remaining combatants are used to  determine who wn and lost and what is to be looted.

    #endregion

    #region // turn variables
    private int turnNumber;
    private int currentTurnIndex = 0; // variable used for iterating theough the SortedTurnOrderDictionary

    #endregion

    #region // ability/ target variables
    private int targetsExpected; //variable stores the number of targets expected by a selected ability so that the program waits for such a number before handling the affects of the ability on all targets.
    private List<GameObject> selectedTargets = new List<GameObject>(); // the targets upon which the selected ability will have effect.
    private Ability_SO selectedAbility; // stores the currently selected ability if the caster had sufficient resource to use it

    #endregion

    #region // buff debuff variables
    public Dictionary<GameObject, Dictionary<Buffs, int>> combatantBuffDurations = new Dictionary<GameObject, Dictionary<Buffs, int>>(); // Holds an outter dictioary which stores key(combatant) and value(what BUFFS they have active(key) and for how long they are active)
    public Dictionary<GameObject, Dictionary<Debuffs, int>> combatantDebuffDurations = new Dictionary<GameObject, Dictionary<Debuffs, int>>();
    // Holds an outter dictioary which stores key(combatant) and value(what DEBUFFS they have active(key) and for how long they are active)
    public Dictionary<GameObject, Dictionary<Ability_SO, int>> combatantDOTDicts = new Dictionary<GameObject, Dictionary<Ability_SO, int>>();// Holds an outter dictioary which stores key(combatant) and value(what DAMAGE OVER TIME EFFECTS they have active(key) and for how long they are active)
    public List<AbilityCategories> defensiveAbilities = new List<AbilityCategories> { AbilityCategories.Heal, AbilityCategories.Buff, AbilityCategories.BuffHeal }; // contains a list of abilities, categorized based on whether it is usually reasonable to use on ALLIED combatants(including self)
    public List<AbilityCategories> offensiveAbilities = new List<AbilityCategories> { AbilityCategories.Attack, AbilityCategories.Debuff, AbilityCategories.DebuffAttack }; // contains a list of abilities, categorized based on whether it is usually reasonable to use on ENEMY combatants(including self)
    #endregion

    #endregion

    #region // combat loop
    //Main loop for combat. Called by Dungeonmaster at the end of the function "Initiate Combat"
    public void CombatCycle()
    {
        if (currentTurnIndex > sortedturnOrder.Count - 1) currentTurnIndex = 0; // resets turn index if the end is reached

        GameObject combatant = sortedturnOrder[currentTurnIndex].Key; // stores the combatant found at the current turn index as "combatant"

        caster = combatant; // sets the caster game object to be the combatant whose turn it is.

        KDebug.SeekBug($"{sortedturnOrder} = list of caombatants");

        StatsHandler stats = combatant.GetComponent<StatsHandler>(); // retriev stat script belonging to the caster/combatant

        if (stats.currentHealth > 0) // if the combatant is alive;
        {
            DecrementBuffsandDebuffs(combatant); // decrement remaining buff/debuff durations and remove buffs/debuffs which have 0 remaining turns

            KDebug.SeekBug($"{stats.characterName}, {stats.charType} started his turn");

            switch (stats.charType) // check the combatants type (can be player, enemy, summon, companion) 
            {

                case (Combatants.Player):
                    {
                        ExecutePlayerTurn(combatant);
                    }
                    break;

                case (Combatants.Enemy):
                    ExecuteEnemyTurn(combatant);
                    break;

                case (Combatants.Summon):
                    ExecuteSummonTurn(combatant);
                    break;

                case (Combatants.Companion):
                    ExecuteCompanionTurn(combatant);
                    break;


                default:
                    {
                        KDebug.SeekBug($"{stats.characterName} was none of the above character types"); // should be an impossible case, unless something is changed (i.e a new type is added)
                        break;
                    }

            }
        }
        else
        {
            KDebug.SeekBug($"{stats.characterName} has less than or 0 health");
            HandleDeath(combatant);
        }

    }

    private void ExecutePlayerTurn(GameObject combatant)
    {

        StatsHandler stats = combatant.GetComponent<StatsHandler>();

        List<Debuffs> debuffs = GetCombatantDebuffs(combatant);
        KDebug.SeekBug($"{debuffs.Count} number of suffered debuffs");

        if (debuffs.Contains(Debuffs.Stun))
        {
            RequestNarration($"{stats.characterName} has been stunlocked and can perform no actions for his turn");
            NextTurn();
        }
        else if (debuffs.Contains(Debuffs.Charmed) || debuffs.Contains(Debuffs.Retarted))
        {
            ExecuteEnemyTurn(combatant);
        }
        else if (!debuffs.Contains(Debuffs.Stun) && !debuffs.Contains(Debuffs.Stun) && !debuffs.Contains(Debuffs.Stun))
        {
            string playerTurnIntro = $"{stats.characterName} Has big plans!... What are they?...";
            RequestNarration(playerTurnIntro);
            RequestOptionButtons(stats.knownAbilities);
        }
    }

    private void ExecuteEnemyTurn(GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        selectedAbility = GetAbility(combatant);
        RequestNarration($"{stats.characterName} decided to use {selectedAbility.AbilityName}");

        List<GameObject> targets = GetTargets(combatant);

        if (targets.Count == 0)
        {
            KDebug.SeekBug($"No targets possible for {stats.characterName} turn");

            ResetCombat();
            CombatEnded?.Invoke();
        }
        else
        {
            HandleAbilityEffect(targets, selectedAbility);
            string targetNames = "";
            foreach (GameObject target in targets)
            {
                StatsHandler targetStats = target.GetComponent<StatsHandler>();
                targetNames += $"{targetStats.characterName}, ";
            }
            string EnemyTurnNarration = $"{stats.characterName} used {selectedAbility.AbilityName} on {targetNames} for their turn.";
            RequestNarration(EnemyTurnNarration);
        }

        //RequestContinueButton();
    }

    private void ExecuteSummonTurn(GameObject combatant)
    {
        KDebug.SeekBug("Summon turn executing method called");
    }

    private void ExecuteCompanionTurn(GameObject combatant)
    {
        KDebug.SeekBug("Companion turn executing method called");

    }

    #endregion

    #region // ability management


    private void HandleAbilityEffect(List<GameObject> targets, Ability_SO selectedAbility)
    {
        List<Debuffs> debuffs = selectedAbility.DebuffEffects;
        ApplyDebufftoCombatants(debuffs, selectedAbility.TurnDuration, targets);

        foreach (GameObject target in targets)
        {
            StatsHandler stats = target.GetComponent<StatsHandler>();
            StatsHandler casterStats = caster.GetComponent<StatsHandler>();
            string narrationText = "";

            // change  resource costs based on ability type
            if (selectedAbility.Type == AbilityCategories.Heal || selectedAbility.Type == AbilityCategories.BuffHeal)
            {
                stats.ChangeResource(ResourceTypes.Health, selectedAbility.HealValue, selectedAbility.ElementType, selectedAbility.PhysicalType);
            }
            if (selectedAbility.Type == AbilityCategories.Attack || selectedAbility.Type == AbilityCategories.DebuffAttack)
            {
                stats.ChangeResource(ResourceTypes.Health, -selectedAbility.DamageValue, selectedAbility.ElementType, selectedAbility.PhysicalType);
            }
            KDebug.SeekBug($"{selectedAbility.DamageValue} = damage value. damage type =  {selectedAbility.ElementType}");

            //caster uses the cost of the ability casted
            casterStats.ChangeResource(selectedAbility.Resource, -selectedAbility.AbilityCost);
            casterStats.SpendActionPoints();

            //decide narration type based on ability type
            switch (selectedAbility.Type)
            {
                case (AbilityCategories.Heal):
                    narrationText = $"{stats.characterName} recieved {selectedAbility.HealValue} hitpoints from {selectedAbility.AbilityName}";
                    break;

                case (AbilityCategories.Attack):
                    narrationText = $"{stats.characterName} recieved {selectedAbility.DamageValue} damage from {selectedAbility.AbilityName}";
                    break;
                case (AbilityCategories.DebuffAttack):
                    narrationText = $"{stats.characterName} recieved {selectedAbility.DamageValue} damage from {selectedAbility.AbilityName} and debuffs: {selectedAbility.GetDebuffListString()}";
                    break;
            }

            RequestNarration(narrationText);
            if (stats.currentHealth <= 0) // if target dies
            {
                KDebug.SeekBug($"{target} died");
                HandleDeath(target);
            }
            else KDebug.SeekBug($"{stats.currentHealth} = targets current health");
        }
        RequestContinueButton();
    }

    public void AddSelectedTarget(GameObject target)
    {
        selectedTargets.Add(target);
        if (selectedTargets.Count == targetsExpected) // if all targets have been selected
        {
            HandleAbilityEffect(selectedTargets, selectedAbility);
        }

    }

    public void NextTurn()
    {
        StatsHandler stats = caster.GetComponent<StatsHandler>();
        // if the caster has action points left, they can take another turn
        if (stats.currentActionPoints > 0)
        {
            selectedTargets = new List<GameObject>();
            Invoke("CombatCycle", 1f);
        }
        else // if the caster has no action points left, the next combatant takes their turn
        {
            stats.RegenActionPoints();
            selectedTargets = new List<GameObject>();
            currentTurnIndex++;
            Invoke("CombatCycle", 1f);
        }
    }

    #endregion

    #region // Event functions
    private void RequestNarration(string message)
    {
        NarrationRequest?.Invoke(message);
    }

    private void RequestContinueButton()
    {
        ContinueButtonRequest?.Invoke();
    }

    private void RequestOptionButtons(List<Ability_SO> abilities)
    {
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

    public IEnumerable<KeyValuePair<GameObject, int>> DecideTurnOrder()
    {
        ResetCombat();

        foreach (GameObject combatant in combatants)
        {
            //all combatants roll initiative
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            int initiativeRoll = UnityEngine.Random.Range(0, 20) + stats.initiative;
            uncookedList.Add(combatant, initiativeRoll);
        }
        //sorts the list of combatants by initiative roll
        sortedturnOrder = uncookedList.OrderByDescending(kvp => kvp.Value).ToList();
        return sortedturnOrder;
    }

    #endregion

    #region // Death functions

    private void HandleDeath(GameObject deadCombatant)
    {
        KDebug.SeekBug($"{deadCombatant} will be removed from combat list, glaub");
        StatsHandler stats = deadCombatant.GetComponent<StatsHandler>();

        if (stats.charType == Combatants.Enemy)
        {
            HandleRewards(deadCombatant);
        }
        if (stats.charType == Combatants.Companion)
        {
            GivePlayerCompanionsInventory(deadCombatant);
        }
        RemoveFromCombat(deadCombatant);
    }

    private void HandleRewards(GameObject deadCombatant)
    {
        //calculate gold and xp
        StatsHandler stats = deadCombatant.GetComponent<StatsHandler>();
        Dictionary<Rewards, int> rewardDict = new Dictionary<Rewards, int>();
        foreach (Rewards reward in stats.rewards) // iterate through rewards in each deadCombatants reward list
        {
            switch (reward)
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
            if (victorStats.charType != Combatants.Enemy)
            {
                int goldGained = rewardDict[Rewards.Gold];
                int XpGained = rewardDict[Rewards.Xp];
                victorStats.ChangeGold(goldGained);
                victorStats.GainXp(XpGained);
                RequestNarration($"{victorStats.characterName} gained {goldGained} gold and {XpGained} XP!");
            }
        }
    }


    private void RemoveFromCombat(GameObject deadCombatant)
    {
        combatants.Remove(deadCombatant);
        sortedturnOrder.RemoveAll(kvp => kvp.Key == deadCombatant);    // removes combatant from list 
        KDebug.SeekBug($"{combatants} = combatants list");

        KDebug.SeekBug($"{sortedturnOrder} = sorted turnOrder.");
        KDebug.SeekBug($"{deadCombatant.GetComponent<StatsHandler>().characterName} will be removed from combat list. HP: {deadCombatant.GetComponent<StatsHandler>().currentHealth}");

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
        foreach (GameObject combatant in combatants)
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
        foreach (GameObject combatant in combatants)
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
    private Dictionary<Debuffs, int> GetCombatantsDebuffDict(GameObject combatant)
    {
        if (combatantDebuffDurations.TryGetValue(combatant, out Dictionary<Debuffs, int> combatantDebuffs))
        {
            return combatantDebuffs;
        }
        else return new Dictionary<Debuffs, int>();
    }

    private List<Debuffs> GetCombatantDebuffs(GameObject combatant)
    {
        Dictionary<Debuffs, int> debuffDict = GetCombatantsDebuffDict(combatant);
        List<Debuffs> debuffs = new List<Debuffs>();
        foreach (KeyValuePair<Debuffs, int> kvp in debuffDict)
        {
            debuffs.Add(kvp.Key);
        }
        return debuffs;
    }
    private Dictionary<Buffs, int> GetCombatantBuffDict(GameObject combatant)
    {

        if (combatantBuffDurations.TryGetValue(combatant, out Dictionary<Buffs, int> combatantBuffs))
        {
            return combatantBuffs;
        }
        else return new Dictionary<Buffs, int>();
    }
    private List<Buffs> GetCombatantBuffs(GameObject combatant)
    {
        Dictionary<Buffs, int> buffDict = GetCombatantBuffDict(combatant);
        List<Buffs> buffs = new List<Buffs>();

        foreach (KeyValuePair<Buffs, int> kvp in buffDict)
        {
            buffs.Add(kvp.Key);
        }
        return buffs;
    }
    private Dictionary<Ability_SO, int> GetCombatantsDOTDict(GameObject combatant)
    {
        if (combatantDOTDicts.TryGetValue(combatant, out Dictionary<Ability_SO, int> combatantDOTS))
        {
            return combatantDOTS;
        }
        else return new Dictionary<Ability_SO, int>();
    }

    private List<Dictionary<Elements, int>> GetDOTinfo(Dictionary<Ability_SO, int> DOTDict)
    {
        List<Dictionary<Elements, int>> DotInfo = new List<Dictionary<Elements, int>>();
        foreach (KeyValuePair<Ability_SO, int> DOT in DOTDict)
        {
            DotInfo.Add(new Dictionary<Elements, int> { { DOT.Key.ElementType, DOT.Key.DamageValue } });
        }
        return DotInfo;
    }

    private List<Debuffs> GetRelevantDebuffs(List<Debuffs> desiredDebuffType, GameObject combatant) /// possible problem
    {
        HashSet<Debuffs> desiredDebuffSet = new HashSet<Debuffs>(desiredDebuffType);
        List<Debuffs> relevantDebuffs = new List<Debuffs>();
        Dictionary<Debuffs, int> debuffDict = GetCombatantsDebuffDict(combatant); // only has charmed, but is passing in retarted as desired type

        foreach (var debuff in debuffDict.Keys)
        {
            if (desiredDebuffSet.Contains(debuff))
            {
                relevantDebuffs.Add(debuff);
            }
        }
        return relevantDebuffs;
    }


    private void DecrementBuffsandDebuffs(GameObject combatant)
    {
        Dictionary<Debuffs, int> combatantDebuffs = GetCombatantsDebuffDict(combatant);
        Dictionary<Buffs, int> combatantBuffs = GetCombatantBuffDict(combatant);
        foreach (KeyValuePair<Debuffs, int> kvp in combatantDebuffs.ToList())
        {
            Debuffs debuff = kvp.Key;
            int remainingDuration = kvp.Value;
            if (remainingDuration > 0)      //if remaining duration is not 0
            {
                combatantDebuffs.Remove(kvp.Key);
                combatantDebuffs.Add(debuff, remainingDuration - 1);     // remove and readd decremented duration 
            }
            if (remainingDuration == 0)
            {
                combatantDebuffs.Remove(debuff);     // else simply remove the debuff from active debuff list
            }
            KDebug.SeekBug($"remaining duration on debuff {debuff}: {remainingDuration}");
        }
        foreach (KeyValuePair<Buffs, int> kvp in combatantBuffs.ToList())
        {
            Buffs buff = kvp.Key;
            int remainingDuration = kvp.Value;
            if (remainingDuration > 0)
            {
                combatantBuffs.Remove(kvp.Key);
                combatantBuffs.Add(buff, remainingDuration - 1);
            }
            if (remainingDuration == 0)
            {
                combatantBuffs.Remove(buff);
            }
            KDebug.SeekBug($"remaining duration on debuff {buff}: {remainingDuration}");

        }
        foreach (KeyValuePair<GameObject, Dictionary<Ability_SO, int>> kvp in combatantDOTDicts)
        {
            Dictionary<Ability_SO, int> abilityDurationDict = kvp.Value;
            foreach (KeyValuePair<Ability_SO, int> innerKvp in abilityDurationDict.ToList())
            {
                int remainingDuration = innerKvp.Value;
                Ability_SO ability = innerKvp.Key;
                if (remainingDuration > 0)      //if remaining duration is not 0
                {
                    abilityDurationDict.Remove(innerKvp.Key);
                    abilityDurationDict.Add(ability, remainingDuration - 1);     // remove and readd decremented duration 
                }
                if (remainingDuration == 0)
                {
                    abilityDurationDict.Remove(ability);     // else simply remove the debuff from active debuff list
                }
                KDebug.SeekBug($"remaining duration on ability {ability} over time effect: {remainingDuration}");

            }


        }
    }


    private void ApplyDebufftoCombatants(List<Debuffs> debuffs, int duration, List<GameObject> combatants)
    {
        foreach (Debuffs debuff in debuffs)
        {
            foreach (GameObject combatant in combatants)
            {
                StatsHandler stats = combatant.GetComponent<StatsHandler>();
                if (!combatantDebuffDurations.TryGetValue(combatant, out Dictionary<Debuffs, int> debuffDict))
                {

                    debuffDict = new Dictionary<Debuffs, int> { { debuff, duration } };
                    combatantDebuffDurations.Add(combatant, debuffDict);

                    KDebug.SeekBug($"Option 0 {stats.characterName} has recieved debuff {debuff}. debuff dict len= {debuffDict.Count}. big dict len: {combatantDebuffDurations.Count}");


                }
                if (!debuffDict.TryGetValue(debuff, out duration))
                {
                    KDebug.SeekBug($"Option 1 {stats.characterName} has recieved debuff {debuff}. debuff dict len= {debuffDict.Count}. big dict len: {combatantDebuffDurations.Count}");
                    debuffDict.Add(debuff, duration);
                }
                else
                {
                    debuffDict.Remove(debuff);
                    debuffDict.Add(debuff, duration);
                    KDebug.SeekBug($"option 2 {stats.characterName} has recieved debuff {debuff}. {duration} debuff dict len = {debuffDict.Count}. big dict len : {combatantDebuffDurations.Count}");
                }
                //RequestNarration($"{combatant} has recieved debuff {debuff}. debuff dict = {debuffDict.Values}. big dict : {combatantDebuffDurations.Values}");
            }
        }

    }

    private void ApplyBufftoCombatants(Buffs buff, int duration, List<GameObject> combatants)
    {
        foreach (GameObject combatant in combatants)
        {
            if (!combatantBuffDurations.TryGetValue(combatant, out Dictionary<Buffs, int> buffDict))
            {
                buffDict = new Dictionary<Buffs, int> { { buff, duration } };
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
        combatantBuffDurations = new Dictionary<GameObject, Dictionary<Buffs, int>>();
        combatantDebuffDurations = new Dictionary<GameObject, Dictionary<Debuffs, int>>();
        combatantDOTDicts = new Dictionary<GameObject, Dictionary<Ability_SO, int>>();

    }

    #endregion

    #region // enemy turn based on Debuffs
    private Ability_SO GetAbility(GameObject combatant)
    {
        bool enemiesRemaining = CheckEnemiesRemaining(); // boolean statement // true or false
        bool alliesRemaining = CheckAlliesRemaining();

        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        List<Ability_SO> usableAbilities = new List<Ability_SO>();//abilities which would be smart to use AGAINST ones enemies

        foreach (Ability_SO ability in stats.knownAbilities)
        {
            if (enemiesRemaining)
            {
                if (defensiveAbilities.Contains(ability.Type))
                {
                    usableAbilities.Add(ability);
                }
            }
            if (alliesRemaining)
            {
                if (offensiveAbilities.Contains(ability.Type))
                {
                    {
                        usableAbilities.Add(ability);
                    }
                }
            }
        }

        int numberKnownAbilities = usableAbilities.Count();
        int randomSkillIndex = UnityEngine.Random.Range(0, numberKnownAbilities);

        KDebug.SeekBug($"{numberKnownAbilities} = number of known abilities");
        selectedAbility = usableAbilities[randomSkillIndex];
        return selectedAbility;

    }

    private List<GameObject> GetTargets(GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        List<GameObject> targets = new List<GameObject>();
        List<Debuffs> debuffs = GetCombatantDebuffs(combatant);
        //debuffs.Add(Debuffs.Charmed);
        KDebug.SeekBug($"number of debuffs suffered by {stats.characterName}: {debuffs.Count}");

        if (debuffs.Contains(Debuffs.Charmed))
        {
            if (defensiveAbilities.Contains(selectedAbility.Type))
            {
                if (stats.charType != Combatants.Enemy)
                {
                    targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
                }
                else
                {
                    targets = SelectRandomCharofType(Combatants.Allies, selectedAbility.Targets);
                }
                KDebug.SeekBug($"combatant {stats.characterName} was affected by charm and will therefore be buffing a good guy");
            }
            if (offensiveAbilities.Contains(selectedAbility.Type))
            {
                if (stats.charType != Combatants.Enemy)
                {
                    targets = SelectRandomCharofType(Combatants.Allies, selectedAbility.Targets);
                }
                else
                {
                    targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
                }
                KDebug.SeekBug($"combatant {stats.characterName} was affected by charm and will therefore be attacking a bad guy");
            }
            return targets;
        }

        if (debuffs.Contains(Debuffs.Retarted))

        {
            if (stats.charType != Combatants.Enemy)
            {
                if (defensiveAbilities.Contains(selectedAbility.Type))
                {
                    targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
                }
                else if (offensiveAbilities.Contains(selectedAbility.Type))
                {
                    targets = SelectRandomCharofType(Combatants.Allies, selectedAbility.Targets);
                }
            }
            else if (defensiveAbilities.Contains(selectedAbility.Type))
            {
                targets = SelectRandomCharofType(Combatants.Allies, selectedAbility.Targets);
            }
            else if (offensiveAbilities.Contains(selectedAbility.Type))
            {
                targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
            }
            return targets;
        }


        if (!debuffs.Contains(Debuffs.Retarted) && !debuffs.Contains(Debuffs.Charmed))
        {
            if (defensiveAbilities.Contains(selectedAbility.Type))
            {
                targets = SelectRandomCharofType(Combatants.Enemy, selectedAbility.Targets);
            }
            if (offensiveAbilities.Contains(selectedAbility.Type))
            {
                KDebug.SeekBug($"combatant {stats.characterName} was affected by neither retardation, nor charm and will therefore be attacking a good guy");
                targets = SelectRandomCharofType(Combatants.Allies, selectedAbility.Targets);
            }
            return targets;
        }
        else
        {
            return targets;
        }
    }
    #endregion
    private List<GameObject> SelectRandomCharofType(Combatants charCategory, int targetNum)//, int targetNum, bool allAllies = false) //if true, will make a list of all combatants that are not of type chartype
    {
        List<GameObject> possibleTargets = new List<GameObject>();
        foreach (GameObject combatant in combatants)
        {
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            if (charCategory == Combatants.Allies)
            {
                if (stats.charType == Combatants.Player ||
                    stats.charType == Combatants.Companion ||
                    stats.charType == Combatants.Summon)
                {
                    possibleTargets.Add(combatant);
                }
            }
            else if (charCategory == Combatants.Enemy)
            {
                if (stats.charType == charCategory)
                {
                    possibleTargets.Add(combatant);
                }

            }
        }
        List<GameObject> selectedTargets = new List<GameObject>();

        //KDebug.SeekBug($"targets = {selectedAbility.Targets}");
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
                targetsSelected++;
            }


            return selectedTargets;
        }


    }
}
// || !debuffs.Contains(Debuffs.Charmed))

//private List<GameObject> SelectRandomCharofType(Combatants charType, int targetNum, bool allAllies = false) //if true, will make a list of all combatants that are not of type chartype
//{
//    List<GameObject> possibleTargets = new List<GameObject>();
//    foreach (GameObject combatant in combatants)
//    {
//        StatsHandler stats = combatant.GetComponent<StatsHandler>();
//        if (allAllies)
//        {
//            if (stats.charType == Combatants.Player ||
//                stats.charType == Combatants.Companion ||
//                stats.charType == Combatants.Summon)
//            {
//                possibleTargets.Add(combatant);
//            }
//        }
//        else if (stats.charType == charType)
//        {
//            possibleTargets.Add(combatant);
//        }
//    }
//    List<GameObject> selectedTargets = new List<GameObject>();
//
//    //KDebug.SeekBug($"targets = {selectedAbility.Targets}");
//    int targets = targetNum;
//    int numberOfEnemies = possibleTargets.Count();
//    int targetsSelected = 0;
//    if (possibleTargets.Count == 0)
//    {
//        return possibleTargets;
//    }
//    else
//    {
//        while (targetsSelected < targets)
//        {
//            int randomTargetIndex = UnityEngine.Random.Range(0, numberOfEnemies - 1);
//            GameObject randomTarget = possibleTargets[randomTargetIndex];
//            selectedTargets.Add(randomTarget);
//            targetsSelected++;
//        }
//
//
//        return selectedTargets;
//    }
//
//
//}