using System;
using System.Collections;
using ScriptableObjects.Dialogue;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private Image _dialogueImage;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float canvasFadeTime;
        [SerializeField] private AnimationCurve fadeCurve;
        private float _dialogueWaitBetweenChars;
        private Coroutine _typingCoroutine;
        private string _currentlyTypedText;
        private MultiDialogueObject _dialogueToShow;

#if UNITY_EDITOR
        [Header("Test objects")] public MultiDialogueObject testDialogue;
#endif

        private void Awake()
        {
            EnableDisableDialogue(false, true);
        }

        private void EnableDisableDialogue(bool enable, bool isImmediate = false)
        {
            print($"Enable is {enable}");
            canvasGroup.gameObject.SetActive(enable);
            if (isImmediate)
            {
                canvasGroup.alpha = enable ? 1 : 0;
            }
            else
            {
                StartCoroutine(SetCanvasOpacity(enable));
            }
        }

        private IEnumerator SetCanvasOpacity(bool isFadeIn)
        {
            var timePassed = 0f;
            var targetFade = isFadeIn ? 1 : 0;
            while (timePassed < canvasFadeTime)
            {
                var lerpAmount = timePassed / canvasFadeTime;
                var fadeAmount = Mathf.Lerp(canvasGroup.alpha, targetFade, fadeCurve.Evaluate(lerpAmount));
                canvasGroup.alpha = fadeAmount;
                timePassed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetFade;
        }

        public void ReceiveDialogue(MultiDialogueObject newDialogueObject)
        {
            _dialogueToShow = newDialogueObject;
            Assert.IsNotNull(_dialogueToShow);
            _dialogueToShow.ResetDialogue();
            EnableDisableDialogue(true);
            NextDialogue();
        }

        private void PrepareDialogueText(float dialogueWaitBetweenChars, string dialogueToShow)
        {
            _dialogueWaitBetweenChars = Mathf.Max(dialogueWaitBetweenChars, 0);
            _currentlyTypedText = dialogueToShow;
        }

        private void SkipDialogue()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            _dialogueText.text = _currentlyTypedText;
        }

        public void PressContinueCallback()
        {
            if (_typingCoroutine != null)
            {
                SkipDialogue();
                return;
            }

            NextDialogue();
        }

        private void NextDialogue()
        {
            if (!_dialogueToShow.CheckNextDialogueAvailable())
            {
                EnableDisableDialogue(false);
                return;
            }

            var nextDialogueObject = _dialogueToShow.GetNextDialogue();
            PrepareDialogueText(nextDialogueObject.timeBetweenCharacters, nextDialogueObject.dialogue);
            _typingCoroutine = StartCoroutine(TypeOutText());
        }

        private IEnumerator TypeOutText()
        {
            Assert.IsTrue(_dialogueWaitBetweenChars > 0);

            var waitForSeconds = new WaitForSeconds(_dialogueWaitBetweenChars);
            _dialogueText.text = "";
            foreach (var character in _currentlyTypedText)
            {
                _dialogueText.text += character;
                yield return waitForSeconds;
            }

            _typingCoroutine = null;
        }

#if UNITY_EDITOR
        [ContextMenu("Test dialogue object")]
        public void TestDialogueSystem()
        {
            if (testDialogue != null)
            {
                ReceiveDialogue(testDialogue);
            }
        }

        [ContextMenu("Next Dialogue test")]
        public void NextDialogueTest()
        {
            NextDialogue();
        }

        [ContextMenu("Skip dialogue Test")]
        public void SkipDialogueTest()
        {
            SkipDialogue();
        }

        [ContextMenu("Press continue callback test")]
        public void PressContinueCallbackTest()
        {
            PressContinueCallback();
        }
#endif
    }
}