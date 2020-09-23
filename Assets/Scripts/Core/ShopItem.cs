﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [Header("Type")]
    public ShopItems itemType;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemLevelText;

    [Header("Values")]
    [SerializeField] private string  itemName;
    [SerializeField] private int itemPrice = 100;
    [SerializeField] private int itemLevel = 1;
    [SerializeField] private int itemLevelMax;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(itemType.ToString()))
        {
            PlayerPrefs.SetInt(itemType.ToString(),1);
        }
        
        UpdateItemUI();
    }

    public void BuyItem()
    {
        var currentMoney = PlayerPrefs.GetInt(Constans.CURRENT_MONEY);
        if (itemLevel < itemLevelMax && currentMoney >= itemPrice*itemLevel)
        {
            PlayerPrefs.SetInt(itemType.ToString(), itemLevel);
            MoneyManager.Instance.AddCoinsAndSave(-itemPrice*itemLevel);
            itemLevel++;
            UpdateItemUI();

            ShopManager.Instance.UpdateMoneyInShopUI();
        }
        
    }

    private void UpdateItemUI()
    {
        itemLevel = PlayerPrefs.GetInt(itemType.ToString());
        itemLevelText.text = "Level. " + itemLevel;
        itemPriceText.text = itemPrice*itemLevel + " C.";

        if (itemLevel == itemLevelMax)
        {
            itemLevelText.text = "Max Level";
            itemPriceText.text = "";
        }
    }
}

public enum ShopItems
{
    Magnet, SomeOtherShit
}