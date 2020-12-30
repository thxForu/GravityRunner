using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Comet : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private UnityEvent CometSound;

    private Rigidbody2D _rb;
    private GameObject _setOffWarning;
    private GameObject _diePanel;
    private Camera _cam;
    private Image _warning;
    private bool _playSoundOnce = true;
    
    private void Start()
    {
        _setOffWarning = GameObject.Find("OffWarning");
        _warning = GameObject.Find("WarningOfRocket").GetComponent<Image>();
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _diePanel = GameObject.Find("GameController");
    }
    
    private void FixedUpdate()
    {
        WarningSign();
        
        _rb.AddForce(transform.right * -1 * thrust);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (Shield.isShilded == false)
            {
                _diePanel.GetComponent<Die>().PlayerDie();
            }

            Shield.pieceOfShield -= 1;
            Shield.isShilded = false;
            Destroy(gameObject);
        }
    }

    private void WarningSign()
    {
        var warningColorAlpha = _warning.color;
        if (transform.position.x < _setOffWarning.transform.position.x)
        {
            warningColorAlpha.a = 0;
            _warning.color = warningColorAlpha;
            if (_playSoundOnce)
            {
                CometSound.Invoke();
                _playSoundOnce = false;
            }
        }
        else
        {
            warningColorAlpha.a = 1;
            _warning.color = warningColorAlpha;
            
        }
        
        Vector2 namePose = new Vector2(_cam.pixelWidth-40,_cam.WorldToScreenPoint(transform.position).y);
        _warning.transform.position = namePose;
    }
}