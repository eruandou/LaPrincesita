using Attributes;
using DefaultNamespace.Managers;
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

        public CustomSceneManager CustomSceneManager { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            CustomSceneManager = new CustomSceneManager();
            Instance = this;
            DontDestroyOnLoad(gameObject);
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