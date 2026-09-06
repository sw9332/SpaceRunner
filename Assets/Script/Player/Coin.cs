using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private static Coin instance = null;

    public static Coin Instance { get { return instance; } }

    public int coin = 0;
    public int coinSave = 0;

    public void SaveCoin()
    {
        PlayerPrefs.SetInt("Coin", coin);
    }

    public void AddCoin(int amount)
    {
        coin += amount;

        MissionManager missionManager = FindAnyObjectByType<MissionManager>();
        if (missionManager != null)
            missionManager.AddProgress(Mission.MissionType.CollectCoins, amount);
    }

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(this.gameObject); }
        else Destroy(this.gameObject);

        coinSave = PlayerPrefs.GetInt("Coin");
        coin = coinSave;
    }
}