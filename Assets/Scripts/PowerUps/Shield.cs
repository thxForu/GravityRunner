using System.Collections;
using UnityEngine;
public class Shield : MonoBehaviour
{
    //if u wanna read this, pls don't...
    
    public static bool isShilded;
    public static int pieceOfShield;
    
    [SerializeField] private float timeShield;
    [SerializeField] private GameObject ShieldSprite;
    
    private int levelShield; 
    private void Start()
    {
        pieceOfShield = 0;
        levelShield = PlayerPrefs.HasKey(Constants.LEVEL_SHIELD) ? PlayerPrefs.GetInt(Constants.LEVEL_SHIELD) : 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            giveShield();
        }
        if (pieceOfShield >= 1)
        {
            ShieldSprite.SetActive(true);
            isShilded = true;
        }
        else
        {
            ShieldSprite.SetActive(false);
            isShilded = false;
        }

    }
    
    private IEnumerator GiveShield(float timeShield)
    {
        pieceOfShield = pieceOfShield < 3 ? pieceOfShield+1 : pieceOfShield=3;
        yield return new WaitForSeconds(timeShield);
        pieceOfShield = pieceOfShield > 0 ? pieceOfShield-1 : pieceOfShield=0;
    }

    private void giveShield()
    {
        pieceOfShield += 1;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Shield"))
        {
            StartCoroutine(GiveShield(timeShield * levelShield));
            Destroy(other.gameObject);
        }
    }
}