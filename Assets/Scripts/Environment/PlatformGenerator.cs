using UnityEngine;

namespace Environment
{
    public class PlatformGenerator : MonoBehaviour
    {
        public Director Director;
        public PlatformManager[] platformsM;

        private GameObject _newPlatform;
        private float _newPlatformPositon;

        private int _platformSelector;
        private float[] _platformsWidth;
        public float  distanceMin, distBetween, distanceMax;

        public Transform generationPotint;

        public bool topPlatform;

        private void Start()
        {
            if (generationPotint == null) GameObject.Find("GenerationPoint");
            _platformsWidth = new float[platformsM.Length];

            for (var i = 0; i < platformsM.Length; i++)
                _platformsWidth[i] = platformsM[i].platform.GetComponent<BoxCollider2D>().size.x;
        }

        private void FixedUpdate()
        {
            if (transform.position.x < generationPotint.position.x)
            {
                 distBetween = Random.Range(distanceMin, distanceMax);

                _platformSelector = Random.Range(0, platformsM.Length);
                _newPlatformPositon = _platformsWidth[_platformSelector] / 2 + distBetween;
                transform.position = new Vector2(transform.position.x + _newPlatformPositon, transform.position.y);
                SpawnPlatform();
            }
        }

        public void SpawnPlatform()
        {
            var chanceSaw = Director.GetChanceSaw();

            if (topPlatform)
            {
                if (chanceSaw > 50)
                    _newPlatform = platformsM[_platformSelector].GetSaws();
                else
                    _newPlatform = platformsM[_platformSelector].GetPlatform();
                _newPlatform.transform.position = transform.position;
                _newPlatform.transform.rotation = transform.rotation;
                _newPlatform.SetActive(true);
            }
            else
            {
                if (chanceSaw > 50)
                    _newPlatform = platformsM[_platformSelector].GetSaws();
                else
                    _newPlatform = platformsM[_platformSelector].GetPlatform();
                _newPlatform.transform.position = transform.position;
                _newPlatform.transform.rotation = transform.rotation;
                _newPlatform.SetActive(true);
            }
        }
    }
}