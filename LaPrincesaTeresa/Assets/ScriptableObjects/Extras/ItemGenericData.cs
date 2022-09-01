using UnityEngine;

namespace ScriptableObjects.Extras
{
    [CreateAssetMenu(menuName = "ItemMoveGenericData")]
    public class ItemGenericData : ScriptableObject
    {
        public float unpickedSpeed;
        public float pickedSpeed;
    }
}