using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float distanceToMoveX;

    private Vector3 lastPosition;
    public PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        lastPosition = player.transform.position;
    }

    private void FixedUpdate()
    {
        distanceToMoveX = player.transform.position.x - lastPosition.x;

        transform.position = new Vector3(transform.position.x + distanceToMoveX, transform.position.y,
            transform.position.z);
        lastPosition = player.transform.position;
    }
}