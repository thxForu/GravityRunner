using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace UI
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private string gameScene;
        [SerializeField] private GameObject startUiScene;
        private void Start()
        {
            Time.timeScale = 0;
        }
        public void AsyncStart()
        {
            StartCoroutine(LoadAsyncScene());
            startUiScene.SetActive(false);
                        
        }

        IEnumerator LoadAsyncScene()
        {

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameScene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
                
            }
        }
    }
}