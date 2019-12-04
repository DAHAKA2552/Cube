using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameScreen : MonoBehaviour
{
    [SerializeField] Button shopButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Image[] progressBlocks;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Text coins;
    [SerializeField] Text currentLevel;
    [SerializeField] Text nextLevel;


    void OnEnable()
    {
        //playButton.onClick.AddListener(UIManager.Instance.GoToLevelSelect);
    }


    void Awake()
    {
    }


    void OnDisable()
    {
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void ShowButtons()
    {
        shopButton.transform.DOLocalMoveX(183, 1);
        settingsButton.transform.DOLocalMoveX(183, 1);
    }


    public void HideButtons()
    {
        shopButton.transform.DOLocalMoveX(30 , 1);
        settingsButton.transform.DOLocalMoveX(30, 1);
    }


    public void SetCoinsTo(int amount)
    {
        coins.text = amount.ToString();
    }


    public void SetLevelTo(int amount)
    {
        currentLevel.text = amount.ToString();
        nextLevel.text = (amount + 1).ToString();
    }


    public void SetSublevelTo(int amount)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < amount)
            {
                progressBlocks[i].sprite = sprites[0];
            }
            else if (i == amount)
            {
                progressBlocks[i].sprite = sprites[1];
            }
            else if (i > amount)
            {
                progressBlocks[i].sprite = sprites[2];
            }
        }
    }
}
