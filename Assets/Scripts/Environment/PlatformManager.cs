using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class PlatformManager : MonoBehaviour
    {
        private List<GameObject> _platforms; //List of platforms
        private List<GameObject> _saws; //List of saws
        public GameObject platform;

        [FormerlySerializedAs("platformAmout")] public int platformAmount;
        public GameObject saw;

        private void Start()
        {
            _platforms = new List<GameObject>();
            _saws = new List<GameObject>();
            for (var i = 0; i < platformAmount; i++)
            {
                var platobj = Instantiate(platform);
                var sawobj = Instantiate(saw);
                platobj.SetActive(false);
                sawobj.SetActive(false);
                _platforms.Add(platobj);
                _saws.Add(sawobj);
            }
        }

        public GameObject GetPlatform()
        {
            for (var i = 0; i < _platforms.Count; i++)
                if (!_platforms[i].activeInHierarchy)
                    return _platforms[i];
            var obj = Instantiate(platform);
            obj.SetActive(false);
            _platforms.Add(obj);

            return obj;
        }

        public GameObject GetSaws()
        {
            for (var i = 0; i < _saws.Count; i++)
                if (!_saws[i].activeInHierarchy)
                    return _saws[i];
            var obj = Instantiate(saw);
            obj.SetActive(false);
            _saws.Add(obj);

            return obj;
        }
    }
}