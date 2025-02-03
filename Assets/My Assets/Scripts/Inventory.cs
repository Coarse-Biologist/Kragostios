using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    private Label itemText;
    private VisualElement root;
    private VisualElement leftCreationPanel;
    private VisualElement rightCreationPanel;
    [SerializeField] UIDocument UIDocument;
    [SerializeField] VisualTreeAsset templateButton;
    [SerializeField] List<Item_SO> playerInventory;

    public UnityEvent requestInventoryScreen;
    public UnityEvent exitInventoryScreen;

    private void Awake()
    {
        VisualElement root = UIDocument.rootVisualElement;
        leftCreationPanel = root.Q<VisualElement>("LeftCreationPanel");
        rightCreationPanel = root.Q<VisualElement>("RightCreationPanel");
        itemText = rightCreationPanel.Q<Label>("CharCreationText");
    }
    public void SpawnInventoryButton(VisualElement panel, StatsHandler playerStats)
    {
        TemplateContainer container = templateButton.Instantiate();
        Button button = container.Query<Button>();
        button.text = "Inventory";
        button.RegisterCallback<ClickEvent>(evt => DisplayInventoryItems(playerStats));
        panel.Add(button);
    }
    public void DisplayInventoryItems(StatsHandler playerStats)
    {
        RequestInventoryScreen();
        //List<Item_SO> playerInventory = playerStats.GetInventory();
        foreach (Item_SO item in playerInventory)
        {
            TemplateContainer container = templateButton.Instantiate();
            Button button = container.Q<Button>();
            button.text = item.itemName + " || Value:" + item.itemValue + " Gold";
            leftCreationPanel.Add(container);
            container.Add(button);
            button.RegisterCallback<PointerEnterEvent>(e => ShowItemInfo(item));
            button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
        }
        TemplateContainer otherContainer = templateButton.Instantiate();
        Button exitInventoryButton = otherContainer.Q<Button>();
        otherContainer.Add(exitInventoryButton);
        leftCreationPanel.Add(otherContainer);
        exitInventoryButton.text = "Exit Inventory";
        exitInventoryButton.RegisterCallback<ClickEvent>(evt => ExitInventory());
    }

    public void RequestInventoryScreen()
    {
        //leftCreationPanel.Clear();
        //rightCreationPanel.Clear();
        leftCreationPanel.style.display = DisplayStyle.Flex;
        rightCreationPanel.style.display = DisplayStyle.Flex;
        requestInventoryScreen?.Invoke();
    }

    private void ShowItemInfo(Item_SO item)
    {
        //HideItemInfo();
        itemText.style.whiteSpace = WhiteSpace.Normal;
        itemText.style.color = Color.white;
        itemText.text = item.itemDescription;
    }

    private void HideItemInfo()
    {
        itemText.text = " ";
    }
    private void ExitInventory()
    {
        exitInventoryScreen?.Invoke();
    }

}
