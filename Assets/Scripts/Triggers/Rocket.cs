using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float thrust;

    private Rigidbody2D _rb;
    private GameObject _setOffWarning;
    private GameObject _diePanel;
    private Camera _cam;
    public Image _warning;
    private Die _dieScript;
    private void Start()
    {
        _setOffWarning = GameObject.Find("OffWarning");
        _warning = GameObject.Find("WarningOfRocket").GetComponent<Image>();
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _diePanel = GameObject.Find("GameController");
        _dieScript = _diePanel.GetComponent<Die>();
    }
    
    private void FixedUpdate()
    {
        
        var warningColorAlpha = _warning.color;
        if (transform.position.x < _setOffWarning.transform.position.x)
        {
            warningColorAlpha.a = 0;
            _warning.color = warningColorAlpha;
        }
        else
        {
            warningColorAlpha.a = 1;
            _warning.color = warningColorAlpha;
        }
        
        _rb.AddForce(transform.right * -1 * thrust);
        
        Vector2 namePose = new Vector2(_cam.pixelWidth-40,_cam.WorldToScreenPoint(transform.position).y);
        _warning.transform.position = namePose;
        
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