using UnityEngine;
using UnityEngine.Events;

namespace Level
{
    public class HintEventListener : MonoBehaviour
    {
        [SerializeField] private HintEvent hintEvent;
        [SerializeField] private UnityEvent<HintEventParam> raisedEvent;
        [SerializeField] private int expectedID;

        private void OnEnable()
        {
            hintEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            hintEvent.UnregisterListener(this);
        }

        public void OnEventRaised(HintEventParam hintEventParam)
        {
            if (hintEventParam.eventID != expectedID)
                return;
            raisedEvent?.Invoke(hintEventParam);
        }
    }
}