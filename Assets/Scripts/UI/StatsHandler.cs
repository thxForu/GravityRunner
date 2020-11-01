using TMPro;
using UnityEngine;
using static UnityEngine.PlayerPrefs;

public class StatsHandler: MonoBehaviour
{
    [SerializeField] private TMP_Text distanceCount, highScore;
    [SerializeField] private TMP_Text bestScore, dodgeSaw, dodgeComet, maxCristal, totalCristal;
    private int _highScore;
    
    private void Start()
    {
        _highScore = GetInt(Constants.PLAYER_HIGH_SCORE);
        bestScore.text = "Best Score: " + _highScore;
        dodgeSaw.text = "Dodge Saw: " + GetInt(Constants.DODGE_SAWS);
        dodgeComet.text = "Dodge Comet: " + GetInt(Constants.DODGE_COMETS);
        maxCristal.text = "Max cristal collected: " + GetInt(Constants.MAX_MONEY);
        totalCristal.text = "Total cristal collected: " + GetInt(Constants.ALL_EARNED_MONEY);
        highScore.text = "HS: " + _highScore;
    }

    private void FixedUpdate()
    {
        distanceCount.text = "Score: " + DistanceCounter.DistanceCount;
    }
    
}