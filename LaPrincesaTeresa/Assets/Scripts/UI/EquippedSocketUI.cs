using ScriptableObjects.Extras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EquippedSocketUI : MonoBehaviour
    {
        [SerializeField] private Image socketImage;
        [SerializeField] private TextMeshProUGUI socketNameText;
        [field: SerializeField] public string TargetSocket { get; private set; }
        private ItemData _item;

        public void SetData(ItemData newData)
        {
            _item = newData;
            socketImage.sprite = _item.itemSprite;
            socketNameText.text = _item.itemName;
        }
    }
}