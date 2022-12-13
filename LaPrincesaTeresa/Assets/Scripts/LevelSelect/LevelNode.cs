using System;
using UnityEngine;

namespace LevelSelect
{
    public class LevelNode : MonoBehaviour
    {
        [field: SerializeField] public LevelNodeData LevelData { get; private set; }

        [SerializeField] private SpriteRenderer visuals;
        [SerializeField] private SpriteRenderer connection;
        [SerializeField] private LevelNode connectionLevel;

        private event Action<bool> OnUnlock;

        public void Init()
        {
            LockLevel();
            if (connectionLevel != default)
            {
                connectionLevel.OnUnlock += SetConnectionVisibility;
            }
        }

        private void SetLevelVisibility(bool isActive)
        {
            visuals.gameObject.SetActive(isActive);
        }

        private void SetConnectionVisibility(bool isActive)
        {
            connection.enabled = isActive;
        }

        private void LockLevel()
        {
            SetLevelVisibility(false);
            SetConnectionVisibility(false);
        }

        [ContextMenu("Test unlock level")]
        public void UnlockLevel()
        {
            SetLevelVisibility(true);
            OnUnlock?.Invoke(true);
        }
    }
}