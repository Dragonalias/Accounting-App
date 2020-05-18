using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/SavedMonths/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static bool SaveFileExists(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (File.Exists(filePath))
        {
            return true;
        }
        return false;
    }

    public static void Save(string saveName, string saveString)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (File.Exists(filePath))
        {
            Debug.LogError("Overriding a save!");
        }
        Debug.Log("Month saved to: " + filePath);
        File.WriteAllText(filePath, saveString);
    }
    public static string Load(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (!File.Exists(filePath))
        {
            Debug.LogError("No file found!");
            return null;
        }
        return File.ReadAllText(filePath);
    }
}
