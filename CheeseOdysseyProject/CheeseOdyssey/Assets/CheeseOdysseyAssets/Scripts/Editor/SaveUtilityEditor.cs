using UnityEditor;
using UnityEngine;
using System.IO;

public static class SaveUtilityEditor
{
    [MenuItem("Tools/Delete Save File")]
    public static void DeleteSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "gameData.json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted save file at: {path}");
        }
        else
        {
            Debug.LogWarning("No save file found to delete.");
        }
    }
}