using UnityEngine;
using System.IO;
using System;
using System.Text;

public static class SaveSystem
{

    private static string path = Application.persistentDataPath + "/yan.txt";

    public static void SavePlayerState(CharacterManager player)
    {
        Debug.Log("SAVING STATE");

        SaveData data = new SaveData(player);

        string json = JsonUtility.ToJson(data);

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream stream = File.Create(path);
            byte[] byteJson = Encoding.UTF8.GetBytes(json);
            stream.Write(byteJson, 0, byteJson.Length);
            stream.Close();

        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
        }
        
    }

    public static SaveData LoadPlayerState()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            return data;
        }
        return null;
    }

    public static void DeleteSave()
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("File Deleted");
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        
    }
}
