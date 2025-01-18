using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using AbilityEnums;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine.UI;


public class DungeonMaster : MonoBehaviour

{
#region // class variables 
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

[SerializeField] AbilityLibrary abilityLibrary;

[Header("player")]

[SerializeField] GameObject creaturePrefab;
private GameObject Player;
private StatsHandler playerStats;

[Header("Player Intro")]

Dictionary<string, List<string>> narratorToPlayerDict;
Dictionary<string, string> playerToNarratorDict;

private bool awaitingPlayerName = false;
private bool awaitingPlayerDescription = false;

#endregion

// SetUp
private void Awake()
{
    Player = MakePlayer();    
    // Initialize component references
    playerOptions = GetComponent<PlayerOptions>();
    playerOptions.SetAbilitiesScript(abilityLibrary);
    map = GetComponent<Map>();
    narrator = GetComponent<NarrationScript>();
    travel = GetComponent<TravelScript>();
    combat = GetComponent<CombatFlow>();
}
private void Start()
{
    
    //RequestPlayerName();
    //CharacterCreation();
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

    playerOptions.StatIncrented.AddListener(HandleStatIncremented);
    playerOptions.CharacterCreationConfirmed.AddListener(CharacterCreationComplete);


    
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
    playerOptions.CharacterCreationConfirmed.RemoveListener(CharacterCreationComplete);


}

// Comand UI
private void DisplayNarration(string message)
{
    Debug.Log("Display Narration request recieved?");
    narrator.DisplayNarrationText(message);
}

private void SpawnOptionButtons(List<Ability_SO> abilities)
{
    Debug.Log("Spawn ability request recieved?");
    playerOptions.SpawnAbilityButtons(playerStats.knownAbilities);
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

public void HandleAbilitySelected(Ability_SO ability) // needs a lot of work
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
    playerStats = creature.GetComponent<StatsHandler>();
    Player = playerStats.MakePlayer();
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
    //Debug.Log($"{playerStats.GetKnownAbilitiesString()} = player known abilities");
    playerOptions.HideCreationScreen();
    combatants.Add(Player);
    combat.SetCombatants(combatants);
    combat.DecideTurnOrder();
    combat.CombatCycle();//combatants);
    
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

private void SetNarratorToPlayerDict()
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
private void CharacterCreation()
{
    playerOptions.DisplayCharacterCreationScreen();
}
private void HandleStatIncremented(string stat)
{
    if (playerStats.availableStatPoints > 0)
    {
    switch(stat)
    {
        
    case "Max Mana":
    if (playerStats.availableStatPoints >= 1) playerStats.AddMaxMana(5, 1);
    break;
    case "Max Health":
    if (playerStats.availableStatPoints >= 1) playerStats.AddMaxHealth(5, 1);
        break;
    case "Max Stamina":
    if (playerStats.availableStatPoints >= 1) playerStats.AddMaxStamina(5, 1);
        break;
    case "Health Regeneration":
    if (playerStats.availableStatPoints >= 3) playerStats.AddHealthRegen(1, 3);
        break;
    case "Mana Regeneration":
    if (playerStats.availableStatPoints >= 3) playerStats.AddManaRegen(1, 3);
        break;
    case "Stamina Regeneration":
    if (playerStats.availableStatPoints >= 3 ) playerStats.AddStaminaRegen(1, 3);
        break;
    case "Max Action Points":
    if (playerStats.availableStatPoints >= 20) playerStats.AddActionPoint(1, 20);
        break;
    case "Action Point Regeneration":
    if (playerStats.availableStatPoints >= 20) playerStats.AddActionPointRegen(1, 20);
        break;
    case "Ice Affinity":
    //if (playerStats.availableStatPoints >= 1) playerStats.AddIceAffinity(5, 1);
    //    break;
    case "Cold Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddColdAffinity(5, 1);
        break;
    case "Water Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddWaterAffinity(5, 1);
        break;
    case "Earth Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddEarthAffinity(5, 1);
        break;
    case "Fire Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddFireAffinity(5, 1);
        break;
    case "Lava Affinity":
    //if (playerStats.availableStatPoints >= 1) playerStats.AddLavaAffinity(5, 1);
    //    break;
    case "Heat Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddHeatAffinity(5, 1);
        break;
    case "Air Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddAirAffinity(5, 1);
        break;
    case "Electricity Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddElectricityAffinity(5, 1);
        break;
    case "Light Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddLightAffinity(5, 1);
        break;
    case "Poison Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddPoisonAffinity(5, 1);
        break;
    case "Acid Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddAcidAffinity(5, 1);
        break;
    case "Bacteria Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddBacteriaAffinity(5, 1);
        break;
    case "Virus Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddVirusAffinity(5, 1);
        break;
    case "Fungi Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddFungiAffinity(5, 1);
        break;
    case "Plant Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddPlantAffinity(5, 1);
        break;
    case "Radiation Affinity":
    if (playerStats.availableStatPoints >= 1) playerStats.AddRadiationAffinity(5, 1);
        break;
    case "Bludgeoning Resistance":
    if (playerStats.availableStatPoints >= 1) playerStats.AddBludgeoningResist(5, 1);
        break;
    case "Slashing Resistance":
    if (playerStats.availableStatPoints >= 1) playerStats.AddSlashingResist(5, 1);
        break;
    case "Piercing Resistance":
    if (playerStats.availableStatPoints >= 1) playerStats.AddPiercingResist(5, 1);
        break;
    default:
        break;
    }
    if (stat.EndsWith("charName")) playerStats.SetName(stat.Replace("charName", ""));
    if (stat.EndsWith("charDescription")) playerStats.SetDescription(stat.Replace("charDescription", ""));
    playerOptions.DisplayeIncrementEffect(stat, playerStats);
}
else

{
    Debug.Log("Insufficient sttatpoints");
    //SpawnContinueButton();
}

}

private void CharacterCreationComplete()
{
    playerOptions.HideCreationScreen();
    narrator.DisplayNarrationText("The Story begins.");
    List<Directions> directions = map.directions;
    playerOptions.SpawnDirectionOptions(directions);
}
}



