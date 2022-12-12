using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Saves;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    public class ListOfScenes : ScriptableObject
    {
        public List<SceneAsset> sceneAssets = new List<SceneAsset>();
    }

    public class SaveFileManagement : EditorWindow
    {
        [MenuItem("LaPrincesita/SaveFile")]
        private static void ShowWindow()
        {
            var window = GetWindow<SaveFileManagement>();
            window.titleContent = new GUIContent("Save File");
            window.Show();
        }

        private static bool _glideUnlocked, _doubleJumpUnlocked, _dashUnlocked;
        private static ReorderableList _sceneToUnlock;
        private static SerializedObject _serializedObject;
        private static SerializedProperty _serializedProperty;


        /*
        private void OnEnable()
        {
            var list = ScriptableObject.CreateInstance<ListOfScenes>();

            _serializedObject = new SerializedObject(list);
            _serializedProperty = _serializedObject.FindProperty("sceneAssets");

            _sceneToUnlock = new ReorderableList(_serializedObject, _serializedProperty, true, true, true, true)
            {
                drawElementCallback = ScenesDrawListItems,
                drawHeaderCallback = ScenesDrawHeader
            };
        }*/

        void ScenesDrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            Debug.Log("A");
        }

        void ScenesDrawHeader(Rect rect)
        {
            Debug.Log("B");
        }

        private void OnGUI()
        {
            _glideUnlocked = EditorGUILayout.Toggle("glide", _glideUnlocked);
            _doubleJumpUnlocked = EditorGUILayout.Toggle("Double Jump", _doubleJumpUnlocked);
            _dashUnlocked = EditorGUILayout.Toggle("dash", _dashUnlocked);
/*
            if (_serializedObject == null)
            {
                return;
            }

            _serializedObject.Update();
            _sceneToUnlock.DoLayoutList();
            _serializedObject.ApplyModifiedProperties();
*/

            if (GUILayout.Button("Create save file"))
            {
                CreateSaveFile();
            }
            if (GUILayout.Button("Delete save file"))
            {
                DeleteSaveFile();
            }

            if (GUILayout.Button("Clear save file"))
            {
                ClearSaveFile();
            }
        }

        private void CreateSaveFile()
        {
            var saveData = new SaveData()
            {
                glide = _glideUnlocked,
                dash = _dashUnlocked,
                doubleJump = _doubleJumpUnlocked
            };
            var saveFileLocation = DataSaver.GetFullSaveDataPath();
            if (!File.Exists(saveFileLocation))
            {
                DataSaver.SaveData(saveData);
                return;
            }

            var accept = EditorUtility.DisplayDialog("Warning", "Save file already exists. Overwrite?", "Yes", "No",
                DialogOptOutDecisionType.ForThisMachine, "operation canceled");

            if (accept)
            {
                DataSaver.SaveData(saveData);
            }
            else
            {
                Debug.LogWarning("Aborted save");
            }
        }

        private void ClearSaveFile()
        {
            var saveFileLocation = DataSaver.GetFullSaveDataPath();

            if (!File.Exists(saveFileLocation))
                return;
            DataSaver.SaveData(new SaveData());
        }

        private void DeleteSaveFile()
        {
            var saveFileLocation = DataSaver.GetFullSaveDataPath();

            if (!File.Exists(saveFileLocation))
                return;
            File.Delete(saveFileLocation);
        }
    }
}