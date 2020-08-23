#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Events;

public class TestManager : MonoBehaviour
{
    public Transform Player;

    [Header("Press C for spawn Coin")] public UnityEvent SpawnCoinsEvent;

    [Header("Press R for spawn Rocket")] public UnityEvent SpawnRocketEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SpawnRocketEvent.Invoke();
        if (Input.GetKeyDown(KeyCode.C)) SpawnCoinsEvent.Invoke();
    }

}
#endif
