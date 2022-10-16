using DefaultNamespace;
using Interface;
using UnityEditor;
using UnityEngine;

public class CrossRoads : MonoBehaviour, IInteractable
{
    private bool _isInteractable;
    [SerializeField] private SceneAsset nextLevelScene;
    private void Awake()
    {
        _isInteractable = true;
    }

    public void OnInteract(PlayerModel model)
    {
        if (!_isInteractable)
            return;
        _isInteractable = true;
        GameManager.Instance.ChangeLevel(nextLevelScene);
    }

    public void FinishedInteractionCallback()
    {
    }
}