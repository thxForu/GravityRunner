using UnityEngine;
using UnityEngine.Serialization;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerTrans;

    public GameObject _rocket;
    public Transform[] PointBetwine;


    private void Start()
    {
        if (playerTrans == null)
            playerTrans = GameObject.FindWithTag("Player");
    }

    public void Spawn()
    {
        Instantiate(_rocket,
            new Vector3(playerTrans.transform.position.x + 25,
                Random.Range(playerTrans.transform.position.y - 1, playerTrans.transform.position.y + 1)),
            Quaternion.Euler(0, 0, 0));
    }
}