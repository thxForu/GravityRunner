using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float thrust;
    private GameObject _diePanel;
    private Die _dieScript;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _diePanel = GameObject.Find("GameController");
        _dieScript = _diePanel.GetComponent<Die>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * -1 * thrust);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            _dieScript.PlayerDie();
        }
    }
}