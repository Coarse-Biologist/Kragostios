using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEditor.Search;
using KragostiosAllEnums;
using System.Reflection.Emit;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    #region // UI elements
    private UnityEngine.UIElements.Label itemInfo;
    private VisualElement root;
    private VisualElement leftCreationPanel;
    private VisualElement rightCreationPanel;
    [SerializeField] UIDocument UIDocument;
    [SerializeField] VisualTreeAsset templateButton;
    # endregion 

    [SerializeField] Dictionary<Item_SO, int> playerInventory;
    private StatsHandler playerStats;
    private EquipmentHandler equipmentHandler;
    [SerializeField] List<Item_SO> allItems;
    [SerializeField] public WorldChest worldChest;

    private bool itemSelected = false;
    private Item_SO selectedSellableItem;
    private Item_SO selectedBuyableItem;
    private Button selectedButton;
    private Item_SO selectedEquippableItem;
    private ItemSlot selectedItemSlotType = ItemSlot.None;

    private List<Button> currentSlotButtons = new List<Button>();


    private List<TemplateContainer> traderButtons = new List<TemplateContainer>();

    public UnityEvent requestInventoryScreen;
    public UnityEvent requestTraderScreen;
    public UnityEvent exitInventoryScreen;
    public UnityEvent<Item_SO, bool> requestTransaction;

    private void Awake()
    {
        VisualElement root = UIDocument.rootVisualElement;
        leftCreationPanel = root.Q<VisualElement>("LeftCreationPanel");
        rightCreationPanel = root.Q<VisualElement>("RightCreationPanel");
        itemInfo = new UnityEngine.UIElements.Label("");
        equipmentHandler = GetComponent<EquipmentHandler>();

    }
    public void SpawnTraderButton(StatsHandler stats, VisualElement panel, List<Item_SO> traderItems)
    {
        playerStats = stats;
        TemplateContainer container = templateButton.Instantiate();
        Button button = container.Query<Button>();
        button.text = "Trader";
        button.RegisterCallback<ClickEvent>(evt => DisplayTraderScreen(stats, traderItems, leftCreationPanel, rightCreationPanel));
        panel.Add(button);
    }
    public void SpawnInventoryButton(VisualElement panel, StatsHandler stats)
    {
        playerStats = stats;
        TemplateContainer container = templateButton.Instantiate();
        Button button = container.Query<Button>();
        button.text = "Inventory";
        button.RegisterCallback<ClickEvent>(evt => DisplayInventoryItems(stats, leftCreationPanel));
        panel.Add(button);
    }
    public void DisplayInventoryItems(StatsHandler stats, VisualElement panel)
    {
        equipmentHandler.SetPlayerStats(playerStats);
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
            button.RegisterCallback<ClickEvent>(e => ShowItemInfo(item, false));
            button.RegisterCallback<PointerEnterEvent>(e => AlterColor(button));
            button.RegisterCallback<ClickEvent>(e => ShowSlotOptions(item, panel));
            button.RegisterCallback<ClickEvent>(e => selectedEquippableItem = item);
            //button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
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
            button.RegisterCallback<ClickEvent>(e => ShowItemInfo(item, true));
            button.RegisterCallback<ClickEvent>(e => AlterColor(button));
            //button.RegisterCallback<PointerLeaveEvent>(evt => HideItemInfo());
            traderButtons.Add(container);
        }
        TemplateContainer sellContainer = templateButton.Instantiate();
        Button sellButton = sellContainer.Q<Button>();
        sellButton.text = "Sell";
        TemplateContainer buyContainer = templateButton.Instantiate();
        sellButton.RegisterCallback<ClickEvent>(e => RequestTransaction(selectedSellableItem, false));
        Button buyButton = buyContainer.Q<Button>();
        buyButton.text = "Buy";
        buyButton.RegisterCallback<ClickEvent>(e => RequestTransaction(selectedBuyableItem, true));
        traderInv.Add(sellButton);
        traderInv.Add(buyButton);

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

    private void ShowItemInfo(Item_SO item, bool tradersItem)
    {
        Debug.Log($"item clicked = {item.name}");
        HideItemInfo();
        if (itemSelected == false || item != selectedSellableItem || item != selectedBuyableItem)
        {
            itemSelected = true;
            if (tradersItem)
            {
                selectedBuyableItem = item;
                selectedSellableItem = null;
            }
            else
            {
                selectedSellableItem = item;
                selectedBuyableItem = null;
            }

            itemInfo = new UnityEngine.UIElements.Label("");
            rightCreationPanel.Add(itemInfo);
            itemInfo.style.display = DisplayStyle.Flex;
            itemInfo.style.whiteSpace = WhiteSpace.Normal;
            itemInfo.style.color = Color.white;
            itemInfo.text = item.GetItemInfo();
        }
        else
        {
            if (item == selectedBuyableItem || item == selectedSellableItem)
            {
                itemSelected = false;
                HideItemInfo();
            }

        }
    }

    private void RequestTransaction(Item_SO selectedItem, bool buying) // true if buying, false if selling
    {
        if (selectedItem != null)
        {

            if (buying)
            {
                if (selectedItem == selectedBuyableItem)
                {
                    if (playerStats.characterGold >= selectedItem.ItemValue)
                    {
                        playerStats.AddToInventory(selectedItem);
                        playerStats.ChangeGold(-selectedItem.ItemValue);
                        Debug.Log($"Player spent {selectedItem.ItemValue} gold");
                    }


                    else
                    {
                        Debug.Log($"Player lacked the required {selectedItem.ItemValue} gold");
                    }

                }
                else { Debug.Log($"You cannot buy your own item"); }
            }
            else if (!buying) // selling
            {
                if (selectedItem == selectedSellableItem)
                {
                    playerStats.RemoveFromInventory(selectedItem);
                    playerStats.ChangeGold(+selectedItem.ItemValue);
                    Debug.Log($"Player sold {selectedItem.ItemName} for {selectedItem.ItemValue} gold");
                }
                else { Debug.Log("You cant sell an item that doesnt belong to you"); }
            }
            DisplayInventoryItems(playerStats, leftCreationPanel);
        }
        else Debug.Log("You cannot perform a transaction with an object that is actually nothing");

    }
    private void HideItemInfo()
    {
        if (rightCreationPanel.Contains(itemInfo))
        {
            rightCreationPanel.Remove(itemInfo);
        }
        //Label itemText = rightCreationPanel.Q<Label>("CharCreationText");
        itemInfo.text = "";
    }
    private void ExitInventory()
    {
        //Label itemText = rightCreationPanel.Q<Label>("CharCreationText");
        rightCreationPanel.Clear();
        leftCreationPanel.Clear();
        exitInventoryScreen?.Invoke();
    }
    private void AlterColor(Button button)
    {
        if (selectedButton != null)
        {
            selectedButton.style.backgroundColor = new StyleColor(Color.grey);
        }
        button.style.backgroundColor = new StyleColor(Color.HSVToRGB(120f / 360f, 1f, 0.5f));
        selectedButton = button;
    }

    #region // equipment
    private void ShowSlotOptions(Item_SO item, VisualElement panel)
    {

        foreach (Button button in currentSlotButtons)
        {
            VisualElement parent = button.parent; // Get parent container
            if (parent != null && panel.Contains(parent))
            {
                panel.Remove(parent);
            }
        }

        currentSlotButtons = new List<Button>();
        List<ItemSlot> itemSlots = item.ItemSlot;
        if (itemSlots[0] != ItemSlot.None)
        {
            foreach (ItemSlot slot in itemSlots)
            {
                TemplateContainer container = templateButton.Instantiate();
                Button button = container.Q<Button>();
                button.text = $"Equip {item.ItemName} in {slot}";
                panel.Add(container);
                container.Add(button);
                button.RegisterCallback<ClickEvent>(e => HandleEquip(slot, item));
                currentSlotButtons.Add(button);
            }
        }

    }

    private void HandleEquip(ItemSlot slot, Item_SO item)
    {
        //Item_SO item = selectedEquippableItem;
        if (item != null)
        {
            if (slot != ItemSlot.None)
            {
                equipmentHandler.DecideEquipItem(playerStats, item, slot);
                Debug.Log($"You have equipped {equipmentHandler.GetItemNameFromSlot(playerStats, slot)} from slot {slot}");
                Debug.Log($"AL EQUIPMENT: {equipmentHandler.GetAllSlotItems(playerStats)}");
            }
            else Debug.Log("Selected item cannot be equipped.");
        }
        else Debug.Log("Select an item to equip");
    }

    #endregion
}
