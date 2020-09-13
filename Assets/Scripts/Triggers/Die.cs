using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Die : MonoBehaviour
{
    public GameObject diePanel, questPanel, dieStatsPanel, homeCanvas, restartButton, continueButton;
    public TMP_Text highScoreText, scoreText, dieCoinsText, totalCoinsText;

    private QuestHandler _questHandler;
    private MoneyManager _moneyManager;
    private QuestManager _questManager;
    private Camera _camera;
    private bool _died;
    private int _deathPoint, _deathCrystals, _dodgeSaw, _dodgeComet, _allEarnedCrystals;
    
    private void Start()
    {
        _moneyManager = GetComponent<MoneyManager>();
        _questHandler = GetComponent<QuestHandler>();
        _questManager = GetComponent<QuestManager>();
        _camera = Camera.main;
        
        if (_questHandler.CheckAll())
        {
            print("setRandom");
            _questManager.SetRandomQuest();
        }
        
    }

    public void PlayerDie()
    {
        if (_died == false)
        {
            SaveData();
            _camera.orthographicSize = 3.6f;
            
            _questHandler.QuestCheck();
            if (_questHandler.CheckAll())
                _questManager.SetRandomQuest();
            
            Time.timeScale = 0;
            
            ShowDiePanel();
            _died = true;
        }
    }

    private void SaveData()
    {
        _deathPoint = DistanceCounter.DistanceCount; //Point when player Die
        _dodgeSaw = DodgeSaw.DodgeSawCounter();
        _dodgeComet = DodgeComet.DodgeCometCounter();
        _deathCrystals = _moneyManager.GetCoins(); //coins when player Die
        int maxCristalCollected = _deathCrystals > PlayerPrefs.GetInt(Constans.MAX_MONEY)? _deathCrystals:PlayerPrefs.GetInt(Constans.MAX_MONEY);
        _allEarnedCrystals = PlayerPrefs.GetInt(Constans.ALL_EARNED_MONEY);
        if (_deathPoint > PlayerPrefs.GetInt(Constans.PLAYER_HIGH_SCORE))
            PlayerPrefs.SetInt(Constans.PLAYER_HIGH_SCORE, _deathPoint);
        
        PlayerPrefs.SetInt(Constans.MAX_MONEY,maxCristalCollected);
        PlayerPrefs.SetInt(Constans.ALL_EARNED_MONEY,_allEarnedCrystals+_deathCrystals);
        PlayerPrefs.SetInt(Constans.CURRENT_MONEY, PlayerPrefs.GetInt(Constans.CURRENT_MONEY) + _deathCrystals);
        PlayerPrefs.SetInt(Constans.DODGE_SAWS,PlayerPrefs.GetInt(Constans.DODGE_SAWS)+_dodgeSaw);
        PlayerPrefs.SetInt(Constans.DODGE_COMETS,PlayerPrefs.GetInt(Constans.DODGE_COMETS)+_dodgeComet);
    }
    
    private void ShowDiePanel()
    {
        diePanel.SetActive(true);
        highScoreText.text = "High score: " + PlayerPrefs.GetInt(Constans.PLAYER_HIGH_SCORE);
        scoreText.text = "Score: "+(_deathPoint).ToString();
        dieCoinsText.text = "Collected: "+_deathCrystals.ToString();
        totalCoinsText.text = "Total crystals: "+PlayerPrefs.GetInt(Constans.CURRENT_MONEY).ToString();
    }

    public void ShowHomeCanvas()
    {
        homeCanvas.SetActive(true);
        restartButton.SetActive(true);
        continueButton.SetActive(false);
        diePanel.SetActive(false);
    }
    public void HideQuestPanel()
    {
        questPanel.SetActive(false);
        dieStatsPanel.SetActive(true);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().name));
        Time.timeScale = 1;
    }
}