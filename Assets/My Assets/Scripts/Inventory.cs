using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEditor.Search;
using KragostiosAllEnums;
using System.Reflection.Emit;

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

    private List<TemplateContainer> traderButtons = new List<TemplateContainer>();

    public UnityEvent requestInventoryScreen;
    public UnityEvent requestTraderScreen;
    public UnityEvent exitInventoryScreen;

    private void Awake()
    {
        VisualElement root = UIDocument.rootVisualElement;
        leftCreationPanel = root.Q<VisualElement>("LeftCreationPanel");
        rightCreationPanel = root.Q<VisualElement>("RightCreationPanel");
        itemText = rightCreationPanel.Q<Label>("CharCreationText");

    }
    public void SpawnTraderButton(StatsHandler stats, VisualElement panel, List<Item_SO> traderItems)
    {
        TemplateContainer container = templateButton.Instantiate();
        Button button = container.Query<Button>();
        button.text = "Trader";
        button.RegisterCallback<ClickEvent>(evt => DisplayTraderScreen(stats, traderItems, leftCreationPanel, rightCreationPanel));
        panel.Add(button);
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
        foreach (KeyValuePair<Item_SO, int> kvp in stats.Inventory)
        {
            Item_SO item = kvp.Key;
            TemplateContainer container = templateButton.Instantiate();
            Button button = container.Q<Button>();
            button.text = item.ItemName + $" || Value: {item.ItemValue} Gold || Number in Inventory: {kvp.Value}";
            panel.Add(container);
            container.Add(button);
            button.RegisterCallback<PointerEnterEvent>(e => ShowItemInfo(item, itemText));
            button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
        }
        TemplateContainer otherContainer = templateButton.Instantiate();
        Button exitInventoryButton = otherContainer.Q<Button>();
        otherContainer.Add(exitInventoryButton);
        panel.Add(otherContainer);
        exitInventoryButton.text = "Exit";
        exitInventoryButton.RegisterCallback<ClickEvent>(evt => ExitInventory());
    }
    public void DisplayTraderScreen(StatsHandler stats, List<Item_SO> traderItems, VisualElement playerInv, VisualElement traderInv)
    {
        //foreach (TemplateContainer container in traderButtons)
        //{
        //    rightCreationPanel.Remove(container);
        //}
        traderInv.Clear();
        DisplayInventoryItems(stats, playerInv);
        RequestInventoryScreen();
        foreach (Item_SO item in traderItems)
        {
            TemplateContainer container = templateButton.Instantiate();
            Button button = container.Q<Button>();
            button.text = item.ItemName + $" || Cost: {item.ItemValue} Gold ||";
            traderInv.Add(container);
            container.Add(button);
            button.RegisterCallback<PointerEnterEvent>(e => ShowItemInfo(item, itemText));
            button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
            traderButtons.Add(container);
        }
        //TemplateContainer otherContainer = templateButton.Instantiate();
        //traderInv.Add(otherContainer);

    }

    public void RequestInventoryScreen()
    {
        leftCreationPanel.style.display = DisplayStyle.Flex;
        rightCreationPanel.style.display = DisplayStyle.Flex;
        requestInventoryScreen?.Invoke();
    }
    public void RequestTraderScreen()
    {
        leftCreationPanel.style.display = DisplayStyle.Flex;
        rightCreationPanel.style.display = DisplayStyle.Flex;
        requestTraderScreen?.Invoke();
    }

    private void ShowItemInfo(Item_SO item, Label itemText)
    {
        //HideItemInfo();
        //itemText = rightCreationPanel.Q<Label>("CharCreationText");
        //Label itemText = new
        itemText.style.display = DisplayStyle.Flex;
        itemText.style.whiteSpace = WhiteSpace.Normal;
        itemText.style.color = Color.white;
        itemText.text = item.GetItemInfo();
    }

    private void HideItemInfo()
    {
        //Label itemText = rightCreationPanel.Q<Label>("CharCreationText");
        itemText.text = " ";
    }
    private void ExitInventory()
    {
        //Label itemText = rightCreationPanel.Q<Label>("CharCreationText");
        itemText.style.justifyContent = Justify.FlexStart;
        exitInventoryScreen?.Invoke();
    }
}
