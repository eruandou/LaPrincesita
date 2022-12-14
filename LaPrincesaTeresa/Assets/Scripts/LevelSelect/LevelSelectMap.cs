using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEditor;
using UnityEngine;

namespace LevelSelect
{
    public class LevelSelectMap : MonoBehaviour
    {
        [SerializeField] private List<LevelNode> m_levelNodes;
        [SerializeField] private LevelSelectPlayer m_levelSelectPlayer;

        public static int lastVisitedNode;

        private void Awake()
        {
            for (int i = 0; i < m_levelNodes.Count; i++)
            {
                m_levelNodes[i].Init(i);
            }

            m_levelSelectPlayer.SetToNode(m_levelNodes[lastVisitedNode]);
        }

        private void Start()
        {
            var saveData = GameManager.Instance.DataSaver.GetCurrentSaveData();

            var unlockedLevels = saveData.unlockedLevels;

            UnlockLevels(unlockedLevels);
        }

        private void UnlockLevels(List<string> unlockedLevels)
        {
            foreach (var levelID in unlockedLevels)
            {
                CheckIndividualLevel(levelID);
            }
        }

        private void CheckIndividualLevel(string levelID)
        {
            foreach (var levelNode in m_levelNodes)
            {
                if (levelID != levelNode.LevelData.levelID) continue;
                levelNode.UnlockLevel();
                return;
            }
        }

        [ContextMenu("Get all level nodes")]
        private void GetAllLevelNodes()
        {
#if !UNITY_EDITOR
     return;
#endif
            var nodes = GetComponentsInChildren<LevelNode>();
            m_levelNodes = nodes.ToList();
            EditorUtility.SetDirty(gameObject);
        }
    }
}