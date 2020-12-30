using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    public Transform DiePointBottom;
    public Transform DiePointTop;

    public float moveSpeed;

    public UnityEvent OnDieEvent;
    private Vector3 playerPosition;
    private bool checkOnce = true;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(moveSpeed, _rb.velocity.y);

        if (transform.position.y < DiePointBottom.transform.position.y && checkOnce ||
            transform.position.y > DiePointTop.transform.position.y && checkOnce)
        {
            checkOnce = false;
            if (Shield.isShilded == false)
            {
                OnDieEvent.Invoke();
            }
            StartCoroutine(ReturnPosition());
            Shield.isShilded = false;
            Shield.pieceOfShield -= 1;
        }
    }

    private IEnumerator ReturnPosition()
    {
        Debug.Log(Shield.pieceOfShield);
        var temp = _rb.gravityScale;
        _rb.gravityScale = 0;
        LeanTween.moveX(gameObject, transform.position.x - 0.08f, 1f);
        LeanTween.moveY(gameObject, 0, 1f).setEaseInOutCubic();
        yield return new WaitForSeconds(1f);
        _rb.gravityScale = temp;
        checkOnce = true;
        Debug.Log(checkOnce);
    }
}