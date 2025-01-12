using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;


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
private StatsHandler playerStats;

[Header("Player Intro")]

Dictionary<string, List<string>> narratorToPlayerDict;
Dictionary<string, string> playerToNarratorDict;

private bool awaitingPlayerName;

// SetUp
private void Awake()
{
    Player = MakePlayer();
    playerStats = Player.GetComponent<StatsHandler>();
    AbilityScrollStorage abilities = Player.GetComponent<AbilityScrollStorage>();
    List<AbilityScrollStorage.Abilities> knownAbilities = playerStats.knownAbilities;
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
    //RequestPlayerName();
    InitiateCombat();
}

private void OnEnable()
{
    combat.NarrationRequest.AddListener(DisplayNarration);
    combat.OptionButtonRequest.AddListener(SpawnOptionButtons);
    combat.ContinueButtonRequest.AddListener(SpawnContinueButton);
    combat.CombatEnded.AddListener(HandleCombatEnd);

    playerOptions.AbilitySelected.AddListener(HandleAbilitySelected);
    playerOptions.JourneyDirectionSelected.AddListener(HandlePlayerTraveled);
    playerOptions.TargetSelected.AddListener(HandleTargetSelected);
    playerOptions.ContinueSelected.AddListener(HandleCombatContinuePressed);
    playerOptions.IntroOptionSelected.AddListener(NarratorResponseToPlayer);
    playerOptions.PlayertextInput.AddListener(HandlePlayerTextInput);

    
}

private void OnDisable()
{
    combat.NarrationRequest.RemoveListener(DisplayNarration);
    combat.OptionButtonRequest.RemoveListener(SpawnOptionButtons);
    combat.ContinueButtonRequest.RemoveListener(SpawnContinueButton);
    combat.CombatEnded.RemoveListener(HandleCombatEnd);


    playerOptions.AbilitySelected.RemoveListener(HandleAbilitySelected);
    playerOptions.JourneyDirectionSelected.RemoveListener(HandlePlayerTraveled);
    playerOptions.TargetSelected.RemoveListener(HandleTargetSelected);
    playerOptions.ContinueSelected.RemoveListener(HandleCombatContinuePressed);
    playerOptions.PlayertextInput.RemoveListener(HandlePlayerTextInput);
    playerOptions.IntroOptionSelected.RemoveListener(NarratorResponseToPlayer);


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

private void HandleCombatContinuePressed()
{
    bool enemiesRemaining = combat.CheckEnemiesRemaining();
    bool alliesRemaining = combat.CheckAlliesRemaining();
    if (enemiesRemaining && alliesRemaining)
    {
        Debug.Log("Show goes on");
    //combat.NextTurn();
    playerOptions.SpawnPlayerInfoButton(Player);
    } 
    else if (enemiesRemaining && !alliesRemaining) 
    {
        narrator.DisplayNarrationText("YOur party has been defeated");
        HandleCombatEnd(combat.combatants);   
    }
    else if (!enemiesRemaining && alliesRemaining)
    {
        narrator.DisplayNarrationText("YOU WON");
        HandleCombatEnd(combat.combatants);
    }

    

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
private void HandleCombatEnd(List<GameObject> survivingCombatants)
{
    // do stuff
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
    playerOptions.SpawnPlayerInfoButton(Player);
}

// Map and main menu
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

private void NarratorResponseToPlayer(string playerChoice)
{
    string narratorResponse = playerToNarratorDict[playerChoice];
    narrator.DisplayNarrationText(narratorResponse); 
}

private void PresentPlayerOptions (string narratorPromt)
{
    List<string> playerAnswerOptions = narratorToPlayerDict[narratorPromt];
    playerOptions.SpawnOptionButtons(playerAnswerOptions);
}

private void RequestPlayerName()
{
    string message = "What are you called?";
    string message1 = "what does this even do???"; // from hydn
    narrator.DisplayNarrationText(message1 + message );
    playerOptions.DisplayTextField(message);
    awaitingPlayerName = true;
}

private void SetnarratorToPlayerDict()
{
    narratorToPlayerDict = new Dictionary<string, List<string>>
{
    { "Are you mighty?", new List<string> { "no", "no" } } // from Hydn
};
    
}

private Dictionary<string, List<string>> GetnarratorToPlayerDict()
{
    return narratorToPlayerDict;
}
private void HandlePlayerTextInput(string playerInput)
{
    if (awaitingPlayerName)
    {
        playerStats.SetName(playerInput);
        narrator.DisplayNarrationText($"Greetings, most exaulted {playerStats.characterName}!");
        awaitingPlayerName = false;
    }
}


}

