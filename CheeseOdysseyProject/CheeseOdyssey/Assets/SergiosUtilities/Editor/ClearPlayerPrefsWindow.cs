using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsWindow : EditorWindow
{
    [MenuItem("Tools/Clear Player Prefs")]
    public static void ShowWindow()
    {
        GetWindow<ClearPlayerPrefsWindow>("Clear Player Prefs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Clear Player Preferences", EditorStyles.boldLabel);

        if (GUILayout.Button("Clear Player Prefs"))
        {
            if (EditorUtility.DisplayDialog("Clear Player Prefs", "Are you sure you want to clear all Player Prefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("Player Prefs cleared.");
            }
        }
    }
}