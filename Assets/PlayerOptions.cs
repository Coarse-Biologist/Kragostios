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
    


    
    public void SpawnDirectionOptions(List<Directions> directions)
    {
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
        root = uiDocument.rootVisualElement;
        buttonContainer = root.Q<VisualElement>("PlayerOptions");
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

    private void OnJourneyDirectionSelected(Directions direction)
    {
        JourneyDirectionSelected?.Invoke(direction);
        ClearButtonContainer();
    }


    //private void OnAbilitySelected(ClickEvent clickEvent, Abilities ability)
    //{
    //    DM.HandleAbilitySelected(ability);
    //    buttonContainer.Clear();
    //}
    public void OnAbilitySelected(AbilityScrollStorage.Abilities ability)
    {
        AbilitySelected?.Invoke(ability);
    }
    public void OnTargetSelected(GameObject target)
    {
        TargetSelected?.Invoke(target);
    }

    public void ClearButtonContainer()
    {
        buttonContainer.Clear();
    }
    
    
}
    //directions = map.directions;
        //uiDocument = GetComponent<UIDocument>();
        //travel = GetComponent<TravelScript>();
        //narrator = GetComponent<NarrationScript>();