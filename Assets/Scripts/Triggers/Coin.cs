using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private float pointToMove;
    [SerializeField] private float timeToMove;
    private AudioSource _onCoinPickup;
    private SpriteRenderer color;
    private CoinMove _coinMove;
    
    private void OnEnable()
    {
        
        var currentPosition = transform.position;
        LeanTween.moveY(gameObject,currentPosition.y+pointToMove,timeToMove).setEaseInOutSine().setLoopPingPong();
        if (currentPosition.y < -3.33f)
        {
            new Vector2(currentPosition.x,currentPosition.y + 3.33f);
        }
        _coinMove = GetComponent<CoinMove>();
        _onCoinPickup = GetComponent<AudioSource>();
        color = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.tag.Equals("CoinDetector"))
        {
            LeanTween.reset();
            _coinMove.enabled = true;
        }
        if (col.tag.Equals("Player"))
        {
            MoneyManager.Instance.AddCoins(1);
            GameEvents.current.MoneyChange();
            _onCoinPickup.pitch = Random.Range(1.2f,1.7f);
            _onCoinPickup.Play();
            color.color = new Color(0,0,0,0);
            Invoke(nameof(OnDisable),1);
        }
        else if (col.tag.Equals("Platform") || col.tag.Equals("Saw"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject,2);
    }
}