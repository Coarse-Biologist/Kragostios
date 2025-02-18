using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{

    public static void SavePlayerData(StatsHandler playerStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/adventurer.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerSaveData saveData = new PlayerSaveData(playerStats);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static PlayerSaveData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/adventurer.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerSaveData saveData = formatter.Deserialize(stream) as PlayerSaveData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }


    public static void SaveMapData(Map map)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/map.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        MapData saveData = new MapData(map);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static MapData LoadMapData()
    {
        string path = Application.persistentDataPath + "/map.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            MapData saveData = formatter.Deserialize(stream) as MapData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }

    public static void SaveModdedItemData(ModdedItems moddedItems)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/moddedItems.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        ModdedItemSaveData saveData = new ModdedItemSaveData(moddedItems);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static ModdedItems LoadModdedItemData()
    {
        string path = Application.persistentDataPath + "/moddedItems.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ModdedItems saveData = formatter.Deserialize(stream) as ModdedItems;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }

    public static void SaveModdedAbilityData(ModdedAbilities moddedAbilities)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/moddedAbilities.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        ModdedAbilitySaveData saveData = new ModdedAbilitySaveData(moddedAbilities);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static ModdedAbilities LoadModdedAbilityData()
    {
        string path = Application.persistentDataPath + "/moddedAbilities.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ModdedAbilities saveData = formatter.Deserialize(stream) as ModdedAbilities;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }

    public static void SaveAlchemyyData(AlchemyHandler alchemyHandler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/alchemyData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        AlchemyData saveData = new AlchemyData(alchemyHandler);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static AlchemyData LoadAlchemyData()
    {
        string path = Application.persistentDataPath + "/alchemyData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            AlchemyData saveData = formatter.Deserialize(stream) as AlchemyData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }

    public static void SaveAll(StatsHandler stats, Map map, AlchemyHandler alchemyHandler, ModdedAbilities moddedAbilities, ModdedItems moddedItems)
    {
        SavePlayerData(stats);
        SaveMapData(map);
        SaveAlchemyyData(alchemyHandler);
        SaveModdedAbilityData(moddedAbilities);
        SaveModdedItemData(moddedItems);
    }



}


