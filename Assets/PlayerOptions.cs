using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Rendering;
using NUnit.Framework.Constraints;

public class PlayerOptions : MonoBehaviour
{
    
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    public VisualTreeAsset templateButton;
    private VisualElement buttonContainer_CO;
    private VisualElement buttonContainer_AO;
    private VisualElement charInfoPanel;
    private VisualElement abilityInfoPanel;
    private VisualElement narratorWindow;


    #region // Character creation screen
    private VisualElement LeftCreationPanel;
    private VisualElement MiddleCreationPanel;
    private VisualElement RightCreationPanel;
    

    # endregion

    private Label abilityInfoText;
    private Label charInfoText;
    private Label charCreationText;

    private TextField myTextField;
    public UnityEvent CharacterCreationConfirmed;
    public UnityEvent<Ability_SO> AbilitySelected;
    public UnityEvent<GameObject> TargetSelected;
    public UnityEvent<Directions> JourneyDirectionSelected;
    public UnityEvent ContinueSelected;
    public UnityEvent<string> IntroOptionSelected;
    public UnityEvent<string> PlayertextInput;
    public UnityEvent<string> StatIncrented;

    public bool awaitingAbilitySelection {private set; get;}

    public Dictionary<string, List<string>> IntroOptionDict {private set; get;}
    

    [SerializeField] public AbilityLibrary abilityLibrary;

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        buttonContainer_CO = root.Q<VisualElement>("CombatantButtons");
        charInfoPanel = root.Q<VisualElement>("CharInfoPanel");
        charInfoText = charInfoPanel.Q<Label>("CharInfo");
        buttonContainer_AO = root.Q<VisualElement>("PlayerOptions");
        abilityInfoPanel = root.Q<VisualElement>("AbilityInfoPanel");
        abilityInfoText = abilityInfoPanel.Q<Label>("AbilityInfo");
        narratorWindow = root.Q<VisualElement>("NarratorWindow");

        //myTextField = root.Q<TextField>("TextField");
        //myTextField.style.display = DisplayStyle.None;

        // char creation panels
        LeftCreationPanel = root.Q<VisualElement>("LeftCreationPanel");
        MiddleCreationPanel = root.Q<VisualElement>("MiddleCreationPanel");
        RightCreationPanel = root.Q<VisualElement>("RightCreationPanel");
        charCreationText = RightCreationPanel.Q<Label>("CharCreationText");
        

        

        
    }
    // Combat functions
    public void SpawnDirectionOptions(List<Directions> directions)
    {
        KDebug.SeekBug($"directions available to travel: {directions}");

        ClearAbilityContainer();
        ClearTargetContainer();
        ShowCombatScreen();
        HideCreationScreen();
   
        foreach (Directions direction in directions)
        {

            KDebug.SeekBug($"spawning direction button: {directions}");

            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");

            newButton.text = direction.ToString();
            buttonContainer_AO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnJourneyDirectionSelected(direction));
        }

        buttonContainer_AO.MarkDirtyRepaint();
    }
    public void SpawnTargetButtons(List<GameObject> combatants) // make this list start with friendly options
    {   

        ClearTargetContainer();
        foreach (GameObject combatant in combatants)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            newButton.text = stats.characterName + stats.charType;
            buttonContainer_CO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnTargetSelected(combatant));
            newButton.RegisterCallback<PointerEnterEvent>(evt =>ShowCharInfo(combatant));
            newButton.RegisterCallback<PointerLeaveEvent>(evt => HideCharInfo());
        }
        buttonContainer_CO.MarkDirtyRepaint();

        
    }
    public void SpawnAbilityButtons(List<Ability_SO> abilities)
    {   
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        
        foreach (Ability_SO ability in abilities)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            Debug.Log($"new ability {ability} Button requested and being processed");

            newButton.text = ability.AbilityName;
            buttonContainer_AO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnAbilitySelected(ability));
            newButton.RegisterCallback<PointerEnterEvent>(evt => ShowAbilityInfo(ability));
            newButton.RegisterCallback<PointerEnterEvent>(evt => HideCharInfo());
            newButton.RegisterCallback<PointerLeaveEvent>(evt => HideAbilityInfo());
            
        }
        buttonContainer_AO.MarkDirtyRepaint();
        //root.MarkDirtyRepaint();  
    }
    public void SetAbilitiesScript(AbilityLibrary theAbilityLibrary)
    {
        abilityLibrary = theAbilityLibrary;
    }

    public void SpawnPlayerInfoButton(GameObject player)
    {
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        Button newButton = newButtonContainer.Q<Button>();
        StatsHandler stats = player.GetComponent<StatsHandler>();
        newButton.text = $"Show {stats.characterName}'s Info";
        buttonContainer_AO.Add(newButtonContainer);
        newButtonContainer.Add(newButton);
        //newButton.style.position = Position.Absolute;
        //newButton.style.bottom = 0; // Set to 0 to anchor to the bottom
        //newButton.style.left = 0;
        newButton.RegisterCallback<ClickEvent>(e => ShowPlayerInfo(player));
        buttonContainer_CO.MarkDirtyRepaint();
    }
    public void SpawnContinueButton()
    {   
        ClearAbilityContainer();
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        Button newButton = newButtonContainer.Q<Button>();
        newButton.text = "Continue";
        buttonContainer_AO.Add(newButtonContainer);
        newButtonContainer.Add(newButton);
        newButton.RegisterCallback<ClickEvent>(e => OnContinueSelected());
        buttonContainer_AO.MarkDirtyRepaint();
        root.MarkDirtyRepaint();
    }

    private void OnJourneyDirectionSelected(Directions direction)
    {
        ClearAbilityContainer();
        JourneyDirectionSelected?.Invoke(direction);
        
    }
    public void OnAbilitySelected(Ability_SO ability)
    {   
        HideAbilityInfo();
        HideCharInfo();
        if (awaitingAbilitySelection)
        {
            awaitingAbilitySelection = false;
            AbilitySelected?.Invoke(ability);
        }
        
    }
    public void ShowPlayerInfo(GameObject player)
    {
        StatsHandler stats = player.GetComponent<StatsHandler>();
        string charInfo = stats.GetCharInfo();
        charInfoPanel.style.display = DisplayStyle.Flex;
        charInfoText.style.whiteSpace = WhiteSpace.Normal;
        charInfoText.style.color = Color.white;
        charInfoText.text = charInfo;
    }
    private void ShowCharInfo(GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string charInfo = stats.GetCharInfo();
        string charAffinities = stats.GetAffinityString();
        charInfoPanel.style.display = DisplayStyle.Flex;
        charInfoText.style.whiteSpace = WhiteSpace.Normal;
        charInfoText.style.color = Color.white;
        charInfoText.text = charInfo + charAffinities;
        //Debug.Log("Hovering over button!");
    }
    public void ShowCombatScreen()
    {
        buttonContainer_AO.style.display = DisplayStyle.Flex;
        buttonContainer_CO.style.display = DisplayStyle.Flex;
        narratorWindow.style.display = DisplayStyle.Flex;

    }

    private void ShowAbilityInfo(Ability_SO ability)
    {
        try {string abilityInfo = abilityLibrary.GetAbilityInfo(ability);
        abilityInfoPanel.style.display = DisplayStyle.Flex;
        abilityInfoText.style.whiteSpace = WhiteSpace.Normal;
        abilityInfoText.style.color = Color.white;
        abilityInfoText.text = abilityInfo;
        }
        catch(NullReferenceException)
        {
            Debug.Log("Hovering over button! but something doesnt exist");
                    Debug.Log($"ability library script: {abilityLibrary}!");


        }
        
        
        //newButton.style.backgroundColor = new StyleColor(Color.red); 
        Debug.Log("Hovering over button!");

    }
    private void HideAbilityInfo()
    {
        abilityInfoText.text = " ";
        abilityInfoPanel.style.display = DisplayStyle.None;
        Debug.Log("Not hovering over button!");
    }
    private void HideCharInfo()
    {
        charInfoPanel.style.display = DisplayStyle.None;
        charInfoText.text = " ";
        Debug.Log("Not hovering over button!");    
    }

    public void HideCreationScreen()
    {
        LeftCreationPanel.style.display = DisplayStyle.None;
        RightCreationPanel.style.display = DisplayStyle.None;
        
    }
    private void OnContinueSelected()
    {
        ClearTargetContainer();
        ClearAbilityContainer();
        ContinueSelected?.Invoke();
    }
    public void OnTargetSelected(GameObject target)
    {
        if (!awaitingAbilitySelection)
        {
            HideAbilityInfo();
            HideCharInfo();
            TargetSelected?.Invoke(target);
        }
        
    }

    public void ClearAbilityContainer()
    {

        buttonContainer_AO.Clear();
    }
    
    public void ClearTargetContainer()
    {
  
        buttonContainer_CO.Clear();
    }

    public void ClearCharCreation()
    {
        LeftCreationPanel.Clear();
    }
    public void SetAwaitingAbilitySelection(bool awaiting)
    {
        awaitingAbilitySelection = awaiting;
    }

    // Intro functions
    public void SetIntroDict(Dictionary<string, List<string>> introDict)
    {
        IntroOptionDict = introDict;
    }  

    public void DisplayTextField(string message)
    {
        myTextField.style.display = DisplayStyle.Flex;
        myTextField.value = "Ecris";
        myTextField.RegisterCallback<KeyDownEvent>(evt =>
        {
        if (evt.keyCode == KeyCode.Return) // Check for Enter key
        {
            Debug.Log("Sqreeeeeech");
            string playerMessage = myTextField.value;
            PlayertextInput?.Invoke(playerMessage);
            myTextField.style.display = DisplayStyle.None;
        }
        });
    }
    public void SpawnOptionButtons(List<string> playerOptions)
    {
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        foreach (string option in playerOptions)
        {        
            Button newButton = newButtonContainer.Q<Button>();
            newButton.text = option;
            buttonContainer_AO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OptionSelected(option));
            //newButton.RegisterCallback<PointerEnterEvent>(evt => ());
            
        }
    }

    private void OptionSelected(string playerChoice)
    {
        IntroOptionSelected?.Invoke(playerChoice);
    }

public void DisplayCharacterCreationScreen()
{
    buttonContainer_AO.style.display = DisplayStyle.None;
    // Create containers
    //TemplateContainer leftPanelButtonContainer = templateButton.Instantiate();
    LeftCreationPanel.style.display = DisplayStyle.Flex;
    //root.MarkDirtyRepaint();
    //LeftCreationPanel.MarkDirtyRepaint();

    // Define button configurations
    var buttonConfigs = new Dictionary<string, string>
    {
        // Resource buttons
        { "healthButton", "Max Health" },
        { "manaButton", "Max Mana" },
        { "staminaButton", "Max Stamina" },
        { "healthRegenButton", "Health Regeneration" },
        { "manaRegenButton", "Mana Regeneration" },
        { "staminaRegenButton", "Stamina Regeneration" },

        // Action points
        { "actionButton", "Max Action Points" },
        { "actionRegenButton", "Action Point Regeneration" },

        // Elemental affinities
        //{ "iceAffinityButton", "Ice Affinity" },
        { "coldAffinityButton", "Cold Affinity" },
        { "waterAffinityButton", "Water Affinity" },
        { "earthAffinityButton", "Earth Affinity" },
        { "fireAffinityButton", "Fire Affinity" },
        //{ "lavaAffinityButton", "Lava Affinity" },
        { "heatAffinityButton", "Heat Affinity" },
        { "airAffinityButton", "Air Affinity" },
        { "electricityAffinityButton", "Electricity Affinity" },
        { "lightAffinityButton", "Light Affinity" },
        { "poisonAffinityButton", "Poison Affinity" },
        { "acidAffinityButton", "Acid Affinity" },
        { "bacteriaAffinityButton", "Bacteria Affinity" },
        { "virusAffinityButton", "Virus Affinity" },
        { "fungiAffinityButton", "Fungi Affinity" },
        { "plantAffinityButton", "Plant Affinity" },

        { "radiationAffinityButton", "Radiation Affinity" },

        // Physical resistance
        { "bludgeoningResistanceButton", "Bludgeoning Resistance" },
        { "slashingResistanceButton", "Slashing Resistance" },
        { "piercingResistanceButton", "Piercing Resistance" }
    };

    // Create and configure buttons
    foreach (var config in buttonConfigs)
    {
        Button button = new Button { text = config.Value };
        button.style.position = Position.Relative;
        button.RegisterCallback<ClickEvent>(e => StatIncrement(config.Value));
        //leftPanelButtonContainer.Add(button);
        LeftCreationPanel.Add(button);
        button.MarkDirtyRepaint();
    }
    TemplateContainer newButtonContainer = templateButton.Instantiate();
    Button confirmButton = newButtonContainer.Q<Button>();
    LeftCreationPanel.Add(confirmButton);
    confirmButton.text = "Confirm Character and Proceed";
    confirmButton.RegisterCallback<ClickEvent>(e => CharStatsConfirmed());

    TextField charNameField = LeftCreationPanel.Q<TextField>("CharName");
    string charName = charNameField.text;
    TextField charDescriptionField = LeftCreationPanel.Q<TextField>("CharDescription");
    string charDescription = charDescriptionField.text;

    LeftCreationPanel.MarkDirtyRepaint(); 

    charNameField.RegisterValueChangedCallback(evt =>
    StatIncrement(charNameField.text +"charName"));
    charDescriptionField.RegisterValueChangedCallback(evt =>
    StatIncrement(charDescriptionField.text +"charDescription"));
    }

    private void CharStatsConfirmed()
    {
        //TextField charNameField = LeftCreationPanel.Q<TextField>("CharName");
        //string charName = charNameField.text;
        //TextField charDescriptionField = LeftCreationPanel.Q<TextField>("CharDescription");
        //string charDescription = charDescriptionField.text;
        //
        CharacterCreationConfirmed?.Invoke();
    }

    private void StatIncrement(string stat)
    {
        StatIncrented?.Invoke(stat);
    }

    public void DisplayeIncrementEffect(string statIncremented, StatsHandler playerStats)
    {
        RightCreationPanel.style.display = DisplayStyle.Flex;
        charCreationText.style.whiteSpace = WhiteSpace.Normal;
        charCreationText.style.color = Color.white;
        string playerAffinity = playerStats.GetAffinityString();
        string playerResist = playerStats.GetResistString();
        charCreationText.text = $"{playerStats.availableStatPoints} remaining. \n {playerStats.GetCharCreationStats()} \n \n {playerAffinity} \n {playerResist}";
    }
    
}