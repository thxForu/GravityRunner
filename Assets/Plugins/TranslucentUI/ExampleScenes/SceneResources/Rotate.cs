using UnityEngine;

namespace TranslucentUI
{
    public class Rotate : MonoBehaviour
    {
        private Vector3 eulerAngles;

        [Range(1f, 5f)] public float RotationSpeed = 3.0f;

        // Use this for initialization
        private void Start()
        {
            eulerAngles = transform.eulerAngles;
        }

        // Update is called once per frame
        private void Update()
        {
            eulerAngles.y += 10 * RotationSpeed * Time.deltaTime;
            transform.eulerAngles = eulerAngles;
        }
    }
}