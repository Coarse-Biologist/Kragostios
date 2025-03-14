using UnityEngine;
using UnityEngine.UIElements;
using KragostiosAllEnums;
//using AbilityEnums;
using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEditor.VersionControl;
//using UnityEngine.UI;
using System;
using Unity.VisualScripting;


public class DungeonMaster : MonoBehaviour

{
    #region // class variables 
    [Header("UI Interface")]
    [SerializeField] UIDocument UIDocument;
    private VisualElement root;

    [Header("scripts")]
    private PlayerOptions playerOptions;
    private Map map;
    private NarrationScript narrator;
    private TravelScript travel;
    private CombatFlow combat;
    private List<Tuple<Difficulty, Elements, string>> enemyCombatantTuple;
    private Inventory inventory;


    [Header("player")]

    [SerializeField] private Item_SO emptyItem;
    [SerializeField] GameObject creaturePrefab;
    private GameObject Player;
    private StatsHandler playerStats;

    [Header("Player Intro")]

    Dictionary<string, List<string>> narratorToPlayerDict;
    Dictionary<string, string> playerToNarratorDict;

    #endregion

    #region management bools
    private bool inCombat = false;
    private bool presentingQuests = false;
    private bool presentingAlchemy = false;

    #endregion

    #region // SetUp
    private void Awake()
    {
        AbilityLibrary.SetAbilityDict();
        AbilityLibrary.LoadAbilities(AbilityLibrary.allAddresses, AbilityLibrary.allAbilities);
        root = UIDocument.rootVisualElement;
        // Initialize component references
        playerOptions = GetComponent<PlayerOptions>();
        EquipmentHandler.placeHolderItem = emptyItem;
        map = GetComponent<Map>();
        narrator = GetComponent<NarrationScript>();
        travel = GetComponent<TravelScript>();
        combat = GetComponent<CombatFlow>();
        inventory = GetComponent<Inventory>();


    }
    private void Start()
    {
        WorldChest.LoadItems(WorldChest.allAddresses);
        Quests.Setup();
        Player = MakePlayer();
        //CharacterCreation();
        PresentPrologue();
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
        playerOptions.ContinueSelected.AddListener(HandleContinuePressed);
        playerOptions.OptionIsSelected.AddListener(NarratorResponseToPlayer);
        //playerOptions.PlayertextInput.AddListener(HandlePlayerTextInput);

        playerOptions.StatIncremented.AddListener(HandleStatIncremented);
        playerOptions.StringInputGiven.AddListener(HandleStringInput);

        playerOptions.CharacterCreationConfirmed.AddListener(CharacterCreationComplete);
        playerOptions.requestLoad.AddListener(LoadAllData);

        inventory.requestInventoryScreen.AddListener(ShowInventoryScreen);
        inventory.exitInventoryScreen.AddListener(ExitInventoryScreen);

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
        playerOptions.ContinueSelected.RemoveListener(HandleContinuePressed);
        playerOptions.OptionIsSelected.RemoveListener(NarratorResponseToPlayer);

        playerOptions.StatIncremented.RemoveListener(HandleStatIncremented);
        playerOptions.StringInputGiven.RemoveListener(HandleStringInput);
        playerOptions.CharacterCreationConfirmed.RemoveListener(CharacterCreationComplete);
        playerOptions.requestLoad.RemoveListener(LoadAllData);

        inventory.requestInventoryScreen.RemoveListener(ShowInventoryScreen);
        inventory.exitInventoryScreen.RemoveListener(ExitInventoryScreen);
    }
    #endregion

    #region // Comand UI
    private void DisplayNarration(string message)
    {
        narrator.DisplayNarrationText(message);
    }

    private void SpawnOptionButtons(List<Ability_SO> abilities)
    {
        List<Ability_SO> playerAbilities = new List<Ability_SO>(playerStats.knownAbilities);
        List<Ability_SO> itemAbilities = EquipmentHandler.GetEquippedItemsWithAbilities(playerStats);
        KDebug.SeekBug($"item abilities length = {itemAbilities.Count}");
        foreach (Ability_SO itemAbility in itemAbilities)
        {
            playerAbilities.Add(itemAbility);
        }
        KDebug.SeekBug($"player abilities total length = {playerAbilities.Count}");

        playerOptions.SpawnAbilityButtons(playerAbilities);
        List<GameObject> combatants = combat.combatants;
        playerOptions.SpawnTargetButtons(combatants);
    }

    private void SpawnContinueButton()
    {
        playerOptions.SpawnContinueButton();
    }
    private void ShowInventoryScreen()
    {
        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.LeftCreationPanel, playerOptions.RightCreationPanel });
        Item_SO questItem = Quests.QuestItemAtTrader();
        List<Item_SO> traderItems = WorldChest.GetTraderItems(playerStats);
        if (questItem != emptyItem && questItem != null)
        {
            traderItems.Add(questItem);
        }
        inventory.DisplayTraderScreen(playerStats, traderItems);
    }

    private void ExitInventoryScreen()
    {
        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.narratorWindow, playerOptions.buttonContainer_AO, playerOptions.buttonContainer_CO });
    }
    #endregion

    #region /// Handle Player Input 

    public void HandleTargetSelected(GameObject target) // needs a lot of work
    {
        combat.AddSelectedTarget(target);
    }

    public void HandleAbilitySelected(Ability_SO ability) // needs a lot of work
    {
        StatsHandler casterStats = combat.caster.GetComponent<StatsHandler>();

        if (casterStats.GetResourceAmount(ability.Resource) >= ability.AbilityCost)
        {
            string resourceCostNarration = $"{casterStats.characterName} used {ability.AbilityCost} {ability.Resource} to cast {ability.AbilityName}";
            narrator.DisplayNarrationText(resourceCostNarration);
            casterStats.ChangeResource(ResourceTypes.Mana, -ability.AbilityCost);
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
        if (inCombat)
        {
            HandleCombatContinuePressed();
        }
        if (presentingQuests)
        {
            PresentPrologue();
        }
    }
    private void HandleCombatContinuePressed()
    {
        bool enemiesRemaining = combat.CheckEnemiesRemaining();
        bool alliesRemaining = combat.CheckAlliesRemaining();

        if (enemiesRemaining && alliesRemaining)
        {
            combat.NextTurn();
            playerOptions.SpawnPlayerInfoButton(Player);
        }
        else if (enemiesRemaining && !alliesRemaining)
        {
            narrator.DisplayNarrationText("Your party has been defeated!");
            HandleCombatEnd();
        }
        else if (!enemiesRemaining && alliesRemaining)
        {
            narrator.DisplayNarrationText("YOU WON!");
            HandleCombatEnd();
        }
    }

    #endregion

    #region /// Combat Setup

    private GameObject MakePlayer()
    {
        GameObject creature = Instantiate(creaturePrefab);
        playerStats = creature.GetComponent<StatsHandler>();
        //Debug.Log($"{playerStats} = player stats");
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
    private void HandleCombatEnd()
    {
        HandleLoot();
        Quests.IncrementIntQuests(Quests.QuestName.DefeatEnemies, enemyCombatantTuple.Count);
        inCombat = false;
        HandlePostCombatAlchemy();
        //Invoke("ShowMainMenu", 5); // Extract 
    }
    private void InitiateCombat()
    {

        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.narratorWindow, playerOptions.buttonContainer_CO, playerOptions.buttonContainer_AO });

        inCombat = true;
        List<GameObject> combatants = new List<GameObject>();
        enemyCombatantTuple = new List<Tuple<Difficulty, Elements, string>>();
        int numberofEnemies = UnityEngine.Random.Range(1, 3);
        while (numberofEnemies > 0)
        {
            GameObject enemy = MakeEnemy(Difficulty.Easy);
            StatsHandler stats = enemy.GetComponent<StatsHandler>();
            numberofEnemies--;
            combatants.Add(enemy);
            enemyCombatantTuple.Add(new Tuple<Difficulty, Elements, string>(stats.difficulty, stats.Element, stats.characterName));
        }
        //Debug.Log($"{playerStats.GetKnownAbilitiesString()} = player known abilities");
        //playerOptions.HideCreationScreen(); // change
        //playerOptions.ShowCombatScreen();
        combatants.Add(Player);
        combat.SetCombatants(combatants);
        combat.DecideTurnOrder();
        combat.CombatCycle();
    }
    private void HandlePostCombatAlchemy()
    {
        presentingAlchemy = true;
        PresentExamExtract(enemyCombatantTuple);
    }
    private void PresentExamExtract(List<Tuple<Difficulty, Elements, string>> combatantTuple)
    {
        List<string> enemiesToExam = new List<string>();
        foreach (Tuple<Difficulty, Elements, string> tripleTuple in combatantTuple)
        {
            enemiesToExam.Add($"Examine {tripleTuple.Item3}");
        }
        enemiesToExam.Add("Continue Journeying");
        playerOptions.SpawnOptionButtons(enemiesToExam);
    }

    private void HandleLoot()
    {
        KDebug.SeekBug($"HandleLoot function: {enemyCombatantTuple.Count}");
        foreach (Tuple<Difficulty, Elements, string> tuple in enemyCombatantTuple)
        {
            Difficulty difficulty = tuple.Item1;
            Elements element = tuple.Item2;
            List<Item_SO> items = new List<Item_SO>();

            switch (difficulty)
            {
                case Difficulty.Easy:
                    items = WorldChest.GetAllItemsofRarity(Rarity.Common);
                    playerStats.AddToInventory(items[0]);
                    break;
                case Difficulty.Medium:
                    items = WorldChest.GetAllItemsofRarity(Rarity.Rare);
                    playerStats.AddToInventory(items[UnityEngine.Random.Range(0, items.Count)]);
                    break;
                case Difficulty.Hard:
                    items = WorldChest.GetAllItemsofRarity(Rarity.Epic);
                    playerStats.AddToInventory(items[UnityEngine.Random.Range(0, items.Count)]);
                    break;
                case Difficulty.Brutal:
                    items = WorldChest.GetAllItemsofRarity(Rarity.Grand);
                    playerStats.AddToInventory(items[UnityEngine.Random.Range(0, items.Count)]);
                    break;
                case Difficulty.Nightmare:
                    items = WorldChest.GetAllItemsofRarity(Rarity.Legndary);
                    playerStats.AddToInventory(items[UnityEngine.Random.Range(0, items.Count)]);
                    break;
                default:
                    break;
            }
            KDebug.SeekBug($"HandleLoot function: {items.Count}");
            foreach (Item_SO item in items)
            {
                narrator.DisplayNarrationText($"{playerStats.characterName} looted {item.ItemName}!");
            }
        }
    }

    #endregion

    #region // Travel
    public void HandlePlayerTraveled(Directions direction)
    {
        travel.TravelInDirection(direction);
        Vector2Int playerLocation = travel.playerLocation;
        LocationType locationType = map.GetLocationType(playerLocation);
        Kingdoms kingdom = map.GetKingdom(playerLocation);
        Biomes biome = map.GetBiome(playerLocation);
        narrator.DisplayNarrationText($"You are in the kingdom: {kingdom}. The surrounding biome is: {biome}");
        locationType = LocationType.Hostile;
        KDebug.SeekBug(playerStats.GetInventoryString());

        switch (locationType)
        {
            case LocationType.Hostile:
                narrator.DisplayNarrationText("Your being tingles with the threat of eminent danger! Hostiles are upon you!");
                InitiateCombat();
                break;

            case LocationType.ImpassableTerrain:
                KDebug.SeekBug($"location is a {locationType}");
                narrator.DisplayNarrationText("You found impassable terrain and needed to return the way you came.");
                HandlePlayerTraveled(map.oppositeDirections[direction]);
                break;

            case LocationType.City:
                narrator.DisplayNarrationText("You found a city. Would you like to stay and have a look around, or journey on?");
                ShowMainMenu();
                break;

            case LocationType.Village:
                narrator.DisplayNarrationText("You found a village. Would you like to stay and have a look around, or journey on?");
                ShowMainMenu();
                break;

            case LocationType.Healer:
                narrator.DisplayNarrationText("You found a healer. Would you like to look at their services, or journey on?");
                ShowMainMenu();
                break;

            case LocationType.Trader:
                narrator.DisplayNarrationText("You found a trader. Would you like to look at their services, or journey on?");
                ShowMainMenu();
                VisualElement buttonContainer_AO = root.Q<VisualElement>("PlayerOptions");
                List<Item_SO> traderItems = WorldChest.GetAllItems();
                inventory.SpawnTraderButton(buttonContainer_AO);
                break;


            case LocationType.HiddenTreasure:
                narrator.DisplayNarrationText("You found a chest! See what's inside!");
                ShowMainMenu();
                break;

            case LocationType.None:
                narrator.DisplayNarrationText("You find yourself in a rather barren wasteland. Nothing but dry futility for you here. It is likely time to journey on.");
                ShowMainMenu();
                break;

            case LocationType.Campsite:
                narrator.DisplayNarrationText("You found an excellent spot for a campsite. Would you like to stay and camp for the night, or journey on?");
                ShowMainMenu();
                break;

            case LocationType.EdgeOfTheWorld:
                narrator.DisplayNarrationText("You find yourself at the feet of the legendary Sagar'had mountain range which is said to encircle the world. Beyond this point you can find no way to progress.");
                HandlePlayerTraveled(map.oppositeDirections[direction]);
                break;

            default:
                KDebug.SeekBug($"location is a {locationType}");
                ShowMainMenu();
                break;

        }
    }

    private void ShowMainMenu()
    {
        root = UIDocument.rootVisualElement;
        VisualElement buttonContainer_AO = root.Q<VisualElement>("PlayerOptions");
        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.narratorWindow, playerOptions.buttonContainer_AO });
        buttonContainer_AO.Clear();
        inventory.SpawnInventoryButton(buttonContainer_AO);
        List<Directions> directions = map.directions;
        playerOptions.SpawnDirectionOptions(directions);
        playerOptions.DisplayLoadAndSaveButtons(playerStats, map, travel);
        DisplayNarration(Quests.GetCurrentQuestString());
    }

    #endregion

    #region // player narrator/npc dialogue
    private void NarratorResponseToPlayer(string playerChoice)
    {
        if (playerChoice == "Continue Journeying")
        {
            ShowMainMenu();
        }
        else
        {
            if (presentingQuests)
            {
                foreach (Elements element in GeneralFunctions.GetAllEnums<Elements>())
                {
                    if (element.ToString() == playerChoice)
                    {
                        Debug.Log($"players affinity to {element} will be increased");
                        playerStats.SetElement(element);
                        PresentPrologue();
                    }
                }
            }
            if (presentingAlchemy)
            {
                foreach (Tuple<Difficulty, Elements, string> combatantTuple in enemyCombatantTuple)
                {
                    if (playerChoice.Contains(combatantTuple.Item3))
                    {
                        if (playerChoice.StartsWith("Examine"))
                        {
                            Quests.IncrementIntQuests(Quests.QuestName.ExamineBodies, 1);
                            DisplayNarration(AlchemyHandler.HandleExamination(combatantTuple));
                            if (AlchemyHandler.CanExtractCores == true)
                            {
                                playerOptions.SpawnOptionButtons(new List<string> { $"Extract Core from {combatantTuple.Item3}" });
                            }
                        }
                        if (playerChoice.StartsWith("Extract"))
                        {
                            DisplayNarration(AlchemyHandler.HandleExtraction(combatantTuple));
                        }
                    }
                }
            }
            else if (!presentingQuests && !presentingAlchemy)
            {
                string narratorResponse = playerToNarratorDict[playerChoice];
                narrator.DisplayNarrationText(narratorResponse);
            }
        }

    }

    private void PresentPlayerOptions(string narratorPromt)
    {
        List<string> playerAnswerOptions = narratorToPlayerDict[narratorPromt];
        playerOptions.SpawnOptionButtons(playerAnswerOptions);
    }

    private void SetNarratorToPlayerDict()
    {
        narratorToPlayerDict = new Dictionary<string, List<string>>
{
    { "Are you mighty?", new List<string> { "no", "no" } } // from Hydn
};

    }

    private Dictionary<string, List<string>> GetNarratorToPlayerDict()
    {
        return narratorToPlayerDict;
    }
    #endregion

    #region // char creation
    private void CharacterCreation()
    {
        playerOptions.DisplayCharacterCreationScreen(playerStats);
    }
    private void HandleStatIncremented(StatType stat)
    {
        if (playerStats.availableStatPoints > playerStats.StatCostandIncDict[stat].Item1)
        {
            playerStats.IncrementAttribute(stat, playerStats.StatCostandIncDict[stat].Item2, playerStats.StatCostandIncDict[stat].Item2);

            playerOptions.DisplayeIncrementEffect(stat.ToString(), playerStats);
            Debug.Log($"HandleStatIncremented is happening");
        }
        else Debug.Log("Insufficient statpoints");
    }
    private void HandleStringInput(string input)
    {
        if (input.EndsWith("charName")) playerStats.SetName(input.Replace("charName", ""));
        if (input.EndsWith("charDescription")) playerStats.SetDescription(input.Replace("charDescription", ""));
    }
    #endregion

    #region // Character Creation Complete
    private void CharacterCreationComplete()
    {
        playerStats.RestoreResources();
        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.narratorWindow, playerOptions.buttonContainer_AO });

        //playerOptions.HideCreationScreen(); // change

        //narrator.DisplayNarrationText("The Story begins.");
        //List<Directions> directions = map.directions;
        //playerOptions.ClearCharCreation();
        //playerOptions.ShowCombatScreen();

        playerStats.LearnAbility(AbilityEnums.Abilities.Fireball);
        playerStats.LearnAbility(AbilityEnums.Abilities.HealingTouch);
        playerStats.LearnAbility(AbilityEnums.Abilities.Melee);
        playerStats.LearnAbility(AbilityEnums.Abilities.DivineStrike);
        ShowMainMenu();

    }
    #endregion

    #region handle story
    private void PresentPrologue()
    {
        presentingQuests = true;

        playerOptions.ChangeScreen(new List<VisualElement> { playerOptions.narratorWindow, playerOptions.buttonContainer_AO });
        if (Quests.prologueStep == 0)
        {
            narrator.DisplayNarrationText(Quests.ParsePrologueString(Quests.GetPrologue()[0], Elements.None));
            List<string> elements = new List<string> {
            Elements.Acid.ToString(), Elements.Poison.ToString(), Elements.Heat.ToString(), Elements.Virus.ToString(), Elements.Plant.ToString(), Elements.Radiation.ToString(), Elements.Fire.ToString(), Elements.Earth.ToString(), Elements.Air.ToString(), Elements.Psychic.ToString(), Elements.Electricity.ToString(), Elements.Radiation.ToString(), Elements.Water.ToString(), Elements.Bacteria.ToString(), Elements.Fungi.ToString()
        };
            playerOptions.SpawnOptionButtons(elements);

        }
        if (Quests.prologueStep > 0 && Quests.prologueStep < 4)
        {
            string parsedString = Quests.GetPrologue()[Quests.prologueStep];
            narrator.DisplayNarrationText(Quests.ParsePrologueString(parsedString, playerStats.Element));
            playerOptions.SpawnContinueButton();
        }
        Quests.IncrementPrologueStep();
        if (Quests.prologueStep > 4)
        {
            presentingQuests = false;
            CharacterCreation();
        }
    }

    #endregion

    private void LoadAllData()
    {
        ModdedAbilities.LoadData();
        //moddedItems.LoadData();
        playerStats.LoadStats();
        travel.LoadData();
        map.LoadData();
        //alchemy.LoadData();
        EquipmentHandler.LoadData();

    }
}

