using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerTrans;


    public List<GameObject> coins; //List of coins

    private void Start()
    {
        if (_playerTrans == null)
            _playerTrans = GameObject.FindWithTag("Player");
    }

    public void Spawn()
    {
        var coinsSelector = Random.Range(0, coins.Count);
        var position = _playerTrans.transform.position;
        Instantiate(coins[coinsSelector], new Vector3(position.x + 20, position.y), transform.rotation);
    }
}