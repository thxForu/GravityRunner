using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.PlayerPrefs;

public class HighScoreManager : MonoBehaviour
{
    [SerializeField] private Text distanceCount, highScore;

    private void Start()
    {
        highScore.text = "HS: " + GetInt("HighScore").ToString("0");
    }

    private void FixedUpdate()
    {
        distanceCount.text = "Score: " + DistanceCounter.DistanceCount;
    }
}