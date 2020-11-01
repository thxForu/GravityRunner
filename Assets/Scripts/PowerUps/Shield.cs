using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public static bool isShilded;

    [SerializeField] private float timeShield;
    [SerializeField] public GameObject ShieldSprite;

    private int levelShield; 
    private void Start()
    {
        if (PlayerPrefs.HasKey(Constants.LEVEL_SHIELD))
            levelShield = PlayerPrefs.GetInt(Constants.LEVEL_SHIELD);
        else
            levelShield = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shield"))
        {
            StartCoroutine(GiveShield(timeShield * levelShield));
            Destroy(other.gameObject);
        }
    }

    public IEnumerator GiveShield(float timeShield)
    {
        ShieldSprite.SetActive(true);
        isShilded = true;
        yield return new WaitForSeconds(timeShield);
        isShilded = false;
        ShieldSprite.SetActive(false);
    }
}