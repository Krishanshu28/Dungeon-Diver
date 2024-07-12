using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class SaveSystem 
{
    public static void SavePlayer(RPlayer player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Saving data = new Saving().PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static Saving LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Saving data = formatter.Deserialize(stream) as Saving;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save not found");
            return null;
        }
    }
    public static void SaveVillage(Teleporter vildata)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/vil.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Saving villdata = new Saving().VillageData(vildata);

        formatter.Serialize(stream, villdata);
        stream.Close();
    }
    public static Saving LoadVillageData()
    {
        string path = Application.persistentDataPath + "/vil.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Saving data = formatter.Deserialize(stream) as Saving;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save not found");
            return null;
        }
    }
}
