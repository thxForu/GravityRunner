using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    public GameObject diePanel;
    public Text hightScoreText, scoreText, dieCoinsText, totalCoinsText;

    private QuestHandler _questHandler;
    private MoneyManager _moneyManager;
    private QuestManager _questManager;
    private Camera _camera;
    private bool _died;
    private int _deathPoint, _deathCoins;
    
    private void Start()
    {
        _moneyManager = GetComponent<MoneyManager>();
        _questHandler = GetComponent<QuestHandler>();
        _questManager = GetComponent<QuestManager>();
        _camera = Camera.main;
        
        if (_questManager.CheckAll())
        {
            print("setRandom");
            _questManager.SetRandomQuest();
        }
        
    }

    public void PlayerDie()
    {
        if (_died == false)
        {
            _deathPoint = DistanceCounter.DistanceCount; //Point when player Die

            _deathCoins = _moneyManager.GetCoins(); //coins when player Die

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + _deathCoins);
            _camera.orthographicSize = 3.6f;

            _questHandler.QuestCheck();
            if (_questHandler.CheckAll())
            {
                print("setRandom");
                _questManager.SetRandomQuest();
            }

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
        SceneManager.LoadScene((SceneManager.GetActiveScene().name));
        Time.timeScale = 1;
    }
}