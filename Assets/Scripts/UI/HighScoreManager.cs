using TMPro;
using UnityEngine;
using static UnityEngine.PlayerPrefs;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceCount, highScore;

    private void Start()
    {
        highScore.text = "HS: " + GetInt("HighScore").ToString("0");
    }

    private void FixedUpdate()
    {
        distanceCount.text = "Score: " + DistanceCounter.DistanceCount;
    }
}