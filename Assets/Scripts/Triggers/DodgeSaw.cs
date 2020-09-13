using UnityEngine;

public class DodgeSaw : MonoBehaviour
{
    public static int CountDodge;
    private bool canTriggered;

    private void OnEnable()
    {
        canTriggered = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")&& canTriggered)
        {
            DodgeSawCounter();
            Debug.Log("SAW COUNTER");
            canTriggered = false;
        }
    }

    public static int  DodgeSawCounter()
    {
        return CountDodge += 1;
    }
    
    private void OnDisable()
    {
        canTriggered = true;
    }
}
