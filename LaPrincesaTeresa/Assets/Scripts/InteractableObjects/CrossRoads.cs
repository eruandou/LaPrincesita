using Attributes;
using Interface;
using UnityEditor;
using UnityEngine;

namespace InteractableObjects
{
    public class CrossRoads : MonoBehaviour, IInteractable, IRangeInteractable
    {
        private bool _isInteractable;
#if UNITY_EDITOR
        [SerializeField] private SceneAsset nextLevelScene;
#endif
        [ReadOnlyInspector, SerializeField] private string nextLevelSceneName;

        [SerializeField] private Renderer[] interactionRenderer;
        [field: SerializeField] public float RangeOutlineThickness { get; private set; }

        private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");


        public void ResetOnStart()
        {
            foreach (var interactionRend in interactionRenderer)
            {
                interactionRend.material.SetFloat(OutlineThickness, 0);
            }
        }

        public void OnRangeChanged(bool isInRange)
        {
            foreach (var interactionRend in interactionRenderer)
            {
                interactionRend.material.SetFloat(OutlineThickness, isInRange ? RangeOutlineThickness : 0);
            }
        }


        private void Awake()
        {
            _isInteractable = true;
            ResetOnStart();
        }

        public void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;
            _isInteractable = true;
            GameManager.Instance.UnlockLevel(nextLevelSceneName);
            GameManager.Instance.CustomSceneManager.LoadLevelSelect();
        }

        public void FinishedInteractionCallback()
        {
        }


#if UNITY_EDITOR
        [ContextMenu("Get scene name")]
        private void GetSceneName()
        {
            nextLevelSceneName = nextLevelScene.name;
            EditorUtility.SetDirty(gameObject);
        }

#endif
    }
}