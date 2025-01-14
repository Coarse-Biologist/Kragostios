using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine.UI;


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

private bool awaitingPlayerName = false;
private bool awaitingPlayerDescription = false;

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
    //playerOptions.SpawnDirectionOptions(directions);
    //RequestPlayerName();
    //InitiateCombat();
    playerOptions.DisplayCharacterCreationScreen();
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

    playerOptions.StatIncrented.AddListener(HandleStatIncremented);


    
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

    playerOptions.StatIncrented.RemoveListener(HandleStatIncremented);


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
    combat.NextTurn();
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
    //Player = stats.MakeCreature(Difficulty.Nightmare, Combatants.Player);  
    //stats.LearnAbility(abilities.DivineFire);

    return creature;  
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
        List<Directions> directions = map.directions;
        playerOptions.SpawnDirectionOptions(directions);
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

private void RequestPlayerDescription()
{
    string message = "How can one describe you? In what do you consist? Where are you from and of what are you made?";
    narrator.DisplayNarrationText(message);
    playerOptions.DisplayTextField(message);
    awaitingPlayerDescription = true;
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
        Invoke("RequestPlayerDescription", 1f);
    }
    if (awaitingPlayerDescription)
    {
        playerStats.SetDescription(playerInput);
        narrator.DisplayNarrationText($"Your character description will be changed to {playerStats.description}!");
        awaitingPlayerDescription = false;
    }
}

private void HandleStatIncremented(string stat)
{
    
    switch(stat)
    {
        
    case "Max Mana":
        playerStats.AddMaxMana(5);
        break;
    case "Max Health":
    playerStats.AddMaxHealth(5);
        break;
    case "Max Stamina":
    playerStats.AddMaxStamina(5);
        break;
    case "Health Regeneration":
    playerStats.AddHealthRegen(1);
        break;
    case "Mana Regeneration":
    playerStats.AddManaRegen(1);
        break;
    case "Stamina Regeneration":
    playerStats.AddStaminaRegen(1);
        break;
    case "Max Action Points":
    playerStats.AddActionPoint(1);
        break;
    case "Action Point Regeneration":
    playerStats.AddActionPointRegen(1);
        break;
    case "Ice Affinity":
    playerStats.AddIceAffinity(5);
        break;
    case "Cold Affinity":
    playerStats.AddColdAffinity(5);
        break;
    case "Water Affinity":
    playerStats.AddWaterAffinity(5);
        break;
    case "Earth Affinity":
    playerStats.AddEarthAffinity(5);
        break;
    case "Fire Affinity":
    playerStats.AddFireAffinity(5);
        break;
    case "Lava Affinity":
    playerStats.AddLavaAffinity(5);
        break;
    case "Heat Affinity":
    playerStats.AddHeatAffinity(5);
        break;
    case "Air Affinity":
    playerStats.AddAirAffinity(5);
        break;
    case "Electricty Affinity":
    playerStats.AddElectricityAffinity(5);
        break;
    case "Light Affinity":
    playerStats.AddLightAffinity(5);
        break;
    case "Poison Affinity":
    playerStats.AddPoisonAffinity(5);
        break;
    case "Acid Affinity":
    playerStats.AddAcidAffinity(5);
        break;
    case "Bacteria Affinity":
    playerStats.AddBacteriaAffinity(5);
        break;
    case "Virus Affinity":
    playerStats.AddVirusAffinity(5);
        break;
    case "Fungi Affinity":
    playerStats.AddFungiAffinity(5);
        break;
    case "Radiation Affinity":
    playerStats.AddRadiationAffinity(5);
        break;
    case "Bludgeoning Resistance":
    playerStats.AddBludgeoningResist(5);
        break;
    case "Slashing Resistance":
    playerStats.AddSlashingResist(5);
        break;
    case "Piercing Resistance":
    playerStats.AddPiercingResist(5);
        break;
    default:
        break;
    }
    playerOptions.DisplayeIncrementEffect(stat);
}
}


