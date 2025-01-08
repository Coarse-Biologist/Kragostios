using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PlayerOptions : MonoBehaviour
{
    
    [SerializeField] UIDocument uiDocument;
    private VisualElement root;
    public VisualTreeAsset templateButton;
    private VisualElement buttonContainer;

    public UnityEvent<AbilityScrollStorage.Abilities> AbilitySelected;
    public UnityEvent<GameObject> TargetSelected;
    public UnityEvent<Directions> JourneyDirectionSelected;
    public UnityEvent ContinueSelected;
    public bool awaitingAbilitySelection {private set; get;}


    
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
        }
    }
    public void SpawnAbilityButtons(List<AbilityScrollStorage.Abilities> abilities)
    {   
        ClearAbilityContainer();
        ClearTargetContainer();
        awaitingAbilitySelection = true;
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
        foreach (AbilityScrollStorage.Abilities ability in abilities)
        {
            TemplateContainer newButtonContainer = templateButton.Instantiate();
            Button newButton = newButtonContainer.Q<Button>();
            //Debug.Log($"{newButton} = new Button. button container = {buttonContainer}///");

            newButton.text = ability.AbilityName;
            buttonContainer.Add(newButtonContainer);
            newButtonContainer.Add(newButton);
            newButton.RegisterCallback<ClickEvent>(e => OnAbilitySelected(ability));
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
    
}