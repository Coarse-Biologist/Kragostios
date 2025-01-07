using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using System.Collections.Generic;


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

private void Awake()
{
    playerOptions = GetComponent<PlayerOptions>();
    map = GetComponent<Map>();
    narrator = GetComponent<NarrationScript>();
    travel = GetComponent<TravelScript>();
    combat = GetComponent<CombatFlow>();
    Player = MakePlayer();
    List<Directions> directions = map.directions;
    //InitiateCombat();
    StatsHandler stats = Player.GetComponent<StatsHandler>();
    List<AbilityScrollStorage.Abilities> knownAbilities = stats.knownAbilities;
    playerOptions.SpawnAbilityButtons(knownAbilities);
    //playerOptions.SpawnDirectionOptions(directions);
}

private void OnEnable()
    {
        combat.NarrationRequest.AddListener(DisplayNarration);
        combat.AbilityButtonRequest.AddListener(SpawnAbilityButtons);
        playerOptions.AbilitySelected.AddListener(HandleAbilitySelected);
        playerOptions.JourneyDirectionSelected.AddListener(HandlePlayerTraveled);

    }

    private void OnDisable()
    {
        combat.NarrationRequest.RemoveListener(DisplayNarration);
        combat.AbilityButtonRequest.RemoveListener(SpawnAbilityButtons);
        playerOptions.AbilitySelected.RemoveListener(HandleAbilitySelected);
        playerOptions.JourneyDirectionSelected.RemoveListener(HandlePlayerTraveled);
    }

private void DisplayNarration(string message)
{
    narrator.DisplayNarrationText(message);
}

private void SpawnAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
{
    playerOptions.SpawnAbilityButtons(abilities);
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

}
public void HandleAbilitySelected(AbilityScrollStorage.Abilities ability) // needs a lot of work
{
    int targetNum = ability.Targets;
}
/// Command UI 
private GameObject MakePlayer()
{
    GameObject creature = Instantiate(creaturePrefab);
    StatsHandler stats = creature.GetComponent<StatsHandler>();
    Player = stats.MakeCreature(Difficulty.Nightmare, Combatants.Player);  
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


private List<GameObject> CombatantList()
{
    List<GameObject> combatants = new List<GameObject>();
    GameObject summon = MakeSummon(Difficulty.Easy);
    return combatants;
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
    combat.combatants = combatants;
    combat.CombatCycle(combatants);
}


}

