using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
        var sprite = GetComponent<SpriteRenderer>().sprite;
        var texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        var deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
            deltaMovement.y * parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;
        if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
        {
            var offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}