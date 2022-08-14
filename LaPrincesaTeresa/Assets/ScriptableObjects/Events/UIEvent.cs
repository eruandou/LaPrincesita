using System.Collections.Generic;
using UI;
using UnityEngine;

namespace ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "NewUIEvent", menuName = "ScriptableObjects/UI/UIEvents", order = 0)]
    public class UIEvent : ScriptableObject
    {
        private List<UIEventListener> _listeners = new();

        public void Raise(UIParams p)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(p);
            }
        }

        public void RegisterListener(UIEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void UnregisterListener(UIEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
    }
}