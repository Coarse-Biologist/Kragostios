using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using KragostiosAllEnums;
using System.Linq;

public class ModdedAbilities
{
    #region class variables
    public Dictionary<Ability_SO, Dictionary<AbilityVars, int>> AbilityIntMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, int>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, string>> AbilityStringMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, string>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>> AbilityResourceMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, ResourceTypes>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>> AbilityElementMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, Elements>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>> AbilityBuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Buffs>>>();
    public Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>> AbilityDebuffMods = new Dictionary<Ability_SO, Dictionary<AbilityVars, List<Debuffs>>>();
    #endregion

    #region modify variables

    public void ModifyIntAttribute(Ability_SO ability, AbilityVars var, int amount) // can be used for AbilityCost, HealValue, DamageValue, TurnDuration, Targets
    {
        if (AbilityIntMods.TryGetValue(ability, out Dictionary<AbilityVars, int> intModDict))
        {
            if (intModDict.TryGetValue(var, out int previousValue))
            {
                intModDict[var] += amount;
            }
            else intModDict.TryAdd(var, amount);
        }
        else
        {
            AbilityIntMods.TryAdd(ability, new Dictionary<AbilityVars, int>());
            intModDict.TryAdd(var, amount);
        }
    }
    public void ModifyStringAttribute(Ability_SO ability, AbilityVars var, string newString) // can be used for name and description
    {
        if (AbilityStringMods.TryGetValue(ability, out Dictionary<AbilityVars, string> stringModDict))
        {
            if (stringModDict.TryGetValue(var, out string previousValue))
            {
                stringModDict[var] = newString;
            }
            else stringModDict.TryAdd(var, newString);
        }
        else
        {
            AbilityStringMods.TryAdd(ability, new Dictionary<AbilityVars, string>());
            stringModDict.TryAdd(var, newString);
        }
    }
    public void ModifyResourceAttribute(Ability_SO ability, AbilityVars var, ResourceTypes resource) // can be used for name and description
    {
        if (AbilityResourceMods.TryGetValue(ability, out Dictionary<AbilityVars, ResourceTypes> resourceModDict))
        {
            if (resourceModDict.TryGetValue(var, out ResourceTypes previousResource))
            {
                resourceModDict[var] = resource;
            }
            else resourceModDict.TryAdd(var, resource);
        }
        else
        {
            AbilityResourceMods.TryAdd(ability, new Dictionary<AbilityVars, ResourceTypes>());
            resourceModDict.TryAdd(var, resource);
        }
    }
    public void ModifyElementAttribute(Ability_SO ability, AbilityVars var, Elements element) // can be used for name and description
    {
        if (AbilityElementMods.TryGetValue(ability, out Dictionary<AbilityVars, Elements> elementModDict))
        {
            if (elementModDict.TryGetValue(var, out Elements previousValue))
            {
                elementModDict[var] = element;
            }
            else elementModDict.TryAdd(var, element);
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            elementModDict.TryAdd(var, element);
        }
    }
    public void ModifyBuffsAttribute(Ability_SO ability, AbilityVars var, Buffs buff) // can be used for name and description
    {
        if (AbilityBuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Buffs>> buffModDict))
        {
            if (buffModDict.TryGetValue(var, out List<Buffs> previousBuffsList))
            {
                buffModDict[var].Add(buff);
            }
            else buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            buffModDict.TryAdd(var, new List<Buffs> { buff });
        }
    }
    public void ModifyDebuffsAttribute(Ability_SO ability, AbilityVars var, Debuffs debuff) // can be used for name and description
    {
        if (AbilityDebuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Debuffs>> debuffModDict))
        {
            if (debuffModDict.TryGetValue(var, out List<Debuffs> debuffsList))
            {
                debuffModDict[var].Add(debuff);
            }
            else debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
        else
        {
            AbilityElementMods.TryAdd(ability, new Dictionary<AbilityVars, Elements>());
            debuffModDict.TryAdd(var, new List<Debuffs> { debuff });
        }
    }
    #endregion

    #region get modded values

    public int GetModdedInt(Ability_SO ability, AbilityVars var)
    {

        if (AbilityIntMods.TryGetValue(ability, out Dictionary<AbilityVars, int> innerDict))
        {
            if (innerDict.TryGetValue(var, out int value))
            {
                return value;
            }
            else return 0;
        }
        else return 0;
    }
    public string GetModdedString(Ability_SO ability, AbilityVars var)
    {

        if (AbilityStringMods.TryGetValue(ability, out Dictionary<AbilityVars, string> innerDict))
        {
            if (innerDict.TryGetValue(var, out string value))
            {
                return value;
            }
            else return "None";
        }
        else return "None";
    }
    public Elements GetModdedElement(Ability_SO ability, AbilityVars var)
    {

        if (AbilityElementMods.TryGetValue(ability, out Dictionary<AbilityVars, Elements> innerDict))
        {
            if (innerDict.TryGetValue(var, out Elements value))
            {
                return value;
            }
            else return Elements.None;
        }
        else return Elements.None;
    }
    public ResourceTypes GetModdedResource(Ability_SO ability, AbilityVars var)
    {

        if (AbilityResourceMods.TryGetValue(ability, out Dictionary<AbilityVars, ResourceTypes> innerDict))
        {
            if (innerDict.TryGetValue(var, out ResourceTypes value))
            {
                return value;
            }
            else return ResourceTypes.None;
        }
        else return ResourceTypes.None;
    }

    public List<Buffs> GetModdedBuffs(Ability_SO ability, AbilityVars var)
    {

        if (AbilityBuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Buffs>> innerDict))
        {
            if (innerDict.TryGetValue(var, out List<Buffs> value))
            {
                return value;
            }
            else return new List<Buffs>();
        }
        else return new List<Buffs>();
    }

    public List<Debuffs> GetModdedDebuffs(Ability_SO ability, AbilityVars var)
    {

        if (AbilityDebuffMods.TryGetValue(ability, out Dictionary<AbilityVars, List<Debuffs>> innerDict))
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
        ModdedAbilitySaveData moddedData = SaveSystem.LoadModdedAbilityData();
        AbilityIntMods = moddedData.AbilityIntMods_SD;
        AbilityStringMods = moddedData.AbilityStringMods_SD;
        AbilityResourceMods = moddedData.AbilityResourceMods_SD;
        AbilityElementMods = moddedData.AbilityElementMods_SD;
        AbilityBuffMods = moddedData.AbilityBuffMods_SD;
        AbilityDebuffMods = moddedData.AbilityDebuffMods_SD;
    }
}
