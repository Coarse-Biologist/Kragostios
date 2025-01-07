using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using KragostiosAllEnums;
using System.Numerics;


public class NarrationScript : MonoBehaviour
{
    //private TravelScript travel;
    //[SerializeField] private Map map;
    private VisualElement root;
    private VisualElement narratorWindow;
    private Label narratorText;
    [SerializeField] UIDocument UIDocument;

    [SerializeField] 
    private Dictionary<string, string> responseDictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //travel = GetComponent<TravelScript>();
        root = UIDocument.rootVisualElement;
        narratorWindow = root.Q<VisualElement>("NarratorWindow"); 

        Debug.Log($"{narratorWindow} = narration window");
        narratorText = narratorWindow.Q<Label>("NarratorText");
    }

    public void PlayerTraveled(Directions direction, UnityEngine.Vector2 playerLocation, LocationType locationType)
    {
        narratorText.text = $"You journey {direction.ToString()}. You are at the coordinates {playerLocation.ToString()}. The area is {locationType.ToString()}";
    }

    public void DisplayNarrationText(string message)
    {
        narratorText.text += narratorText.text; // + message;
        Debug.Log($"{message}");
    }






            //public static class NarrationEventManager
            //{
            //    public static UnityEvent<string> OnNarrationRequested = new UnityEvent<string>();
//
            //}
        
    }
