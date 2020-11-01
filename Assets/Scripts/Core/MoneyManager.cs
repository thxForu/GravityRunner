using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static int Coins;
    public static MoneyManager Instance;

    [SerializeField] private TMP_Text coinsCounter;

    private void Awake()
    {
        Instance = this;
    }

    public int GetCoins()
    {
        return Coins;
    }

    public void SetCoins(int value)
    {
        Coins = value;
    }
    public void AddCoins(int value)
    {
        Coins += value;
    }

    public void AddCoinsAndSave(int value)
    {
        PlayerPrefs.SetInt(Constants.CURRENT_MONEY,PlayerPrefs.GetInt(Constants.CURRENT_MONEY)+value);
    }
    


    private void Start()
    {
        GameEvents.current.OnMoneyChange += UpdateCoins;
        SetCoins(0);
        UpdateCoins();
    }

    public void UpdateCoins()
    {
        coinsCounter.text = GetCoins().ToString();
    }
    

    private void OnDestroy()
    {
        GameEvents.current.OnMoneyChange -= UpdateCoins;
    }
}