using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public GameObject destroyPoint;

    private void Start()
    {
        destroyPoint = GameObject.Find("DestroyPoint");
    }

    private void FixedUpdate()
    {
        if (transform.position.x < destroyPoint.transform.position.x) gameObject.SetActive(false);
    }
}