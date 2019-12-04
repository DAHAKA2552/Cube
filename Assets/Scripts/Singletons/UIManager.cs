using UnityEngine.UI;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] Canvas canvas;
    [SerializeField] Canvas backgroundCanvas;
    [SerializeField] GameScreen gameScreenPrefab;
    [SerializeField] DeathScreen deathScreenPrefab;
    [SerializeField] Image backgroundImage;

    Transform canvasTransform;
    GameScreen gameScreen;
    DeathScreen deathScreen;
    Image background;


    public Camera CanvasCamera
    {
        get
        {
            return canvas.worldCamera;
        }
        set
        {
            canvas.worldCamera = value;
            backgroundCanvas.worldCamera = value;
            backgroundCanvas.planeDistance = 50;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        canvasTransform = canvas.transform;

        gameScreen = Instantiate(gameScreenPrefab, canvasTransform);
        background = Instantiate(backgroundImage, backgroundCanvas.transform);

        deathScreen = Instantiate(deathScreenPrefab, canvasTransform);
    }


    public void SetCoinsTo(int amount)
    {
        gameScreen.SetCoinsTo(amount);
    }


    public void SetLevelTo(int amount)
    {
        gameScreen.SetLevelTo(amount);
    }


    public void SetSublevelTo(int amount)
    {
        gameScreen.SetSublevelTo(amount);
        deathScreen.SetSublevelTo(amount);
    }


    public void ShowButtons()
    {
        gameScreen.ShowButtons();
    }


    public void HideButtons()
    {
        gameScreen.HideButtons();
    }


    public void ShowDeathScreen()
    {
        deathScreen.Show();
    }
}
