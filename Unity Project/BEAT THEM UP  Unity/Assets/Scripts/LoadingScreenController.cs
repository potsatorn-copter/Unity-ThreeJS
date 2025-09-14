using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyPlatformer
{
    public class LoadingScreenController : MonoBehaviour
    {
        private static LoadingScreenController _instance;
        public static LoadingScreenController instance => _instance;

        [SerializeField] private GameObject _loadingScreenObject;

        private void Awake()
        {
            _instance = this;
        }

        public void LoadNextScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            // show the loading screen
            _loadingScreenObject.SetActive(true);

            // start unloading the current scene
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            // wait for the unloading to finish
            while (!unloadOp.isDone)
            {
                // you can update the progress here

                // wait for a frame
                yield return null;
            }

            // start loading a new scene
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            // activate the scene as soon as it is ready
            loadOp.allowSceneActivation = true;

            // wait for the loading to finish
            while (!loadOp.isDone)
            {
                // you can update the progress here

                // wait for a frame
                yield return null;
            }

            // set the newly loaded scene as the main scene
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            // hide the loading screen
            _loadingScreenObject.SetActive(false);
        }
    }
}
