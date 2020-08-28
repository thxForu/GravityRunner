using System;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private GameObject diePanel;
    private Die dieScript;
    private void Start()
    {
        diePanel = GameObject.Find("GameController");
        dieScript = diePanel.GetComponent<Die>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            dieScript.PlayerDie();
        }
    }
}