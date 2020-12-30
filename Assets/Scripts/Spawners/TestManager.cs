#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Events;

public class TestManager : MonoBehaviour
{
    public Transform Player;

    [Header("Press C for spawn Coin")] public UnityEvent SpawnCoinsEvent;
    [Header("Press R for spawn Comet")] public UnityEvent SpawnRocketEvent;
    [Header("Press M for spawn PowerUps")] public UnityEvent SpawnPowerUpsEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SpawnRocketEvent.Invoke();
        if (Input.GetKeyDown(KeyCode.C)) SpawnCoinsEvent.Invoke();
        if (Input.GetKeyDown(KeyCode.M)) SpawnPowerUpsEvent.Invoke();
    }
}
#endif
