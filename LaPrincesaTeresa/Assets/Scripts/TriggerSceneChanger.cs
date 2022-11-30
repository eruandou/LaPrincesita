using System;
using UnityEngine;
using Attributes;
using DefaultNamespace;
using UnityEditor;
public class TriggerSceneChanger : MonoBehaviour
{
    [SerializeField] private LayerMask contactLayers;
#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextLevelScene;
#endif
    [ReadOnlyInspector, SerializeField] private string nextLevelSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameStaticFunctions.IsGoInLayerMask(other.gameObject, contactLayers))
        {
         GameManager.Instance.CustomSceneManager.ChangeScene(nextLevelSceneName);
        }
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
