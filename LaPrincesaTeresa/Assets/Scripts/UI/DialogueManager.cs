using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScriptableObjects.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private Image dialogueImage;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float canvasFadeInTime, canvasFadeOutTime;
        [SerializeField] private AnimationCurve fadeCurve;
        private float _dialogueWaitBetweenChars;
        private Coroutine _typingCoroutine;
        private Coroutine _setCanvasOpacityCoroutine;
        private string _currentlyTypedText;
        private MultiDialogueObject _dialogueToShow;

        public static event Action OnDialogueFinished;
#if UNITY_EDITOR
        [Header("Test objects")] public MultiDialogueObject testDialogue;
#endif

        private void Awake()
        {
            EnableDisableDialogue(false, true);
            var playerInput = FindObjectOfType<PlayerInput>();

            if (playerInput != null)
            {
                SubscribeToEvents(playerInput);
            }
        }

        private void SubscribeToEvents(PlayerInput playerInput)
        {
            var moveOption = playerInput.actions["MoveOption"];
            var selectOption = playerInput.actions["Select"];
            var cancelOption = playerInput.actions["Cancel"];

            moveOption.performed += PlayerPressMove;
            selectOption.performed += PlayerPressSubmit;
            cancelOption.performed += PlayerPressCancel;
        }

        private void PlayerPressMove(InputAction.CallbackContext context)
        {
            Debug.Log($"Move option with {context.ReadValue<Vector2>()}");
        }


        private void PlayerPressSubmit(InputAction.CallbackContext context)
        {
            PressContinueCallback();
        }


        private void PlayerPressCancel(InputAction.CallbackContext context)
        {
            Debug.Log("Player canceled selection");
        }

        private void EnableDisableDialogue(bool enable, bool isImmediate = false)
        {
            print($"Enable is {enable}");
            if (enable)
            {
                canvasGroup.gameObject.SetActive(true);
            }

            if (isImmediate)
            {
                canvasGroup.alpha = enable ? 1 : 0;
            }
            else
            {
                if (_setCanvasOpacityCoroutine != null)
                {
                    StopCoroutine(_setCanvasOpacityCoroutine);
                }

                _setCanvasOpacityCoroutine =
                    StartCoroutine(SetCanvasOpacity(enable, enable ? canvasFadeInTime : canvasFadeOutTime));
            }
        }

        private IEnumerator SetCanvasOpacity(bool isFadeIn, float timeToFade)
        {
            var timePassed = 0f;
            var targetFade = isFadeIn ? 1 : 0;
            var start = canvasGroup.alpha;
            while (timePassed < timeToFade)
            {
                var lerpAmount = timePassed / timeToFade;
                var fadeAmount = Mathf.Lerp(start, targetFade, fadeCurve.Evaluate(lerpAmount));
                canvasGroup.alpha = fadeAmount;
                timePassed += Time.deltaTime;
                print(fadeAmount);
                yield return null;
            }

            if (!isFadeIn)
            {
                canvasGroup.gameObject.SetActive(false);
            }

            _setCanvasOpacityCoroutine = null;
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

            dialogueText.text = _currentlyTypedText;
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
                OnDialogueFinished?.Invoke();
                return;
            }

            var nextDialogueObject = _dialogueToShow.GetNextDialogue();
            PrepareDialogueText(nextDialogueObject.timeBetweenCharacters, nextDialogueObject.dialogue);
            SetupDialogueImage(nextDialogueObject.speakerImage);
            _typingCoroutine = StartCoroutine(TypeOutText());
        }

        private void SetupDialogueImage(Sprite talkerSprite)
        {
            dialogueImage.sprite = talkerSprite;
        }

        private IEnumerator TypeOutText()
        {
            Assert.IsTrue(_dialogueWaitBetweenChars > 0);

            var waitForSeconds = new WaitForSeconds(_dialogueWaitBetweenChars);
            dialogueText.text = "";
            foreach (var character in _currentlyTypedText)
            {
                dialogueText.text += character;
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