using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{

    public static void SavePlayer(StatsHandler playerStats)
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
}


