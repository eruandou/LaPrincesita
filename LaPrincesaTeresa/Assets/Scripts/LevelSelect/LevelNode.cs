using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSelect
{
    public class LevelNode : MonoBehaviour
    {
        [field: SerializeField] public LevelNodeData LevelData { get; private set; }

        [SerializeField] private SpriteRenderer visuals;
        [SerializeField] private SpriteRenderer connection;
        [SerializeField] private TextMeshPro levelName;

        [FormerlySerializedAs("connectionLevel")] [SerializeField]
        private LevelNode nextConnectionLevel;

        [SerializeField] private LevelNode rightNode, leftNode, upNode, downNode;
        public bool IsLocked { get; private set; }
        public int NodeNumber { get; private set; }
        private event Action<bool> OnUnlock;


        public void Init(int nodeNumber)
        {
            LockLevel();
            if (nextConnectionLevel != default)
            {
                nextConnectionLevel.OnUnlock += SetNextConnectionVisibility;
            }

            levelName.text = LevelData.levelFormalName;
            NodeNumber = nodeNumber;
        }

        public LevelNode GetLeftConnection()
        {
            return leftNode;
        }

        public LevelNode GetRightConnection()
        {
            return rightNode;
        }

        public LevelNode GetUpConnection()
        {
            return upNode;
        }

        public LevelNode GetDownConnection()
        {
            return downNode;
        }

        private void SetLevelVisibility(bool isActive)
        {
            visuals.gameObject.SetActive(isActive);
        }

        private void SetNextConnectionVisibility(bool isActive)
        {
            connection.enabled = isActive;
        }

        private void LockLevel()
        {
            SetLevelVisibility(false);
            SetNextConnectionVisibility(false);
            IsLocked = true;
        }

        [ContextMenu("Test unlock level")]
        public void UnlockLevel()
        {
            SetLevelVisibility(true);
            IsLocked = false;
            OnUnlock?.Invoke(true);
        }

        [ContextMenu("Set level name")]
        private void SetLevelName()
        {
#if !UNITY_EDITOR
     return;
#endif
            levelName.text = LevelData.levelFormalName;
            EditorUtility.SetDirty(this);
        }
    }
}