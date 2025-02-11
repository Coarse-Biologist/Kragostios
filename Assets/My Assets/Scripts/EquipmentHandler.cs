using System;
using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Constraints;

public class EquipmentHandler : MonoBehaviour
{
    public Item_SO placeHolderItem;
    public Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>> allEquipmentDicts = new Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>>();
    private StatsHandler PlayerStats;
    private List<ItemSlot> allItemSlots;
    private bool playerDictAdded = false;

    void Awake()
    {
        allItemSlots = Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>().ToList();
    }
    public void SetPlayerStats(StatsHandler playerStats)
    {
        PlayerStats = playerStats;
        if (!playerDictAdded)
        {
            Debug.Log($"Adding player stats ({playerStats}) to dict");
            AddCharToEquipmentDict(PlayerStats);
            playerDictAdded = true;
        }
        Debug.Log($"player dict added? = {playerDictAdded}");
    }

    private void AddCharToEquipmentDict(StatsHandler stats)
    {
        Dictionary<ItemSlot, Item_SO> charEquipment = new Dictionary<ItemSlot, Item_SO>();
        allEquipmentDicts.TryAdd(stats, charEquipment);
        foreach (ItemSlot slot in allItemSlots)
        {
            charEquipment.Add(slot, placeHolderItem);
        }
    }

    public void DecideEquipItem(StatsHandler stats, Item_SO item, ItemSlot slot = ItemSlot.None)
    {
        Debug.Log($"Slot of {item.ItemName} selected = {slot}");
        // get the correct equipment dict
        if (item.ItemType == ItemType.Weapon)               // check if its a weapon
        {
            if (slot == ItemSlot.TwoHands)         // check if its a two hander
            {
                Debug.Log($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.RightHand, ItemSlot.LeftHand, ItemSlot.TwoHands }, ItemSlot.TwoHands, item);
            }
            if (slot == ItemSlot.RightHand)
            {
                Debug.Log($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.RightHand, ItemSlot.TwoHands }, slot, item);
            }
            if (slot == ItemSlot.LeftHand)
            {
                Debug.Log($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.LeftHand, ItemSlot.TwoHands }, slot, item);
            }
            //else HandleEquipItem(stats, new List<ItemSlot> { slot }, slot, item);
            // check weapon slot
            //checked if there was a previously equipped weapon in that slot
            //unequip previous weapon
            //equip weapon in slot
        }
    }
    private void HandleEquipItem(StatsHandler stats, List<ItemSlot> unequipSlots, ItemSlot equipSlot, Item_SO item)
    {
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        foreach (ItemSlot slot in unequipSlots)
        {
            if (equipment.TryGetValue(slot, out Item_SO dictValue))  // check if the slot exists in the dictionary
            {
                Unequip(stats, slot);                   // clear it
            }
            // equip item in newly cleared slot
        }
        if (equipSlot == ItemSlot.LeftHand && stats.GetNumItemsInInventory(item) < 2)
        {
            if (GetItemFromSlot(stats, ItemSlot.RightHand) == item)
            {
                Unequip(stats, ItemSlot.RightHand);
            }
        }
        if (equipSlot == ItemSlot.RightHand && stats.GetNumItemsInInventory(item) < 2)
        {
            if (GetItemFromSlot(stats, ItemSlot.LeftHand) == item)
            {
                Unequip(stats, ItemSlot.LeftHand);
            }
        }
        EquipItem(stats, equipSlot, item);

    }

    public void Unequip(StatsHandler stats, ItemSlot slot)
    {
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        if (equipment.TryGetValue(slot, out Item_SO dictValue))
        {
            Debug.Log($"You want to unequip {dictValue.ItemName} in slot {slot}");
            equipment[slot] = placeHolderItem;
        }
        else equipment.TryAdd(slot, placeHolderItem);
    }

    public void EquipItem(StatsHandler stats, ItemSlot slot, Item_SO item)
    {
        Debug.Log($"You want to equip {item.ItemName}");
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        if (equipment.TryGetValue(slot, out Item_SO whoCares))
        {
            Debug.Log($"{item.ItemName} is actually litterally being added to {slot}");
            equipment[slot] = item;
        }

    }

    public string GetItemNameFromSlot(StatsHandler stats, ItemSlot slot)
    {
        string itemName = "None";
        if (allEquipmentDicts.TryGetValue(stats, out Dictionary<ItemSlot, Item_SO> charEquipment))
        {
            if (charEquipment.TryGetValue(slot, out Item_SO item))
            {
                item = charEquipment[slot];
                if (item != placeHolderItem)
                {
                    itemName = item.ItemName;
                }
            }
        }
        Debug.Log("dict has no key playerStats");
        return itemName;
    }

    public Item_SO GetItemFromSlot(StatsHandler stats, ItemSlot slot)
    {
        Item_SO itemInSlot = placeHolderItem;
        if (allEquipmentDicts.TryGetValue(stats, out Dictionary<ItemSlot, Item_SO> charEquipment))
        {
            if (charEquipment.TryGetValue(slot, out Item_SO item))
            {
                item = charEquipment[slot];
                if (item != placeHolderItem)
                {
                    itemInSlot = item;
                }
            }

        }
        return itemInSlot;
    }
    public string GetAllSlotItems(StatsHandler stats)
    {
        string slotAndItem = "";
        if (allEquipmentDicts.TryGetValue(stats, out Dictionary<ItemSlot, Item_SO> charEquipment))
        {
            foreach (KeyValuePair<ItemSlot, Item_SO> kvp in charEquipment)
            {
                string itemName = kvp.Value.ItemName;
                if (kvp.Key != ItemSlot.None)
                {
                    if (kvp.Value != placeHolderItem)
                    {
                        slotAndItem += $"\n {kvp.Key}: {itemName} ||";
                    }
                    else
                    {
                        slotAndItem += $"\n {kvp.Key}: None ||";
                    }
                }
            }
        }
        Debug.Log("dict has no key playerStats");
        return slotAndItem;
    }
}