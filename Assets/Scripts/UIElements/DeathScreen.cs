using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] Text timerSeconds;
    [SerializeField] Text sublevelIndex;
    [SerializeField] Image ring;
    [SerializeField] Button continueButton;
    [SerializeField] Button tryAgainButton;

    float timer = 5f;
    int currentSublevelIndex;


    void OnEnable()
    {
        tryAgainButton.onClick.AddListener(LevelManager.Instance.ShowFirstSub);
        tryAgainButton.onClick.AddListener(Hide);
    }


    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            int i = (int)timer;

            timerSeconds.text = i.ToString();
        }
        else
        {
            LevelManager.Instance.ShowFirstSub();
            Hide();
        }
    }


    public void Show()
    {
        gameObject.SetActive(true);
        timer = 5f;
        ring.fillAmount = 0f;
        ring.DOFillAmount(1, 5);

        sublevelIndex.text = currentSublevelIndex.ToString() + " OF 3 PASS";
    }


    public void Hide()
    {
        gameObject.SetActive(false);
        timer = 0.0f;
        ring.fillAmount = 1f;
    }


    public void SetSublevelTo(int amount)
    {
        currentSublevelIndex = amount;
    }
}
