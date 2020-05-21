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

    public static bool Save(string saveName, string saveString)
    {
        string filePath = SAVE_FOLDER + saveName;
        bool overriding = false;
        if (File.Exists(filePath))
        {
            Debug.Log("Overriding a save!");
            overriding = true;
        }
        Debug.Log("Month saved to: " + filePath);
        File.WriteAllText(filePath, saveString);
        return overriding;
    }
    public static string Load(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (!File.Exists(filePath))
        {
            Debug.Log("No file found!");
            return null;
        }
        return File.ReadAllText(filePath);
    }
    public static bool DeleteSave(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (!File.Exists(filePath))
        {
            Debug.Log("No file to delete!");
            return false;
        }
        File.Delete(filePath);
        return true;
    }
}
