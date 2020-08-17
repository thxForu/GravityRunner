using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    public GameObject diePanel;
    public Text hightScoreText, scoreText, dieCoinsText, totalCoinsText;
    
    
    private MoneyManager _moneyManager;
    private Camera _camera;
    private bool _died;
    private int _deathPoint, _deathCoins;
    
    private void Start()
    {
        _moneyManager = GetComponent<MoneyManager>();
        _camera = Camera.main;
    }

    public void PlayerDie()
    {
        if (_died == false)
        {
            _deathPoint = DistanceCounter.DistanceCount; //Point when player Die

            _deathCoins = _moneyManager.GetCoins(); //coins when player Die

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _deathCoins);
            if (_deathPoint > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", _deathPoint);
            _camera.orthographicSize = 3.6f;
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
        SceneManager.LoadScene((SceneManager.GetActiveScene().name));
        Time.timeScale = 1;
    }
}