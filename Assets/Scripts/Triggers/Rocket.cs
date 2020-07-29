using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float thrust;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * -1 * thrust);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            Destroy(gameObject);
        //Die.PlayerDie();
    }
}