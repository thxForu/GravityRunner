    using UnityEngine;

public class CoinMove : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private float moveSpeed;
    public float coinMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position =
            Vector3.MoveTowards(transform.position, _playerTransform.position, coinMoveSpeed * Time.deltaTime);
    }
}
