using System;
using System.Collections.Generic;
using InteractableObjects;
using ScriptableObjects.Extras;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InventorySystemUI : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryScreen;

        [SerializeField] private List<EquippedSocketUI> socketUIElements;
        private Dictionary<string, EquippedSocketUI> _socketToUISocket;

        private void Awake()
        {
            _socketToUISocket = new Dictionary<string, EquippedSocketUI>();

            foreach (var socketElement in socketUIElements)
            {
                _socketToUISocket.Add(socketElement.TargetSocket, socketElement);
            }

       
        }

        public void Initialize()
        {
            InteractableEquipableItem.OnItemPickedUp += SetUIElement;
        }

        public void DisableCanvas()
        {
            inventoryScreen.SetActive(false);
        }

        private void SetUIElement(string socket, ItemData data)
        {
            if (!_socketToUISocket.TryGetValue(socket, out var socketUI))
            {
                Debug.LogError($"Incorrect socket assigned to item {socket}");
                return;
            }

            socketUI.SetData(data);
        }
    }
}