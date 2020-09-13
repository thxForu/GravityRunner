using System;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private GameObject _diePanel;
    private Die _dieScript;
    private void Start()
    {
        _diePanel = GameObject.Find("GameController");
        _dieScript = _diePanel.GetComponent<Die>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            _dieScript.PlayerDie();
        }
    }
}