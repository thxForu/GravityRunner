using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeComet : MonoBehaviour
{
    public static int cometDodge;
    void Start()
    {
        GameEvents.current.OnMoneyChange += DodgeCometCounter;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            GameEvents.current.DodgeComet();
    }

    void DodgeCometCounter()
    {
        cometDodge += 1;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnMoneyChange -= DodgeCometCounter;
    }
}
