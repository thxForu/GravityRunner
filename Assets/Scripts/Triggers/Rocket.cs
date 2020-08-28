using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float thrust;
    private GameObject diePanel;
    private Die dieScript;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        diePanel = GameObject.Find("GameController");
        dieScript = diePanel.GetComponent<Die>();
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
            dieScript.PlayerDie();
        }
    }
}