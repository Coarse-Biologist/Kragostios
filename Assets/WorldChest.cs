using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using KragostiosAllEnums;
using System;

public static class WorldChest
{
    // Dictionary to store loaded ScriptableObjects
    public static Dictionary<string, Item_SO> allItems = new Dictionary<string, Item_SO>();
    public static List<Item_SO> allItemsList = new List<Item_SO>();


    // List of addresses to load (manually assigned or from an external source)
    public static List<string> allAddresses = new List<string> { "PlaceHolder", "Sword", "Sword 1", "Sword 3", "Sword 4", "Sword 5", "Sword 6", "Sword 7", "Sword 8" };



    public static void LoadItems(List<string> addressType)
    {
        foreach (string address in addressType)
        {
            Addressables.LoadAssetAsync<Item_SO>("Assets/My Assets/Addressables/Items/" + address + ".asset").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Item_SO loadedSO = handle.Result;
                    if (!allItems.ContainsKey(address))
                    {
                        allItems.Add(loadedSO.ItemName, loadedSO);

                        //Debug.Log($"Loaded: {address} which has name {loadedSO.ItemName} -----");
                    }
                    if (!allItemsList.Contains(loadedSO))
                    {
                        allItemsList.Add(loadedSO);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load ScriptableObject at address: {address}");
                }
            };
        }
    }

    // Example method to access a ScriptableObject
    public static Item_SO GetItem(string key)
    {
        return allItems.TryGetValue(key, out Item_SO so) ? so : null;
    }
    public static Item_SO GetItemFromName(string itemName)
    {
        if (allItems.TryGetValue(itemName, out Item_SO item))
        {
            //Debug.Log($"{itemName} has return item: {item}");
            return item;
        }
        else
        {
            //Debug.Log($"item: {itemName} not in allItems dict");
            return null;
        }
    }
    public static string GetItemName(Item_SO item)
    {
        return item.ItemName;
    }
    public static List<Item_SO> GetAllItems()
    {
        return new List<Item_SO>(allItems.Values.ToList());
    }
    public static List<Item_SO> GetAllItemsofRarity(Rarity desiredRarity)
    {
        List<Item_SO> items = new List<Item_SO>();
        foreach (Item_SO item in allItems.Values)
        {
            if (item.ItemRarity == desiredRarity)
            {
                items.Add(item);
            }
        }
        return new List<Item_SO>(items);
    }
    public static List<Item_SO> GetItemsOfType(ItemType desiredItemType)
    {
        List<Item_SO> items = new List<Item_SO>();
        foreach (Item_SO item in allItems.Values)
        {
            if (item.ItemType == desiredItemType)
            {
                items.Add(item);
            }
        }
        return new List<Item_SO>(items);
    }


    public static List<Item_SO> GetTraderItems(StatsHandler playerStats)
    {
        List<Item_SO> traderItems = new List<Item_SO>();
        foreach (Item_SO item in allItemsList)
        {
            //int c = Array.IndexOf.Enum.GetValues(typeof(Rarity), item.ItemRarity);
            //playerStats.characterLevel > 
            if (playerStats.characterLevel > (Convert.ToInt32(item.ItemRarity) * 3) || item.ItemRarity == Rarity.Common)
            {
                traderItems.Add(item);
            }
        }

        return traderItems;
    }
}

