using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject coinDetectorObj;

    private float _levelMagnet;

    private CapsuleCollider2D coinDetectorCollider2D;
    // Start is called before the first frame update
    private void Start()
    {
        coinDetectorObj = GameObject.Find("CoinDetector");
        coinDetectorCollider2D = coinDetectorObj.GetComponent<CapsuleCollider2D>();
        coinDetectorCollider2D.enabled = false;
        _levelMagnet = PlayerPrefs.GetInt(Constans.LEVEL_MAGNET);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            StartCoroutine(ActivateCoin(10));
        }
        
    }

    private IEnumerator ActivateCoin(float durationMagnet)
    {
        coinDetectorCollider2D.enabled = true;
        yield return new WaitForSeconds(durationMagnet);
        coinDetectorCollider2D.enabled = false;
    }
}
