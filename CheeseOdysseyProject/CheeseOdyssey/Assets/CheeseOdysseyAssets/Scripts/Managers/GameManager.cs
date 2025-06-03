using NicoUtilities;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region SerializableFields
    [SerializeField] private GameData gameData = new GameData();
    #endregion
    #region Unity Life Cycle
    private void Start()
    {
        LevelManager.Instance.StartScene();
    }
    #endregion
    #region Public Methods
    public void StopTime() => Time.timeScale = 0f;
    public void ResumeTime() => Time.timeScale = 1f;
    #endregion
}

[System.Serializable]
public class GameData
{
    public List<InventoryItemData> inventory = new List<InventoryItemData>();
}