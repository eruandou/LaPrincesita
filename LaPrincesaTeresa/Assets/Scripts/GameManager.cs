using System;
using Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

#if UNITY_EDITOR
        [SerializeField] private SceneAsset menuLevelScene;
#endif

        [SerializeField, ReadOnlyInspector] private string menuLevelName;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMenu()
        {
            ChangeLevel(menuLevelName);
        }

        private static void ChangeLevel(string levelToLoad)
        {
            SceneManager.LoadScene(levelToLoad);
        }

#if UNITY_EDITOR
        [ContextMenu("Get menu level")]
        internal void GetMenuName()
        {
            menuLevelName = menuLevelScene.name;
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}