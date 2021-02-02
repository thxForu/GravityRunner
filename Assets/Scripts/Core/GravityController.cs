using System.Collections;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public static float timeStart;
    
    [SerializeField] private Animator animator;
    [SerializeField] private float secondsDivideBy = 7;
    [SerializeField] private float thrust = 2.0f;
    [SerializeField] private float microThrust = 0.1f;

    private Rigidbody2D _rb;
    private bool _top;
    private bool _canThrust;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ChangeGravity(0));
    }
#endif

    public void StartChangeGravity()
    {
        StartCoroutine(ChangeGravity(timeStart/secondsDivideBy));
    }

    private IEnumerator ChangeGravity(float timeForChangeRotate)
    {
        _canThrust = false;
        _rb.gravityScale *= -1;

        if (timeStart >= 0.2)
        {
            if (_top)
                _rb.AddForce(transform.up * (-1 * (1 + timeStart) * thrust), ForceMode2D.Impulse);
            else
                _rb.AddForce(transform.up * ((1 + timeStart) * thrust), ForceMode2D.Impulse);

            _canThrust = true;
        }

        if (!_canThrust)
        {
            if (_top)
                _rb.AddForce(transform.up * (-1 * microThrust), ForceMode2D.Impulse);
            else
                _rb.AddForce(transform.up * microThrust, ForceMode2D.Impulse);
        }    

        yield return new WaitForSeconds(timeForChangeRotate);
        Rotate();

        _top = !_top;

        timeStart = 0;
        yield return null;
    }
    private void Rotate()
    {
        transform.localScale *= new Vector2(1,-1);
    }
}
