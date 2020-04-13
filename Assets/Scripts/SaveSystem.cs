using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/SavedMonths";
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveName, string saveString)
    {
        string filePath = SAVE_FOLDER + "/" + saveName;
        if (Directory.Exists(filePath))
        {
            Debug.LogError("Overriding a save!");
        }
        File.WriteAllText(filePath, saveString);
    }
    public static string Load(string saveName)
    {
        string filePath = SAVE_FOLDER + "/" + saveName;
        if (!File.Exists(filePath))
        {
            Debug.LogError("No file found!");
            return null;
        }
        return File.ReadAllText(filePath);
    }
}
