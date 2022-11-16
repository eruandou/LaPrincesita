using System.Collections;
using System.Linq;
using Interface;
using ScriptableObjects.Dialogue;
using ScriptableObjects.Events;
using TMPro;
using UI;
using UnityEngine;

namespace NPC
{
    [SelectionBase]
    public class NPCController : MonoBehaviour, IInteractable
    {
        [SerializeField] private MultiDialogueObject npcDialogue;
        [SerializeField] private UIEvent uiEvent;
        [SerializeField] private CanvasGroup pressEToInteract;
        [SerializeField] private float fadeInTime, fadeOutTime;

        private Coroutine _setCanvasOpacityCoroutine;
        private PlayerModel _model;
        private bool _isInteractable;

        private void Awake()
        {
            _isInteractable = true;

#if UNITY_EDITOR
            CheckUniqueNpcController();
#endif
        }

#if UNITY_EDITOR
        private void CheckUniqueNpcController()
        {
            var otherNpcController = GetComponentsInChildren<NPCController>().Where(x => x != this).ToArray();

            if (otherNpcController.Length <= 0) return;

            Debug.LogError($"Character {gameObject.name} has more than one NPC Controller assigned");
            Debug.Break();
        }
#endif


        private void EnableDisableDialogue(bool enable)
        {
            if (enable)
            {
                pressEToInteract.gameObject.SetActive(true);
            }

            if (_setCanvasOpacityCoroutine != null)
            {
                StopCoroutine(_setCanvasOpacityCoroutine);
            }

            _setCanvasOpacityCoroutine =
                StartCoroutine(SetCanvasOpacity(enable, enable ? fadeInTime : fadeOutTime));
        }

        private IEnumerator SetCanvasOpacity(bool isFadeIn, float timeToFade)
        {
            var timePassed = 0f;
            var targetFade = isFadeIn ? 1 : 0;
            var start = pressEToInteract.alpha;
            while (timePassed < timeToFade)
            {
                var lerpAmount = timePassed / timeToFade;
                var fadeAmount = Mathf.Lerp(start, targetFade, lerpAmount);
                pressEToInteract.alpha = fadeAmount;
                timePassed += Time.deltaTime;
                yield return null;
            }

            if (!isFadeIn)
            {
                pressEToInteract.gameObject.SetActive(false);
            }

            _setCanvasOpacityCoroutine = null;
            pressEToInteract.alpha = targetFade;
        }

        public void OnInteract(PlayerModel model)
        {
            if (!_isInteractable)
                return;
            _isInteractable = false;
            _model = model;
            _model.ResetMobility();

            OnInteractionChange(ControllerTypes.Dialogue);

            uiEvent.Raise(new UIParams(UICommand.DialogueCommand, npcDialogue));

            InGameDialogueManager.OnInGameDialogueFinished += FinishedInteractionCallback;
        }


        private void OnInteractionChange(ControllerTypes newControllerType)
        {
            if (_model == null || _model.Controller == null)
                return;
            _model.Controller.RequestChangeMap(newControllerType);
        }

        public void FinishedInteractionCallback()
        {
            _isInteractable = true;
            OnInteractionChange(ControllerTypes.Regular);
            InGameDialogueManager.OnInGameDialogueFinished -= FinishedInteractionCallback;
        }

        public void OnRangeChanged(bool isInRange)
        {
#if UNITY_EDITOR
            print($"Player is near? {isInRange}");
#endif
            EnableDisableDialogue(isInRange);
        }

        public bool IsInteractable()
        {
            return _isInteractable;
        }
    }
}