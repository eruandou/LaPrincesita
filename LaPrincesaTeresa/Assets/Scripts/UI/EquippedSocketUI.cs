using System;
using ScriptableObjects.Extras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EquippedSocketUI : MonoBehaviour
    {
        [SerializeField] private Image socketImage;
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [field: SerializeField] public string TargetSocket { get; private set; }
        private ItemData _item;

        private void Awake()
        {
            ResetState();
        }

        public void SetData(ItemData newData)
        {
            _item = newData;
            socketImage.sprite = _item.itemSprite;
            itemName.text = _item.itemName;
        }

        public void ResetState()
        {
            _item = null;
            itemName.text = "";
            socketImage.sprite = defaultIcon;
        }
    }
}