using System;
using System.Collections.Generic;
using InteractableObjects;
using ScriptableObjects.Extras;
using UnityEngine;

namespace UI
{
    public class InventorySystemUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryScreen;

        [SerializeField] private List<EquippedSocketUI> socketUIElements;

        private void OnEnable()
        {
            InteractableEquipableItem.OnItemPickedUp += SetUIElement;
        }

        private void OnDisable()
        {
            InteractableEquipableItem.OnItemPickedUp -= SetUIElement;
        }

        public void Disable()
        {
            inventoryScreen.SetActive(false);
        }

        public void SetUIElement(string socket, ItemData data)
        {
            for (int i = 0; i < socketUIElements.Count; i++)
            {
                if (socketUIElements[i].TargetSocket != socket) continue;
                socketUIElements[i].SetData(data);
                return;
            }

            Debug.LogError($"Incorrect socket assigned to item {socket}");
        }
    }
}