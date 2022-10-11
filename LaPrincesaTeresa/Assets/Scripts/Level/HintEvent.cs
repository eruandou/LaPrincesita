using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "NewHintEvent", menuName = "ScriptableObjects/Level/HintEvents", order = 0)]
    public class HintEvent : ScriptableObject
    {
        private List<HintEventListener> _listeners = new();

        public void Raise(HintEventParam hintEventParam)
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(hintEventParam);
            }
        }

        public bool CheckActiveListeners()
        {
            return _listeners.Count > 0;
        }

        public void RegisterListener(HintEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void UnregisterListener(HintEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
    }
}