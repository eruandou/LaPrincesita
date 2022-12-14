using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LevelSelect
{
    [CreateAssetMenu(menuName = "LaPrincesita/Levels/LevelNodeData")]
    public class LevelNodeData : ScriptableObject
    {
        public string levelID;
        public string levelFormalName;

#if UNITY_EDITOR
        public SceneAsset sceneLevel;
#endif

        [ContextMenu("Get scene ID")]
        private void GetSceneToID()
        {
#if !UNITY_EDITOR
     return;
#endif
            if (sceneLevel != null)
            {
                levelID = sceneLevel.name;
            }


            EditorUtility.SetDirty(this);
        }
    }
}