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

    public static bool SavePathExists(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        return File.Exists(filePath);
    }
    public static void Save(string saveName, string saveString)
    {
        string filePath = SAVE_FOLDER + saveName;

        Debug.Log("Month saved to: " + filePath);
        File.WriteAllText(filePath, saveString);
    }
    public static void SaveWithBackup(string saveName, string saveString)
    {
        string filePath = SAVE_FOLDER + saveName;
        string backupPath = filePath + ".backup";
        if (File.Exists(backupPath))
        {
            File.Delete(backupPath);
        }
        File.Move(filePath, backupPath);
        Debug.Log("file backed up: " + filePath);
        Save(saveName, saveString);
    }
    public static string Load(string saveName)
    {
        string filePath = SAVE_FOLDER + saveName;
        if (!File.Exists(filePath))
        {
            Debug.Log("No file found!");
            return null;
        }
        Debug.Log("Month loaded from: " + filePath);
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
