using System;
using System.Collections.Generic;
using UnityEngine;
using KragostiosAllEnums;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using NUnit.Framework.Constraints;
using UnityEngine.InputSystem;
using System.Diagnostics;

public static class EquipmentHandler
{
    public static Item_SO placeHolderItem;
    public static List<ItemSlot> allItemSlots = Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>().ToList();
    public static Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>> allEquipmentDicts = new Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>>();

    public static Dictionary<string, Dictionary<ItemSlot, string>> allEquipmentDicts_save = new Dictionary<string, Dictionary<ItemSlot, string>>();

    private static StatsHandler PlayerStats;
    private static bool playerDictAdded = false;
    public static List<Item_SO> playerEquippedItems = new List<Item_SO>();


    public static void SetPlayerStats(StatsHandler playerStats)
    {
        PlayerStats = playerStats;
        if (!playerDictAdded)
        {
            KDebug.SeekBug($"Adding player stats ({playerStats}) to dict");
            AddCharToEquipmentDict(PlayerStats);
            //AddCharToEquipmentDict_Save(playerStats);
            playerDictAdded = true;
        }
        KDebug.SeekBug($"player dict added? = {playerDictAdded}");
    }

    private static void AddCharToEquipmentDict(StatsHandler stats)
    {
        Dictionary<ItemSlot, Item_SO> charEquipment = new Dictionary<ItemSlot, Item_SO>();
        UnityEngine.Debug.Log($"{stats}");
        allEquipmentDicts.TryAdd(stats, charEquipment);

        foreach (ItemSlot slot in allItemSlots)
        {
            charEquipment.Add(slot, placeHolderItem);
        }
    }
    private static void AddCharToEquipmentDict_Save(StatsHandler stats)
    {
        KDebug.SeekBug($"Creating savable dict for {stats.characterName}");
        Dictionary<ItemSlot, string> charEquipment = new Dictionary<ItemSlot, string>();
        allEquipmentDicts_save.TryAdd(stats.characterName, charEquipment);
        foreach (ItemSlot slot in allItemSlots)
        {
            charEquipment.TryAdd(slot, "PlaceHolder");
        }
    }
    public static Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>> ConvertLoadedAllDicts_Save(Dictionary<string, Dictionary<ItemSlot, string>> allDicts_save)
    {
        KDebug.SeekBug($"converting save data to allDicts scriptable object data");
        allEquipmentDicts = new Dictionary<StatsHandler, Dictionary<ItemSlot, Item_SO>>();
        foreach (KeyValuePair<string, Dictionary<ItemSlot, string>> kvp in allDicts_save)
        {
            StatsHandler stats = PlayerStats;
            KDebug.SeekBug($"stats = {stats}");
            AddCharToEquipmentDict(stats); // remake later correct. for now ill just only have the player in it
            //AddCharToEquipmentDict_Save(stats);

            foreach (KeyValuePair<ItemSlot, string> innerDict in kvp.Value)
            {
                Item_SO item = WorldChest.GetItemFromName(innerDict.Value);
                KDebug.SeekBug($"retrieving item : {item} for slot {innerDict.Key}");
                EquipItem(stats, innerDict.Key, item); // i think the name i using is not getting the correct (or perhaps any item) #todo
            }
        }
        return allEquipmentDicts;
    }



    public static void DecideEquipItem(StatsHandler stats, Item_SO item, ItemSlot slot = ItemSlot.None)
    {
        KDebug.SeekBug($"Slot of {item.ItemName} selected = {slot}");
        // get the correct equipment dict
        if (item.ItemType == ItemType.Weapon)               // check if its a weapon
        {
            if (slot == ItemSlot.TwoHands)         // check if its a two hander
            {
                KDebug.SeekBug($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.RightHand, ItemSlot.LeftHand, ItemSlot.TwoHands }, ItemSlot.TwoHands, item);
            }
            if (slot == ItemSlot.RightHand)
            {
                KDebug.SeekBug($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.RightHand, ItemSlot.TwoHands }, slot, item);
            }
            if (slot == ItemSlot.LeftHand)
            {
                KDebug.SeekBug($"Slot is {slot}. item = {item.ItemName}");
                HandleEquipItem(stats, new List<ItemSlot> { ItemSlot.LeftHand, ItemSlot.TwoHands }, slot, item);
            }
            //else HandleEquipItem(stats, new List<ItemSlot> { slot }, slot, item);
            // check weapon slot
            //checked if there was a previously equipped weapon in that slot
            //unequip previous weapon
            //equip weapon in slot
        }
    }
    private static void HandleEquipItem(StatsHandler stats, List<ItemSlot> unequipSlots, ItemSlot equipSlot, Item_SO item)
    {
        if (!allEquipmentDicts.TryGetValue(stats, out Dictionary<ItemSlot, Item_SO> dict))
        {
            KDebug.SeekBug($"{stats} is not present in the dict");
            AddCharToEquipmentDict(stats);
            //AddCharToEquipmentDict_Save(stats);
        }
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

    public static void Unequip(StatsHandler stats, ItemSlot slot)
    {
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];

        if (equipment.TryGetValue(slot, out Item_SO dictValue))
        {
            playerEquippedItems.Remove(dictValue);
            KDebug.SeekBug($"You want to unequip {dictValue} in slot {slot}");
            equipment[slot] = placeHolderItem;
        }
        else equipment.TryAdd(slot, placeHolderItem);
    }

    public static void EquipItem(StatsHandler stats, ItemSlot slot, Item_SO item)
    {
        playerEquippedItems.Add(item);
        KDebug.SeekBug($"You want to equip {item}");
        Dictionary<ItemSlot, Item_SO> equipment = allEquipmentDicts[stats];
        //Dictionary<ItemSlot, string> equipment_save = allEquipmentDicts_save[stats.characterName];

        if (equipment.TryGetValue(slot, out Item_SO previousWeapon))
        {
            KDebug.SeekBug($"{item.ItemName} is actually litterally being added to {slot}");
            equipment[slot] = item;
            //equipment_save[slot] = item.ItemName;
        }

    }

    public static string GetItemNameFromSlot(StatsHandler stats, ItemSlot slot)
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
        KDebug.SeekBug("dict has no key playerStats");
        return itemName;
    }

    public static Item_SO GetItemFromSlot(StatsHandler stats, ItemSlot slot)
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

    public static string GetAllSlotItems(StatsHandler stats)
    {

        string slotAndItem = "";
        if (allEquipmentDicts.TryGetValue(stats, out Dictionary<ItemSlot, Item_SO> charEquipment))
        {
            KDebug.SeekBug($"{charEquipment.Count} = num of slots in charEquipment");
            foreach (KeyValuePair<ItemSlot, Item_SO> kvp in charEquipment)
            {
                KDebug.SeekBug($"{kvp.Key} = slot name. {kvp.Value} = item in the slot");
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
        else
        {
            AddCharToEquipmentDict(stats);         //sweat change
            //AddCharToEquipmentDict_Save(stats);     //sweat change

        }
        KDebug.SeekBug("dict has no key playerStats");
        return slotAndItem;
    }

    public static List<Item_SO> GetAllEquippedItems(StatsHandler stats)
    {
        return playerEquippedItems;
    }
    public static List<Ability_SO> GetEquippedItemsWithAbilities(StatsHandler stats)
    {
        List<Ability_SO> itemAbilities = new List<Ability_SO>();
        foreach (Item_SO item in playerEquippedItems)
        {
            if (item.Ability != null)
            {
                itemAbilities.Add(item.Ability);
            }
        }
        return itemAbilities;
    }

    // public static Dictionary<string, Dictionary<ItemSlot, string>> ConvertEquipmentDictToSavableForm()
    // {
    //     allEquipmentDicts_save = new Dictionary<string, Dictionary<ItemSlot, string>>();
    //     foreach (KeyValuePair<StatsHandler, Dictionary<ItemSlot, Item_SO>> individualCharEquipmentDict in allEquipmentDicts)
    //     {
    //         StatsHandler stats = PlayerStats;
    //         AddCharToEquipmentDict_Save(stats);
    //         foreach (KeyValuePair<ItemSlot, Item_SO> innerDict in individualCharEquipmentDict.Value)
    //         {
    //             Dictionary<ItemSlot, Item_SO> equipmentDict = allEquipmentDicts[stats];
    //
    //             innerDict.TryAdd(equipmentDict.Key, equipmentDict.Value.ItemName);
    //             //KDebug.SeekBug($"retrieving item : {item} for slot {innerDict.Key}");
    //         }
    //     }
    //     return allEquipmentDicts_save;
    // }
    public static Dictionary<string, Dictionary<ItemSlot, string>> ConvertEquipmentDictToSavableForm()
    {
        KDebug.SeekBug($"converting data from scriptable object for to string form in ConvertequipmentDictToSavableForm method");
        allEquipmentDicts_save = new Dictionary<string, Dictionary<ItemSlot, string>>();

        foreach (KeyValuePair<StatsHandler, Dictionary<ItemSlot, Item_SO>> individualCharEquipmentDict in allEquipmentDicts)
        {
            StatsHandler stats = individualCharEquipmentDict.Key;
            string characterKey = stats.characterName; // Assuming StatsHandler has a 'name' property

            if (!allEquipmentDicts_save.ContainsKey(characterKey))
            {
                AddCharToEquipmentDict_Save(stats);
            }

            foreach (KeyValuePair<ItemSlot, Item_SO> innerDict in individualCharEquipmentDict.Value)
            {
                ItemSlot slot = innerDict.Key;
                Item_SO item = innerDict.Value;

                if (item != null)
                {
                    allEquipmentDicts_save[characterKey][slot] = item.ItemName; // Save item name as string
                }
            }

        }
        return allEquipmentDicts_save;
    }




    public static void LoadData()
    {
        KDebug.SeekBug($"Loading data in load data function where allEquipmentDicts is set based on resu;t of ConvertLoadedAllDicts_Save return");
        EquipmentData equipmentData = SaveSystem.LoadEquipmentData();
        allEquipmentDicts = ConvertLoadedAllDicts_Save(equipmentData.allEquipmentDicts_SD);

    }
}

// player equipment and allEquipmentdicts must be replaced with non-scriptable object data types