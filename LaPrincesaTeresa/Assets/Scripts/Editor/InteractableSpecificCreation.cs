using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public enum InteractableType
    {
        ContinuedInteraction,
        Equipable
    }

    public class InteractableSpecificCreation : EditorWindow
    {
        [MenuItem("Princesita/CreateSpecificInteractable")]
        private static void ShowWindow()
        {
            var window = GetWindow<InteractableSpecificCreation>();
            window.titleContent = new GUIContent("Interactable Creation");
            window.Show();
        }

        private static PlayerModel _model;
        private static InteractableType _type;
        private static string _socketName;
        private string[] _socketNamesPlayer;
        private int _selectedSocketIndex;

        private void OnGUI()
        {
            _model = (PlayerModel)EditorGUILayout.ObjectField("Player Model", _model, typeof(PlayerModel));
            _type = (InteractableType)EditorGUILayout.EnumPopup("Interactable Type", _type);

            if (_type == InteractableType.Equipable && _model != null)
            {
                if (_socketNamesPlayer == null || _socketNamesPlayer.Length < 1)
                {
                    _socketNamesPlayer = _model.GetAllSockets().ToArray();
                }

                _selectedSocketIndex = EditorGUILayout.Popup("Socket Name Select", _selectedSocketIndex,
                    _socketNamesPlayer);

                _socketName = _socketNamesPlayer[_selectedSocketIndex];
            }

            if (GUILayout.Button("Create Interactable"))
            {
                SpawnInteractable();
            }
        }

        private static void SpawnInteractable()
        {
            switch (_type)
            {
                case InteractableType.ContinuedInteraction:
                    break;
                case InteractableType.Equipable:
                    var interactable = EditorInstantiator.CreateEquipableInteractable();
                    interactable.SetSocketByEditor(_socketName);
                    break;
            }
        }
    }
}