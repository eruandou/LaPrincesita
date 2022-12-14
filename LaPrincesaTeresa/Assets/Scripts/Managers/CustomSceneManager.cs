using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class CustomSceneManager
    {
        public static event Action SceneLoadedSuccesfully;

        private CanvasGroup m_curtainCanvasGroup;

        public CustomSceneManager(CanvasGroup p_curtainCanvasGroup)
        {
            m_curtainCanvasGroup = p_curtainCanvasGroup;
        }

        public void ChangeScene(string sceneToLoad)
        {
            LoadSceneInternal(sceneToLoad, null);
        }

        internal TweenerCore<float, float, FloatOptions> SetCurtains(bool areSet)
        {
            var async = m_curtainCanvasGroup.DOFade(areSet ? 1 : 0, 0.4f);
            return async;
        }

        public void ChangeScene(string sceneToLoad, Action finishedCallback)
        {
            LoadSceneInternal(sceneToLoad, finishedCallback);
        }

        public void LoadLevelSelect()
        {
            LoadSceneInternal("LevelSelect");
        }

        private void LoadSceneInternal(string sceneToLoad, Action finishedCallback = null)
        {
            //Catch problem
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            var tween = SetCurtains(true);
            tween.onComplete += () => StartLoadScene(sceneToLoad, finishedCallback);
        }

        private void StartLoadScene(string sceneToLoad, Action finishedCallback = null)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            asyncOperation.completed += (operation) => FinishedLoadingScene(finishedCallback);
        }

        private void FinishedLoadingScene(Action finishedCallback = null)
        {
            SceneLoadedSuccesfully?.Invoke();
            finishedCallback?.Invoke();
            SetCurtains(false);
        }

        public void LoadMenu()
        {
            LoadSceneInternal("Main Menu");
        }
    }
}