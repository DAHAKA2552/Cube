using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    #region Fields

    [SerializeField] SublevelController sublevelControllerPrefab;
    [SerializeField] Camera mainCameraPrefab;
    [SerializeField] Light mainDirLightPrefab;
    [SerializeField] Vector3 cameraOffset;

    [SerializeField] Level[] levels;

    Light mainDirLight;
    SublevelController sublevelController;
    Level currentLevel;

    int currentLevelIndex;
    bool isLast = false;

    #endregion



    public Camera mainCamera { get; set; }



    #region Unity Lifecycle

    void Start()
    {
        mainCamera = Instantiate(mainCameraPrefab, transform);
        mainDirLight = Instantiate(mainDirLightPrefab, transform);

        sublevelController = Instantiate(sublevelControllerPrefab, transform);
        sublevelController.OnSublevelsComplete += NextLevel;

        UIManager.Instance.CanvasCamera = mainCamera;

        InputManager.Instance.IsEnabled = false;

        currentLevelIndex = -1;

        NextLevel();
    }

    #endregion



    #region Public Methods

    public void NextLevel()
    {
        LoadLevel(++currentLevelIndex);

        UIManager.Instance.ShowButtons();
        UIManager.Instance.SetLevelTo(currentLevelIndex);
    }


    public void LoadLevel(int index)
    {
        currentLevel = levels[currentLevelIndex];

        CoinManager.Instance.NextLevel(currentLevel.GetAllSublevels());

        sublevelController.Sublevels = currentLevel.GetAllSublevels();
        sublevelController.ShowFirst();
    }


    public void ShowFirstSub()
    {
        sublevelController.ShowFirst();
    }

    #endregion
}
