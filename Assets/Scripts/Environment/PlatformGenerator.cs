using UnityEngine;

namespace Environment
{
    public class PlatformGenerator : MonoBehaviour
    {
        public Director Director;
        public PlatformManager[] platformsM;

        private GameObject _newPlatform;
        public float _newPlatformPosition;

        private int _platformSelector;
        private float[] _platformsWidth;
        //public float  distanceMin, distanceMax, distBetween;
        [Range(2,5)] public float distForPutMin,distForPutMax,distForPut; // перейменувати, переменные для того чтобы делать разломы 
        [Range(6,9)] public float distForFaultMin, distForFaultMax, distForFault;
        public Transform generationPotint;
        public bool doFault;
        public bool topPlatform;
        private int _platformBefore;
        

        private void Start()
        {
            if (generationPotint == null) GameObject.Find("GenerationPoint");
                _platformsWidth = new float[platformsM.Length];

            for (var i = 0; i < platformsM.Length; i++)
                _platformsWidth[i] = platformsM[i].platform.GetComponent<BoxCollider2D>().size.x;
            
        }
        //[7,10] [12,15]
        private void FixedUpdate()
        {
            if (transform.position.x < generationPotint.position.x)
            {
                _platformSelector = Random.Range(0, platformsM.Length);
                if (Director.DoFault())
                {
                    
                    distForFault = Random.Range(distForFaultMin, distForFaultMax);
                    _newPlatformPosition = _platformsWidth[_platformSelector] / 2 + distForFault;
                }
                else
                {
                    distForPut = Random.Range(distForPutMin, distForPutMax);
                    _newPlatformPosition = _platformsWidth[_platformSelector] / 2 + distForPut;
                }

                //_platformBefore = _platformSelector;
                //_newPlatform = 
                transform.position = new Vector2(transform.position.x + _newPlatformPosition, transform.position.y);
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