using UnityEngine;

public class SpaceshipLevelManager : LevelManager
{
    #region Overriden Methods
    public override void StartScene()
    {
        Debug.Log("Starting spaceship scene");
        base.StartScene();
    }
    #endregion
}
