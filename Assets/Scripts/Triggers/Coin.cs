using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float pointToMove;
    [SerializeField] private float timeToMove;
    private CoinMove _coinMove;
    
    private void OnEnable()
    {
        var currentPosition = transform.position;
        LeanTween.moveY(gameObject,currentPosition.y+pointToMove,timeToMove).setEaseInOutSine().setLoopPingPong();
        if (currentPosition.y < -3.33f)
        {
            new Vector2(currentPosition.x,currentPosition.y + 3.33f);
        }
        _coinMove = gameObject.GetComponent<CoinMove>();
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
            Destroy(gameObject);
        }
        else if (col.tag.Equals("Platform") || col.tag.Equals("Saw"))
        {
            Destroy(gameObject);
        }
    }
}