using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class MoneyManager : MonoBehaviour
{
    public static int Coins;

    [SerializeField] private Text coinsCounter;

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