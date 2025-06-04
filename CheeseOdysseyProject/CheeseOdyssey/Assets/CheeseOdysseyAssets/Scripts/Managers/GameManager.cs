using NicoUtilities;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : Singleton<GameManager>
{
    #region SerializableFields
    [SerializeField] private GameDataSO newFileGameData = null;
    [SerializeField][ReadOnly] private string saveFilePath = "";
    [SerializeField][ReadOnly] private GameData gameData;
    #endregion
    #region Accessors
    public GameData GameData { get { return gameData; } }
    #endregion
    #region Unity Life Cycle
    protected override void Awake()
    {
        base.Awake();
        SetupGameData();
    }
    private void Start() => LevelManager.Instance.StartScene();
    #endregion
    #region Time Methods
    public void StopTime() => Time.timeScale = 0f;
    public void ResumeTime() => Time.timeScale = 1f;
    #endregion
    #region Save/Load Methods
    private void SetupGameData()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        gameData = LoadGame();
        if (gameData == null)
        {
            gameData = Instantiate(newFileGameData).gameData;
            SaveGame();
        }
    }
    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(gameData, prettyPrint: true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Game saved to: {saveFilePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save game: {ex.Message}");
        }
    }
    private GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                GameData data = JsonUtility.FromJson<GameData>(json);
                Debug.Log("Game loaded.");
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load game: {ex.Message}");
                return new GameData();
            }
        }
        else
        {
            Debug.Log("No save file found.");
            return null;
        }
    }
    #endregion
}

[System.Serializable]
public class GameData
{
    public List<InventoryItemData> inventory = new List<InventoryItemData>();
}
