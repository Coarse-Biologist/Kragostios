using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using System.Collections.Generic;
using Unity.VisualScripting;


public class DungeonMaster : MonoBehaviour

{

[Header("UI Interface")]
[SerializeField] UIDocument UIDocument;
private VisualElement root;
private VisualElement narratorWindow;
private Label narratorText;

[Header("scripts")]
private PlayerOptions playerOptions;
private Map map;
private NarrationScript narrator;
private TravelScript travel;
private CombatFlow combat;

[Header("player")]

[SerializeField] GameObject creaturePrefab;
private GameObject Player;

// SetUp
private void Awake()
{
    Player = MakePlayer();
    StatsHandler stats = Player.GetComponent<StatsHandler>();
    AbilityScrollStorage abilities = Player.GetComponent<AbilityScrollStorage>();
    List<AbilityScrollStorage.Abilities> knownAbilities = stats.knownAbilities;
    // Initialize component references
    playerOptions = GetComponent<PlayerOptions>();
    playerOptions.SetAbilitiesScript(abilities);
    map = GetComponent<Map>();
    narrator = GetComponent<NarrationScript>();
    travel = GetComponent<TravelScript>();
    combat = GetComponent<CombatFlow>();
}
private void Start()
{
    List<Directions> directions = map.directions;
    InitiateCombat();
}

private void OnEnable()
{
    combat.NarrationRequest.AddListener(DisplayNarration);
    combat.OptionButtonRequest.AddListener(SpawnOptionButtons);
    combat.ContinueButtonRequest.AddListener(SpawnContinueButton);
    playerOptions.AbilitySelected.AddListener(HandleAbilitySelected);
    playerOptions.JourneyDirectionSelected.AddListener(HandlePlayerTraveled);
    playerOptions.TargetSelected.AddListener(HandleTargetSelected);
    playerOptions.ContinueSelected.AddListener(HandleContinuePressed);
    
}

private void OnDisable()
{
    combat.NarrationRequest.RemoveListener(DisplayNarration);
    combat.OptionButtonRequest.RemoveListener(SpawnOptionButtons);
    combat.ContinueButtonRequest.RemoveListener(SpawnContinueButton);
    playerOptions.AbilitySelected.RemoveListener(HandleAbilitySelected);
    playerOptions.JourneyDirectionSelected.RemoveListener(HandlePlayerTraveled);
    playerOptions.TargetSelected.RemoveListener(HandleTargetSelected);
}

// Comand UI
private void DisplayNarration(string message)
{
    Debug.Log("Display Narration request recieved?");
    narrator.DisplayNarrationText(message);
}

private void SpawnOptionButtons(List<AbilityScrollStorage.Abilities> abilities)
{
    Debug.Log("Spawn ability request recieved?");
    playerOptions.SpawnAbilityButtons(abilities);
    List<GameObject> combatants = combat.combatants;
    playerOptions.SpawnTargetButtons(combatants);
}

private void SpawnContinueButton()
{
    playerOptions.SpawnContinueButton();
}

/// Handle Player input
public void HandlePlayerTraveled(Directions direction)
{
    travel.TravelInDirection(direction);
    Vector2 playerLocation = travel.playerLocation;
    LocationType locationType = map.GetLocationType(playerLocation);
    narrator.PlayerTraveled(direction, playerLocation, locationType);
    //locationType = LocationType.Hostile;
    switch(locationType)
    {
        case LocationType.Hostile:
        InitiateCombat();
        break;

        default:
        break;
    }
}
        
public void HandleTargetSelected(GameObject target) // needs a lot of work
{
    Debug.Log("Handle target Selected request recieved?");

    combat.AddSelectedTarget(target);
}

public void HandleAbilitySelected(AbilityScrollStorage.Abilities ability) // needs a lot of work
{
    StatsHandler casterStats = combat.caster.GetComponent<StatsHandler>();
    
    if (casterStats.currentMana >= ability.AbilityCost) 
    {
        string resourceCostNarration = $"{casterStats.characterName} used {ability.AbilityCost} {ability.Resource} to cast {ability.AbilityName}";
        narrator.DisplayNarrationText(resourceCostNarration);

        combat.SetSelectedAbility(ability);
        int targetNum = ability.Targets;
        combat.SetExpectedTargets(targetNum);
        List<GameObject> combatants = combat.combatants;
        
        playerOptions.SpawnTargetButtons(combatants);
    }
    else 
    {
        string insufficientResource = $"{casterStats.characterName} has insuffienct {ability.Resource} to use {ability.AbilityName}";
        narrator.DisplayNarrationText(insufficientResource);
        playerOptions.SetAwaitingAbilitySelection(true);
    }
    
}

private void HandleContinuePressed()
{
    bool enemiesRemaining = combat.CheckEnemiesRemaining();
    if (enemiesRemaining) combat.NextTurn();
    else narrator.DisplayNarrationText("YOU WON");
}

/// Combat Setup
private GameObject MakePlayer()
{
    GameObject creature = Instantiate(creaturePrefab);
    StatsHandler stats = creature.GetComponent<StatsHandler>();
    AbilityScrollStorage abilities = creature.GetComponent<AbilityScrollStorage>();
    Player = stats.MakeCreature(Difficulty.Nightmare, Combatants.Player);  
    stats.LearnAbility(abilities.DivineFire);
    return Player;  
}

private GameObject MakeEnemy(Difficulty difficulty)
{
    GameObject creature = Instantiate(creaturePrefab);
    StatsHandler stats = creature.GetComponent<StatsHandler>();
    GameObject enemy = stats.MakeCreature(difficulty, Combatants.Enemy); 
    return enemy;   
}

private GameObject MakeSummon(Difficulty difficulty)
{
    GameObject creature = Instantiate(creaturePrefab);
    StatsHandler stats = creature.GetComponent<StatsHandler>();
    GameObject summon = stats.MakeCreature(difficulty, Combatants.Summon);    
    return summon;
}

private GameObject MakeCompanion(Difficulty difficulty)
{
    GameObject creature = Instantiate(creaturePrefab);
    StatsHandler stats = creature.GetComponent<StatsHandler>();
    GameObject companion = stats.MakeCreature(difficulty, Combatants.Companion);  
    return companion;  
}

private void InitiateCombat()
{
    List<GameObject> combatants = new List<GameObject>();
    int numberofEnemies = Random.Range(1, 3);
    while(numberofEnemies > 0)
    {
        GameObject enemy = MakeEnemy(Difficulty.Easy);
        numberofEnemies --; 
        combatants.Add(enemy);
    }
    combatants.Add(Player);
    combat.SetCombatants(combatants);
    combat.DecideTurnOrder();
    combat.CombatCycle();//combatants);
}


}

