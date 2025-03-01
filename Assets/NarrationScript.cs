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

    private Dictionary<string, string> responseDictionary;

    private int numLinesOfNarration = 0;
    private List<string> stringsOfNarration = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void Awake()
    //{
    //    root = UIDocument.rootVisualElement;
    //    narratorWindow = root.Q<VisualElement>("NarratorWindow"); 
    //    Debug.Log($"{narratorWindow} = narration window");
    //    narratorText = narratorWindow.Q<Label>("NarratorText");
    //}
    private void Awake()
    {
        root = UIDocument.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("Root VisualElement is null. Ensure UIDocument is assigned and loaded properly.");
            return;
        }

        narratorWindow = root.Q<VisualElement>("NarratorWindow");
        if (narratorWindow == null)
        {
            Debug.LogError("NarratorWindow not found. Check your UXML for a VisualElement with the name 'NarratorWindow'.");
            return;
        }
        Debug.Log($"{narratorWindow} = narration window");

        narratorText = narratorWindow.Q<Label>("NarratorText");
        if (narratorText == null)
        {
            Debug.LogError("NarratorText not found. Check your UXML for a Label with the name 'NarratorText' inside NarratorWindow.");
        }
        narratorText.style.whiteSpace = WhiteSpace.Normal;
        narratorText.text = "";
        root.style.backgroundColor = Color.black;
        narratorText.style.color = Color.white;
        root.MarkDirtyRepaint();
        narratorWindow.style.display = DisplayStyle.None;
    }



    public void DisplayNarrationText(string message)
    {
        narratorWindow.style.display = DisplayStyle.Flex;

        numLinesOfNarration++;
        stringsOfNarration.Add(message);

        if (numLinesOfNarration >= 30)
        {
            stringsOfNarration.RemoveRange(0, 4);
            numLinesOfNarration -= 5;
            string trimmedMessage = "";
            foreach (string oldMessage in stringsOfNarration)
            {
                trimmedMessage += "\n \n" + oldMessage;
            }
            narratorText.text = trimmedMessage;
        }
        else
        {
            narratorText.text += "\n \n" + message; // + message;
        }


    }





    //public static class NarrationEventManager
    //{
    //    public static UnityEvent<string> OnNarrationRequested = new UnityEvent<string>();
    //
    //}

}
