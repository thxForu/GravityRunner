using UnityEngine;

public class DistanceCounter : MonoBehaviour
{
    public static int DistanceCount;

    private void Start()
    {
        DistanceCount = 0;
    }

    private void FixedUpdate()
    {
        DistanceCount = (int)transform.position.x;
    }
}