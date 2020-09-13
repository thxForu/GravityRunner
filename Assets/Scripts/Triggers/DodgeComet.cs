using UnityEngine;

public class DodgeComet : MonoBehaviour
{
    public static int CountDodge;
    private bool canTriggered;

    private void OnEnable()
    {
        canTriggered = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canTriggered)
        {
            DodgeCometCounter();
            Debug.Log("Comet COUNTER");
            canTriggered = false;
        }
    }

    public static int DodgeCometCounter()
    {
        return CountDodge += 1;
    }

    private void OnDisable()
    {
        canTriggered = true;
    }
}
