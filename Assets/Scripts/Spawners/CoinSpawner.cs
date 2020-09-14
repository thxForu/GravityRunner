using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _playerTrans;
    
    public List<GameObject> prefabsForSpawn; //List of coins prefab

    private void Start()
    {
        if (_playerTrans == null)
            _playerTrans = GameObject.FindWithTag("Player");
    }

    public void Spawn()
    {
        var coinsSelector = Random.Range(0, prefabsForSpawn.Count);
        var position = _playerTrans.transform.position;
        Instantiate(prefabsForSpawn[coinsSelector], new Vector3(position.x + 20, position.y), transform.rotation);
    }
}