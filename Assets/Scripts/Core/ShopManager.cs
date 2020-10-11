using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    [SerializeField] private TextMeshProUGUI moneyInShopText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateMoneyInShopUI();
    }
    public void UpdateMoneyInShopUI()
    {
        moneyInShopText.text = PlayerPrefs.GetInt(Constans.CURRENT_MONEY).ToString();
    }
}
