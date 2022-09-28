using UnityEngine;

namespace ScriptableObjects.Extras
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemMoveGenericData")]
    public class ItemData : ScriptableObject
    {
        public float unpickedSpeed;
        public float pickedSpeed;
        public Sprite itemSprite;
        public string itemName;
        public string itemDescription;
    }
}