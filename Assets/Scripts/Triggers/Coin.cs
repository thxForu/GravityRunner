using System;
using UnityEngine;


public class Coin : MonoBehaviour
{
    //TODO add listener or some shit\\\ this not normal with static;


    private void OnEnable()
    {
        if (transform.position.y< -3.33f)
        {
            transform.position = new Vector2(transform.position.x,transform.position.y + 3.33f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            MoneyManager.AddCoins(1);
            GameEvents.current.MoneyChange();
            Destroy(gameObject);
        }
        else if (col.tag.Equals("Platform") || col.tag.Equals("Saw"))
        {
            Destroy(gameObject);
        }
    }
    
}