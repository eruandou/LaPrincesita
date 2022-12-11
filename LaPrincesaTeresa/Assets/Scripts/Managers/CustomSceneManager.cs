using System;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class CustomSceneManager
    {
        public static event Action SceneLoadedSuccesfully;

        public void ChangeScene(string sceneToLoad)
        {
            LoadSceneInternal(sceneToLoad, null);
        }


        public void ChangeScene(string sceneToLoad, Action finishedCallback)
        {
            LoadSceneInternal(sceneToLoad, finishedCallback);
        }

        private static void LoadSceneInternal(string sceneToLoad, Action finishedCallback = null)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            asyncOperation.completed += (op) => SceneLoadedSuccesfully?.Invoke();
            asyncOperation.completed += (op) => finishedCallback?.Invoke();
        }

        public void LoadMenu()
        {
            LoadSceneInternal("MainMenu");
        }
    }
}