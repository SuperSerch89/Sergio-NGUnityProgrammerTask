using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipLevelManager : LevelManager
{
    [SerializeField] PlayerDetector playerDetectorExitBase = null;
    [SerializeField] PlayerDetector playerDetectorEnterBase = null;
    [SerializeField] private CinemachineCamera baseCam = null;
    [SerializeField] private CinemachineCamera followCam = null;
    [SerializeField] private GameObject door = null;

    private GameMode currentGameMode = GameMode.None;
    private int newPriority = 15;
    private CinemachineCamera currentCam = null;

    #region Overriden Methods
    public override void StartScene()
    {
        Debug.Log("Starting spaceship scene");
        base.StartScene();
        SetBaseGameMode();
        playerDetectorExitBase.OnPlayerDetected += SetFlyingGameMode;
        playerDetectorEnterBase.OnPlayerDetected += SetBaseGameMode;
    }
    #endregion
    private void SetBaseGameMode()
    {
        if (currentGameMode == GameMode.Base) { return; }
        currentGameMode = GameMode.Base;
        if (currentCam != null) { currentCam.Priority = 0; }
        currentCam = baseCam;
        currentCam.Priority = newPriority;
        door.gameObject.SetActive(true);
        playerDetectorEnterBase.gameObject.SetActive(false);
        playerDetectorExitBase.gameObject.SetActive(true);
        MouseController.Instance.ChangeFlyMode(MouseController.MovementMode.Normal);
    }
    private void SetFlyingGameMode()
    {
        if (currentGameMode == GameMode.Flying) { return; }
        currentGameMode = GameMode.Flying;
        if (currentCam != null) { currentCam.Priority = 0; }
        currentCam = followCam;
        currentCam.Priority = newPriority;
        door.gameObject.SetActive(false);
        playerDetectorEnterBase.gameObject.SetActive(true);
        playerDetectorExitBase.gameObject.SetActive(false);
        MouseController.Instance.ChangeFlyMode(MouseController.MovementMode.Jetpack);
    }

    public enum GameMode { None, Base, Flying }
}
