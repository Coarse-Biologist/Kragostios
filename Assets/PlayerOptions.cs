using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Rendering;

public class PlayerOptions : MonoBehaviour
{
    
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    public VisualTreeAsset templateButton;
    private VisualElement buttonContainer_CO;
    private VisualElement buttonContainer_PO;
    private VisualElement charInfoPanel;
    private VisualElement abilityInfoPanel;
    private Label abilityInfoText;
    private Label charInfoText;
    private TextField myTextField;
    public UnityEvent<AbilityScrollStorage.Abilities> AbilitySelected;
    public UnityEvent<GameObject> TargetSelected;
    public UnityEvent<Directions> JourneyDirectionSelected;
    public UnityEvent ContinueSelected;
    public UnityEvent<string> IntroOptionSelected;
    public UnityEvent<string> PlayertextInput;
    public bool awaitingAbilitySelection {private set; get;}

    public Dictionary<string, List<string>> IntroOptionDict {private set; get;}
    

    [SerializeField] public AbilityScrollStorage abilityScript;

    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        buttonContainer_CO = root.Q<VisualElement>("CombatantButtons");
        charInfoPanel = root.Q<VisualElement>("CharInfoPanel");
        charInfoText = charInfoPanel.Q<Label>("CharInfo");
        buttonContainer_PO = root.Q<VisualElement>("PlayerOptions");
        abilityInfoPanel = root.Q<VisualElement>("AbilityInfoPanel");
        abilityInfoText = abilityInfoPanel.Q<Label>("AbilityInfo");
        myTextField = root.Q<TextField>("TextField");
        myTextField.style.display = DisplayStyle.None;
        
    }
    // Combat functions
    public void SpawnDirectionOptions(List<Directions> directions)
    {
        ClearAbilityContainer();
        ClearTargetContainer();
        
        foreach (Directions direction in directions)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");

            newButton.text = direction.ToString();
            buttonContainer_PO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnJourneyDirectionSelected(direction));
        }
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
        
    }
    public void SpawnAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
    {   
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        
        foreach (AbilityScrollStorage.Abilities ability in abilities)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            Debug.Log($"new ability Button requested and being processed");

            newButton.text = ability.AbilityName;
            buttonContainer_PO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnAbilitySelected(ability));
            newButton.RegisterCallback<PointerEnterEvent>(evt => ShowAbilityInfo(ability));
            newButton.RegisterCallback<PointerEnterEvent>(evt => HideCharInfo());
            newButton.RegisterCallback<PointerLeaveEvent>(evt => HideAbilityInfo());
        }
        
    }

    public void SpawnPlayerInfoButton(GameObject player)
    {
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        Button newButton = newButtonContainer.Q<Button>();
        StatsHandler stats = player.GetComponent<StatsHandler>();
        newButton.text = $"Show {stats.characterName}'s Info";
        buttonContainer_PO.Add(newButtonContainer);
        newButtonContainer.Add(newButton);
        //newButton.style.position = Position.Absolute;
        //newButton.style.bottom = 0; // Set to 0 to anchor to the bottom
        //newButton.style.left = 0;
        newButton.RegisterCallback<ClickEvent>(e => ShowPlayerInfo(player));
    }
    public void SpawnContinueButton()
    {   
        ClearAbilityContainer();
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        Button newButton = newButtonContainer.Q<Button>();
        newButton.text = "Continue";
        buttonContainer_PO.Add(newButtonContainer);
        newButtonContainer.Add(newButton);
        newButton.RegisterCallback<ClickEvent>(e => OnContinueSelected());
        
    }

    private void OnJourneyDirectionSelected(Directions direction)
    {
        JourneyDirectionSelected?.Invoke(direction);
        ClearAbilityContainer();
    }
    public void OnAbilitySelected(AbilityScrollStorage.Abilities ability)
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
        charInfoPanel.style.display = DisplayStyle.Flex;
        charInfoText.style.whiteSpace = WhiteSpace.Normal;
        charInfoText.style.color = Color.white;
        charInfoText.text = charInfo;
        //Debug.Log("Hovering over button!");
    }

    private void ShowAbilityInfo(AbilityScrollStorage.Abilities ability)
    {
        string abilityInfo = ability.GetAbilityInfo(ability);
        abilityInfoPanel.style.display = DisplayStyle.Flex;
        abilityInfoText.style.whiteSpace = WhiteSpace.Normal;
        abilityInfoText.style.color = Color.white;
        abilityInfoText.text = abilityInfo;
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
    private void OnContinueSelected()
    {
        ContinueSelected?.Invoke();
        ClearTargetContainer();
        ClearAbilityContainer();
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

        buttonContainer_PO.Clear();
    }
    
    public void ClearTargetContainer()
    {
  
        buttonContainer_CO.Clear();
    }
    public void SetAwaitingAbilitySelection(bool awaiting)
    {
        awaitingAbilitySelection = awaiting;
    }

    public void SetAbilitiesScript(AbilityScrollStorage script)
    {
        abilityScript = script;
    }

    // Intro functions
    public void SetIntroDict(Dictionary<string, List<string>> introDict)
    {
        IntroOptionDict = introDict;
    }  

    public void DisplayTextField(string message)
    {
        myTextField.style.display = DisplayStyle.Flex;
        myTextField.value = "Ecri ta appellation";
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
            buttonContainer_PO.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OptionSelected(option));
            //newButton.RegisterCallback<PointerEnterEvent>(evt => ());
            
        }
    }

    private void OptionSelected(string playerChoice)
    {
        IntroOptionSelected?.Invoke(playerChoice);
    }
    
}