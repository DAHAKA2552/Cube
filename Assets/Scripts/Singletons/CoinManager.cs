using System.Collections.Generic;
using UnityEngine;

public class CoinManager : SingletonMonoBehaviour<CoinManager>
{
    int currentSublevelIndex = 0;
    int amountCoins = 0;

    List<List<Coin>> coins = new List<List<Coin>>();
    GameObject[] sublevels;


    void Start()
    {
        amountCoins = 0;

        UIManager.Instance.SetCoinsTo(amountCoins);
    }


    public void AddCoin(Coin coin)
    {
        coins[currentSublevelIndex].Add(coin);

        amountCoins++;
        UIManager.Instance.SetCoinsTo(amountCoins);
    }


    public void NextLevel(GameObject[] sublevels)
    {
        this.sublevels = sublevels;
        currentSublevelIndex = 0;

        if (coins != null)
        {
            foreach (List<Coin> list in coins)
            {
                list.Clear();
            }

            coins.Clear();
        }

        for (int i = 0; i < sublevels.Length; i++)
        {
            coins.Add(new List<Coin>());
        }
    }


    public void NextSublevel()
    {
        currentSublevelIndex++;
    }


    public List<Transform> GetCoins()
    {
        List<Transform> result = new List<Transform>();

        foreach (Coin c in coins[currentSublevelIndex])
        {
            result.Add(c.transform);
        }

        return result;
    }
}
