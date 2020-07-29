﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    public GameObject diePanel;
    public Text hightScoreText;
    public Text scoreText;
    public Text dieCoinsText;
    public Text totalCoinsText;
    
    
    private MoneyManager _moneyManager;
    private bool _died;
    private int _deathPoint;
    private int _deathCoins;

    private void Start()
    {
        _moneyManager = GetComponent<MoneyManager>();
    }

    public void PlayerDie()
    {
        if (_died == false)
        {
            _deathPoint = DistanceCounter.DistanceCount; //Point when player Die

            _deathCoins = _moneyManager.GetCoins(); //coins when player Die

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _deathCoins);
            if (_deathPoint > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", _deathPoint);

            _died = true;
            
            Time.timeScale = 0;
            ShowDiePanel();
        }
    }

    public void ShowDiePanel()
    {
        diePanel.SetActive(true);
        hightScoreText.text = "High Score:" + PlayerPrefs.GetInt("HighScore");
        scoreText.text = "Score:"+(_deathPoint).ToString();
        dieCoinsText.text = "Coins:"+_deathCoins.ToString();
        totalCoinsText.text = "Total coins:"+PlayerPrefs.GetInt("Money").ToString();
    }

    public void Reset()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }
}