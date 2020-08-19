using System;
using System.Collections;
using UnityEngine;

public class DistanceCounter : MonoBehaviour
{
    public static int DistanceCount;

    private void Start()
    {
        DistanceCount = 0;
        StartCoroutine(Counter());
    }

    private IEnumerator Counter()
    {
        DistanceCount = (int)transform.position.x;
        yield return new WaitForSeconds(0.2f);
    }
}