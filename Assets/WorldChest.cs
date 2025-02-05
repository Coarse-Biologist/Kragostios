using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using KragostiosAllEnums;

public class WorldChest : MonoBehaviour
{
    // Dictionary to store loaded ScriptableObjects
    private Dictionary<string, Item_SO> allItems = new Dictionary<string, Item_SO>();


    // List of addresses to load (manually assigned or from an external source)
    public List<string> allAddresses = new List<string> { "Sword", "Sword 1", "Sword 3" };


    void Start()
    {
        LoadItems(allAddresses, allItems);
    }

    private void LoadItems(List<string> addressType, Dictionary<string, Item_SO> destination)
    {
        foreach (string address in addressType)
        {
            Addressables.LoadAssetAsync<Item_SO>("Assets/My Assets/Addressables/Items/" + address + ".asset").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Item_SO loadedSO = handle.Result;
                    if (!destination.ContainsKey(address))
                    {
                        destination.Add(address, loadedSO);
                        Debug.Log($"Loaded: {address}");
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
    public Item_SO GetItem(string key)
    {
        return allItems.TryGetValue(key, out Item_SO so) ? so : null;
    }
    public List<Item_SO> GetAllItems()
    {
        return allItems.Values.ToList();
    }
    public List<Item_SO> GetAllItemsofRarity(Rarity desiredRarity)
    {
        List<Item_SO> items = new List<Item_SO>();
        foreach (Item_SO item in allItems.Values)
        {
            if (item.itemRarity == desiredRarity)
            {
                items.Add(item);
            }
        }
        return items;
    }
    public List<Item_SO> GetItemsOfType(ItemType desiredItemType)
    {
        List<Item_SO> items = new List<Item_SO>();
        foreach (Item_SO item in allItems.Values)
        {
            if (item.itemType == desiredItemType)
            {
                items.Add(item);
            }
        }
        return items;
    }
}

