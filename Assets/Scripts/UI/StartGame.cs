using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private string gameScene;
        [SerializeField] private GameObject startUiScene;
        [SerializeField] private UnityEvent changeGravity;
        private void Start()
        {
            StartCoroutine(MovePlayer());
        }
        
        public void AsyncStart()
        {
            StartCoroutine(LoadAsyncScene());
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

        private IEnumerator MovePlayer()
        {
            while (true)
            {
                changeGravity.Invoke();
                yield return new WaitForSeconds(Random.Range(0.2f, 1f));
            }
        }
    }
}