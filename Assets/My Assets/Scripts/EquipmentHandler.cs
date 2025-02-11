using System;
using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using Unity.VisualScripting;

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
            AddCharToEquipmentDict(PlayerStats);
            playerDictAdded = true;
        }
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
                Debug.Log($"You want to unequip {item.name} of slot {slot}");
                Unequip(stats, slot);                   // clear it
            }
            // equip item in newly cleared slot
        }
        EquipItem(stats, equipSlot, item);

    }

    public void Unequip(StatsHandler stats, ItemSlot slot)
    {
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        if (equipment.TryGetValue(slot, out Item_SO dictValue))
        {
            equipment[slot] = placeHolderItem;
        }
        else equipment.TryAdd(slot, placeHolderItem);
    }

    public void EquipItem(StatsHandler stats, ItemSlot slot, Item_SO item)
    {
        Debug.Log($"You want to equip {item.ItemName}");
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        if (equipment[slot] != placeHolderItem)
        {
            equipment[slot] = item;
        }

    }



}