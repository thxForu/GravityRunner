using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class SawRotate : MonoBehaviour
{
    [FormerlySerializedAs("SpeedRotation")]
    public float speedRotation;

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, speedRotation, Space.World);
    }
}