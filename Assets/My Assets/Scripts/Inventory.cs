using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEditor.Search;
using KragostiosAllEnums;

public class Inventory : MonoBehaviour
{
    private Label itemText;
    private VisualElement root;
    private VisualElement leftCreationPanel;
    private VisualElement rightCreationPanel;
    [SerializeField] UIDocument UIDocument;
    [SerializeField] VisualTreeAsset templateButton;
    [SerializeField] Dictionary<Item_SO, int> playerInventory;
    [SerializeField] List<Item_SO> allItems;
    [SerializeField] public WorldChest worldChest;

    public UnityEvent requestInventoryScreen;
    public UnityEvent exitInventoryScreen;

    private void Awake()
    {
        VisualElement root = UIDocument.rootVisualElement;
        leftCreationPanel = root.Q<VisualElement>("LeftCreationPanel");
        rightCreationPanel = root.Q<VisualElement>("RightCreationPanel");
        itemText = rightCreationPanel.Q<Label>("CharCreationText");

    }
    public void SpawnInventoryButton(VisualElement panel, StatsHandler stats)
    {
        TemplateContainer container = templateButton.Instantiate();
        Button button = container.Query<Button>();
        button.text = "Inventory";
        button.RegisterCallback<ClickEvent>(evt => DisplayInventoryItems(stats, leftCreationPanel));
        panel.Add(button);
    }
    public void DisplayInventoryItems(StatsHandler stats, VisualElement panel)
    {
        panel.Clear();
        RequestInventoryScreen();
        //allItems = worldChest.GetItemsOfType(ItemType.Weapon);
        //List<Item_SO> playerInventory = stats.GetInventory();
        foreach (KeyValuePair<Item_SO, int> kvp in stats.Inventory)
        //foreach (Item_SO item in charItems)
        {
            Item_SO item = kvp.Key;
            TemplateContainer container = templateButton.Instantiate();
            Button button = container.Q<Button>();
            button.text = item.itemName + $" || Value: {item.itemValue} Gold || Number in Inventory: {kvp.Value}";
            //button.text = item.itemName + $" || Value: {item.itemValue} Gold || Number in Inventory:";// {Value}";

            panel.Add(container);
            container.Add(button);
            button.RegisterCallback<PointerEnterEvent>(e => ShowItemInfo(item));
            button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
        }
        TemplateContainer otherContainer = templateButton.Instantiate();
        Button exitInventoryButton = otherContainer.Q<Button>();
        otherContainer.Add(exitInventoryButton);
        panel.Add(otherContainer);
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
