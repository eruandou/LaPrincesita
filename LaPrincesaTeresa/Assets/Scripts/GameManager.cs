using System;
using Attributes;
using Managers;
using Saves;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset menuLevelScene;
#endif

    [SerializeField, ReadOnlyInspector] private string menuLevelName;

    public CustomSceneManager CustomSceneManager { get; private set; }
    public DataSaver DataSaver { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        CustomSceneManager = new CustomSceneManager();
        DataSaver = new DataSaver();
        DataSaver.Initialize();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PowerUpGet(PowerupType powerUpObtained)
    {
        DataSaver.SetUnlockedElement(powerUpObtained);
    }

    public void UnlockLevel(string levelID)
    {
        DataSaver.SetUnlockedElement(PowerupType.Level, levelID);
    }

    private void OnApplicationQuit()
    {
        DataSaver.SaveCurrentData();
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