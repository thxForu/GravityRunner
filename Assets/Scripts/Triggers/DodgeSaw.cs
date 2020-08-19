using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSaw : MonoBehaviour
{
    public static int sawDodge;
    void Start()
    {
        GameEvents.current.OnMoneyChange += DodgeSawCounter;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            GameEvents.current.DodgeSaw();
    }

    void DodgeSawCounter()
    {
        sawDodge += 1;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnMoneyChange -= DodgeSawCounter;
    }
}
