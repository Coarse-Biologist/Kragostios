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

    public static ModdedItemSaveData LoadModdedItemData()
    {
        string path = Application.persistentDataPath + "/moddedItems.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ModdedItemSaveData saveData = formatter.Deserialize(stream) as ModdedItemSaveData;
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

    public static ModdedAbilitySaveData LoadModdedAbilityData()
    {
        string path = Application.persistentDataPath + "/moddedAbilities.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ModdedAbilitySaveData saveData = formatter.Deserialize(stream) as ModdedAbilitySaveData;
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


    public static void SaveEquipmentData(EquipmentHandler equipmentHandler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/EquipmentData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        EquipmentData saveData = new EquipmentData(equipmentHandler);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static EquipmentData LoadEquipmentData()
    {
        string path = Application.persistentDataPath + "/EquipmentData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            EquipmentData saveData = formatter.Deserialize(stream) as EquipmentData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }
    public static void SavePlayerLocationData(TravelScript travelScript)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerLocationData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        LocationData saveData = new LocationData(travelScript);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static LocationData LoadPlayerLocationData()
    {
        string path = Application.persistentDataPath + "/playerLocationData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            LocationData saveData = formatter.Deserialize(stream) as LocationData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.Log($"file not found{path} -------------------");
            return null;
        }
    }

    public static void SaveAll(StatsHandler stats, Map map, TravelScript travelScript, AlchemyHandler alchemyHandler, ModdedAbilities moddedAbilities, ModdedItems moddedItems)
    {
        //SavePlayerData(stats);
        SaveMapData(map);
        SavePlayerLocationData(travelScript);
        //SaveAlchemyyData(alchemyHandler);
        //SaveModdedAbilityData(moddedAbilities);
        //SaveModdedItemData(moddedItems);
    }



}


