using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Director : MonoBehaviour
{
    private bool _canSpawnCoins = true;

    private bool _canSpawnRockets = true;
    private bool _canSpawnPowerUps = true;

    public int chanceSaw;

    public int increaseDifficulty = -100;
    public int defaultDifficulty = -100;
    
    public UnityEvent spawnCoinsEvent;
    public UnityEvent spawnRocketEvent;
    public UnityEvent spawnPowerUpsEvent;

    public int timeForSpawn = 35; //average time when something must happening 35 seconds

    private void Start()
    {
        StartCoroutine(SpawnComets());
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnComets()
    {
        yield return new WaitForSeconds(15);
        
        if (_canSpawnRockets)
            spawnRocketEvent.Invoke(); 
        yield return new WaitForSeconds(Random.Range(1, timeForSpawn));
        StartCoroutine(SpawnComets());
    }

    private IEnumerator SpawnCoins()
    {
        if (_canSpawnCoins)
            spawnCoinsEvent.Invoke();
        yield return new WaitForSeconds(Random.Range(1, timeForSpawn));
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnPowerUps()
    {
        yield return new WaitForSeconds(7);
        if (_canSpawnPowerUps)
            spawnPowerUpsEvent.Invoke();
        yield return new WaitForSeconds(Random.Range(1, timeForSpawn));
        StartCoroutine(SpawnPowerUps());
    }

    private int GetDifficulty()
    {
        return DistanceCounter.DistanceCount / 15;
    }
    public int GetChanceSaw(int minChanceSaw = -100, int maxChanceSaw = 100, int changeChanceSaw = 0)
    {
        increaseDifficulty = defaultDifficulty + Random.Range(0, GetDifficulty());
        chanceSaw = Random.Range(minChanceSaw + changeChanceSaw + increaseDifficulty, maxChanceSaw);
        return chanceSaw;
    }
}