using Attributes;
using DefaultNamespace;
using Interface;
using UnityEditor;
using UnityEngine;

public class CrossRoads : MonoBehaviour, IInteractable
{
    private bool _isInteractable;
#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextLevelScene;
#endif
    [ReadOnlyInspector, SerializeField] private string nextLevelSceneName;

    private void Awake()
    {
        _isInteractable = true;
    }

    public void OnInteract(PlayerModel model)
    {
        if (!_isInteractable)
            return;
        _isInteractable = true;
        GameManager.Instance.CustomSceneManager.ChangeScene(nextLevelSceneName);
    }

    public void FinishedInteractionCallback()
    {
    }

#if UNITY_EDITOR
    [ContextMenu("Get scene name")]
    private void GetSceneName()
    {
        nextLevelSceneName = nextLevelScene.name;
    }

#endif
}