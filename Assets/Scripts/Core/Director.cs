using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Director : MonoBehaviour
{
    private readonly bool _canSpawnCoins = true;

    private readonly bool _canSpawnRockets = true;
    [FormerlySerializedAs("ChanceSaw")] public int chanceSaw;

    public int increaseDifficulty = -100;

    [FormerlySerializedAs("SpawnCoinsEvent")]
    public UnityEvent spawnCoinsEvent;

    [FormerlySerializedAs("SpawnRocketEvent")]
    public UnityEvent spawnRocketEvent;

    public int timeForSpawn = 35; //average time when something must happening 35 seconds

    private void Start()
    {
        StartCoroutine(SpawnRockets());
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnRockets()
    {
        while (true)
        {
            if (_canSpawnRockets)
                spawnRocketEvent.Invoke();
            yield return new WaitForSeconds(Random.Range(1, timeForSpawn));
        }
    }

    private IEnumerator SpawnCoins()
    {
        while (true)
        {
            if (_canSpawnCoins)
                spawnCoinsEvent.Invoke();
            yield return new WaitForSeconds(Random.Range(1, timeForSpawn));
        }
    }

    public int GetChanceSaw(int minChanceSaw = -100, int maxChanceSaw = 100, int changeChanceSaw = 0)
    {
        increaseDifficulty = Random.Range(0, (int) DistanceCounter.DistanceCount / 100);
        chanceSaw = Random.Range(minChanceSaw + changeChanceSaw + increaseDifficulty,
            maxChanceSaw + increaseDifficulty);
        return chanceSaw;
    }
}