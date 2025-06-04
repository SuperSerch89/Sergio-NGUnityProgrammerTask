using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipLevelManager : LevelManager
{
    [Header("ObjectReferences")]
    [SerializeField] private PlayerDetector playerDetectorExitBase = null;
    [SerializeField] private PlayerDetector playerDetectorEnterBase = null;
    [SerializeField] private GameObject door = null;
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera baseCam = null;
    [SerializeField] private CinemachineCamera followCam = null;
    [Header("Audio")]
    [SerializeField] private AudioClip baseMusic = null;
    [SerializeField] private AudioClip spaceMusic = null;
    [SerializeField] private AudioClip grabSfx = null;
    [SerializeField] private AudioClip dropSfx = null;
    [SerializeField] private AudioClip submitSfx = null;


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
    public override void PlaySFX(SFX sfxToPlay)
    {
        AudioClip selectedClip = null;
        switch (sfxToPlay)
        {
            case SFX.grab:
                selectedClip = grabSfx;
                break;
            case SFX.drop:
                selectedClip = dropSfx;
                break;
            case SFX.button:
                selectedClip = submitSfx;
                break;
        }
        SoundManager.Instance.PlaySFX(selectedClip);
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
        SoundManager.Instance.PlayMusic(baseMusic);
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
        SoundManager.Instance.PlayMusic(spaceMusic);
    }

    public enum GameMode { None, Base, Flying }
}
public enum SFX
{
    empty = 0,
    grab,
    drop,
    button,
}