using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject coinDetectorObj;

    private float _levelMagnet;
    public GameObject MagnetImage;
    private GameObject magnetUiPosition;
    
    private SpriteRenderer sprite;
    private CapsuleCollider2D coinDetectorCollider2D;

    private void Start()
    {
        
        coinDetectorObj = GameObject.Find("CoinDetector");
        magnetUiPosition = GameObject.Find("MagnetUIPosition");

        sprite = GetComponent<SpriteRenderer>();
        coinDetectorCollider2D = coinDetectorObj.GetComponent<CapsuleCollider2D>();
        coinDetectorCollider2D.enabled = false;
        _levelMagnet = PlayerPrefs.GetInt(Constants.LEVEL_MAGNET);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sprite.color = Color.clear;
            StartCoroutine(ActivateCoin(7+(3*_levelMagnet)));
        }
    }

    private IEnumerator ActivateCoin(float durationMagnet)
    {
        coinDetectorCollider2D.enabled = true;
        var newMagnetImage = Instantiate (MagnetImage, new Vector2(magnetUiPosition.transform.position.x, magnetUiPosition.transform.position.y), Quaternion.identity); 
        newMagnetImage.transform.SetParent(magnetUiPosition.transform);
        yield return new WaitForSeconds(durationMagnet);
        Destroy(newMagnetImage.gameObject);
        Destroy(gameObject);
        coinDetectorCollider2D.enabled = false;
    }
}
