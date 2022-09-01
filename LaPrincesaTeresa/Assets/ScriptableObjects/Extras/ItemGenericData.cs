using UnityEngine;

namespace ScriptableObjects.Extras
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemMoveGenericData")]
    public class ItemGenericData : ScriptableObject
    {
        public float unpickedSpeed;
        public float pickedSpeed;
    }
}