using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static int Coins;

    [SerializeField] private TMP_Text coinsCounter;

    public int GetCoins()
    {
        return Coins;
    }

    public void SetCoins(int value)
    {
        Coins = value;
    }
    public static void AddCoins(int value)
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