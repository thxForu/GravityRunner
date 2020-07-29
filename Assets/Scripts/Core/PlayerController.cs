using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Transform DiePointBottom;
    public Transform DiePointTop;

    public float moveSpeed;

    public UnityEvent OnDieEvent;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);

        if (transform.position.y < DiePointBottom.transform.position.y ||
            transform.position.y > DiePointTop.transform.position.y)
            //Debug.Log("Die");
            OnDieEvent.Invoke();
    }
}