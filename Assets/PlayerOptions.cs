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
    private VisualElement buttonContainer;
    private VisualElement charInfoPanel;
    private VisualElement abilityInfoPanel;
    private Label abilityInfoText;
    private Label charInfoText;
    public UnityEvent<AbilityScrollStorage.Abilities> AbilitySelected;
    public UnityEvent<GameObject> TargetSelected;
    public UnityEvent<Directions> JourneyDirectionSelected;
    public UnityEvent ContinueSelected;
    public bool awaitingAbilitySelection {private set; get;}

    [SerializeField] public AbilityScrollStorage.Abilities abilities;


    
    public void SpawnDirectionOptions(List<Directions> directions)
    {
        ClearAbilityContainer();
        ClearTargetContainer();
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
        foreach (Directions direction in directions)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");

            newButton.text = direction.ToString();
            buttonContainer.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnJourneyDirectionSelected(direction));
        }
    }
    public void SpawnTargetButtons(List<GameObject> combatants) // make this list start with friendly options
    {
        ClearAbilityContainer();
        ClearTargetContainer();
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("CombatantButtons");
        charInfoPanel = root.Q<VisualElement>("CharInfoPanel");
        charInfoText = charInfoPanel.Q<Label>("CharInfo");
        foreach (GameObject combatant in combatants)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");
            StatsHandler stats = combatant.GetComponent<StatsHandler>();
            newButton.text = stats.characterName + stats.charType;
            buttonContainer.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnTargetSelected(combatant));
            newButton.RegisterCallback<PointerEnterEvent>(evt =>ShowCharInfo(combatant));
        newButton.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            charInfoPanel.style.display = DisplayStyle.None;
            charInfoText.text = " ";
            Debug.Log("Not hovering over button!");
            newButton.style.backgroundColor = new StyleColor(Color.white);
        });
        }
    }
    public void SpawnAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
    {   
        ClearAbilityContainer();
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
        abilityInfoPanel = root.Q<VisualElement>("AbilityInfoPanel");
        abilityInfoText = abilityInfoPanel.Q<Label>("AbilityInfo");
        foreach (AbilityScrollStorage.Abilities ability in abilities)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");

            newButton.text = ability.AbilityName;
            buttonContainer.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnAbilitySelected(ability));
            newButton.RegisterCallback<PointerEnterEvent>(evt => ShowAbilityInfo(ability));
            newButton.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            abilityInfoText.text = " ";
            abilityInfoPanel.style.display = DisplayStyle.None;
            Debug.Log("Not hovering over button!");
            newButton.style.backgroundColor = new StyleColor(Color.white);
        });
        }
    }
    public void SpawnContinueButton()
    {   
        ClearAbilityContainer();
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
        
        TemplateContainer newButtonContainer = templateButton.Instantiate();
        Button newButton = newButtonContainer.Q<Button>();
        newButton.text = "Continue";
        buttonContainer.Add(newButtonContainer);
        newButtonContainer.Add(newButton);
        newButton.RegisterCallback<ClickEvent>(e => OnContinueSelected());
        
    }

    private void OnJourneyDirectionSelected(Directions direction)
    {
        JourneyDirectionSelected?.Invoke(direction);
        ClearAbilityContainer();
    }

    public void OnAbilitySelected(AbilityScrollStorage.Abilities ability)
    {   if (awaitingAbilitySelection)
        {
            awaitingAbilitySelection = false;
            AbilitySelected?.Invoke(ability);
        }
        
    }
    private void ShowCharInfo(GameObject combatant)
    {
        StatsHandler stats = combatant.GetComponent<StatsHandler>();
        string charInfo = stats.GetCharInfo();
        charInfoPanel.style.display = DisplayStyle.Flex;
        charInfoText.text = charInfo;
        charInfoText.style.color = Color.white;
        Debug.Log("Hovering over button!");
    }

    private void ShowAbilityInfo(AbilityScrollStorage.Abilities ability)
    {
        string abilityInfo = abilities.GetAbilityInfo(ability);
        abilityInfoPanel.style.display = DisplayStyle.Flex;
        Debug.Log("Hovering over button!");
        abilityInfoText.style.color = Color.white;
        abilityInfoText.text = abilityInfo;
        //newButton.style.backgroundColor = new StyleColor(Color.red);
        
    }
    private void OnContinueSelected()
    {
        ContinueSelected?.Invoke();
        ClearTargetContainer();
        ClearAbilityContainer();
    }
    public void OnTargetSelected(GameObject target)
    {
        TargetSelected?.Invoke(target);
    }

    public void ClearAbilityContainer()
    {
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
        buttonContainer.Clear();
    }
    
    public void ClearTargetContainer()
    {
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("CombatantButtons");
        buttonContainer.Clear();
    }
    public void SetAwaitingAbilitySelection(bool awaiting)
    {
        awaitingAbilitySelection = awaiting;
    }

    public void SetAbilitiesScript(AbilityScrollStorage.Abilities script)
    {
        abilities = script;
    }
    
}