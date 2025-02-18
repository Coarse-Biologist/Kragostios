using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;

public class ModdedItems
{

    #region class variables
    public Dictionary<Item_SO, Dictionary<ItemVars, int>> ItemIntMods = new Dictionary<Item_SO, Dictionary<ItemVars, int>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, string>> ItemStringMods = new Dictionary<Item_SO, Dictionary<ItemVars, string>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>> ItemResourceMods = new Dictionary<Item_SO, Dictionary<ItemVars, ResourceTypes>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, Elements>> ItemElementMods = new Dictionary<Item_SO, Dictionary<ItemVars, Elements>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>> ItemBuffMods = new Dictionary<Item_SO, Dictionary<ItemVars, List<Buffs>>>();
    public Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>> ItemDebuffMods = new Dictionary<Item_SO, Dictionary<ItemVars, List<Debuffs>>>();

    #endregion

    #region modify variables
    public void ModifyStringAttribute(Item_SO item, ItemVars var, string newValue) // can be used for name and description
    {
        if (ItemStringMods.TryGetValue(item, out Dictionary<ItemVars, string> stringModDict))
        {
            if (stringModDict.TryGetValue(var, out string previousValue))
            {
                stringModDict[var] = newValue;
            }
            else stringModDict.TryAdd(var, newValue);
        }
        else
        {
            ItemStringMods.TryAdd(item, new Dictionary<ItemVars, string>());
            stringModDict.TryAdd(var, newValue);
        }
    }
    public void ModifyIntAttribute(Item_SO item, ItemVars var, int newValue) // can be used for name and description
    {
        if (ItemIntMods.TryGetValue(item, out Dictionary<ItemVars, int> intModDict))
        {
            if (intModDict.TryGetValue(var, out int previousValue))
            {
                intModDict[var] = newValue;
            }
            else intModDict.TryAdd(var, newValue);
        }
        else
        {
            ItemIntMods.TryAdd(item, new Dictionary<ItemVars, int>());
            intModDict.TryAdd(var, newValue);
        }
    }
    public void ModifyBuffsAttribute(Item_SO item, ItemVars var, Buffs buff) // can be used for name and description
    {
        if (ItemBuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Buffs>> buffModDict))
        {
            if (buffModDict.TryGetValue(var, out List<Buffs> previousBuffsList))
            {
                buffModDict[var].Add(buff);
            }
            else buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
        else
        {
            ItemBuffMods.TryAdd(item, new Dictionary<ItemVars, List<Buffs>>());
            buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
    }
    public void ModifyDebuffsAttribute(Item_SO item, ItemVars var, Debuffs debuff) // can be used for name and description
    {
        if (ItemDebuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Debuffs>> debuffModDict))
        {
            if (debuffModDict.TryGetValue(var, out List<Debuffs> previousdebuffsList))
            {
                debuffModDict[var].Add(debuff);
            }
            else debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
        else
        {
            ItemDebuffMods.TryAdd(item, new Dictionary<ItemVars, List<Debuffs>>());
            debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
    }
    public void ModifyResourceAttribute(Item_SO item, ItemVars var, ResourceTypes resource) // can be used for name and description
    {
        if (ItemResourceMods.TryGetValue(item, out Dictionary<ItemVars, ResourceTypes> resourceModDict))
        {
            if (resourceModDict.TryGetValue(var, out ResourceTypes previousValue))
            {
                resourceModDict[var] = resource;
            }
            else resourceModDict.TryAdd(var, resource);
        }
        else
        {
            ItemElementMods.TryAdd(item, new Dictionary<ItemVars, Elements>());
            resourceModDict.TryAdd(var, resource);
        }
    }
    public void ModifyElementAttribute(Item_SO item, ItemVars var, Elements element) // can be used for name and description
    {
        if (ItemElementMods.TryGetValue(item, out Dictionary<ItemVars, Elements> elementModDict))
        {
            if (elementModDict.TryGetValue(var, out Elements previousValue))
            {
                elementModDict[var] = element;
            }
            else elementModDict.TryAdd(var, element);
        }
        else
        {
            ItemElementMods.TryAdd(item, new Dictionary<ItemVars, Elements>());
            elementModDict.TryAdd(var, element);
        }
    }
    #endregion

    #region get modded values
    public int GetModdedInt(Item_SO item, ItemVars var)
    {

        if (ItemIntMods.TryGetValue(item, out Dictionary<ItemVars, int> innerDict))
        {
            if (innerDict.TryGetValue(var, out int value))
            {
                return value;
            }
            else return 0;
        }
        else return 0;
    }
    public string GetModdedString(Item_SO item, ItemVars var)
    {

        if (ItemStringMods.TryGetValue(item, out Dictionary<ItemVars, string> innerDict))
        {
            if (innerDict.TryGetValue(var, out string value))
            {
                return value;
            }
            else return "None";
        }
        else return "None";
    }
    public Elements GetModdedElement(Item_SO item, ItemVars var)
    {

        if (ItemElementMods.TryGetValue(item, out Dictionary<ItemVars, Elements> innerDict))
        {
            if (innerDict.TryGetValue(var, out Elements value))
            {
                return value;
            }
            else return Elements.None;
        }
        else return Elements.None;
    }
    public ResourceTypes GetModdedResource(Item_SO item, ItemVars var)
    {

        if (ItemResourceMods.TryGetValue(item, out Dictionary<ItemVars, ResourceTypes> innerDict))
        {
            if (innerDict.TryGetValue(var, out ResourceTypes value))
            {
                return value;
            }
            else return ResourceTypes.None;
        }
        else return ResourceTypes.None;
    }
    public List<Buffs> GetModdedBuffs(Item_SO item, ItemVars var)
    {

        if (ItemBuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Buffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Buffs> value))
            {
                return value;
            }
            else return new List<Buffs>();
        }
        else return new List<Buffs>();
    }
    public List<Debuffs> GetModdedDebuffs(Item_SO item, ItemVars var)
    {
        if (ItemDebuffMods.TryGetValue(item, out Dictionary<ItemVars, List<Debuffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Debuffs> value))
            {
                return value;
            }
            else return new List<Debuffs>();
        }
        else return new List<Debuffs>();
    }
    #endregion
    public void LoadData()
    {
        ModdedItemSaveData moddedData = SaveSystem.LoadModdedItemData();
        ItemIntMods = moddedData.ItemIntMods_SD;
        ItemStringMods = moddedData.ItemStringMods_SD;
        ItemResourceMods = moddedData.ItemResourceMods_SD;
        ItemElementMods = moddedData.ItemElementMods_SD;
        ItemBuffMods = moddedData.ItemBuffMods_SD;
        ItemDebuffMods = moddedData.ItemDebuffMods_SD;
    }
}