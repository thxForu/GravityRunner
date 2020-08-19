using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _distanceToMoveX;

    private Vector3 _lastPosition;
    public PlayerController player;

    private void Start()
    {
        if (player is null)
            player = FindObjectOfType<PlayerController>();
        _lastPosition = player.transform.position;
    }

    private void FixedUpdate()
    {
        _distanceToMoveX = player.transform.position.x - _lastPosition.x;

        transform.position = new Vector3(transform.position.x + _distanceToMoveX, transform.position.y,
            transform.position.z);
        _lastPosition = player.transform.position;
    }
}