using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinMove _coinMove;
    private void OnEnable()
    {
        if (transform.position.y < -3.33f)
        {
            transform.position = new Vector2(transform.position.x,transform.position.y + 3.33f);
        }
        _coinMove = gameObject.GetComponent<CoinMove>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag.Equals("CoinDetector"))
        {
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