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
        _highScore = GetInt(Constans.PLAYER_HIGH_SCORE);
        bestScore.text = "Best Score: " + _highScore;
        dodgeSaw.text = "Dodge Saw: " + GetInt(Constans.DODGE_SAWS);
        dodgeComet.text = "Dodge Comet: " + GetInt(Constans.DODGE_COMETS);
        maxCristal.text = "Max cristal collected: " + GetInt(Constans.MAX_MONEY);
        totalCristal.text = "Total cristal collected: " + GetInt(Constans.ALL_EARNED_MONEY);
        highScore.text = "HS: " + _highScore;
    }

    private void FixedUpdate()
    {
        distanceCount.text = "Score: " + DistanceCounter.DistanceCount;
    }
    
}