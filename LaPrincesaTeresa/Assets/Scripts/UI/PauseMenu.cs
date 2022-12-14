using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton, quitButton, mainMenuButton;
        [SerializeField] private CanvasGroup pauseMenuCanvas;
        [SerializeField] private float fadeDuration = 0.6f;

        private const string MAIN_MENU_SCENE = "Main Menu";

        private bool _isPaused;
        private bool _isInteractable;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            pauseMenuCanvas.alpha = 0;
        }

        private void Start()
        {
            FixedCallbacks();

            var input = FindObjectOfType<PlayerInput>();

            if (input == default)
                return;
            PauseCallbacks(input);
        }

        private void FixedCallbacks()
        {
            resumeButton.onClick.AddListener(() => SetPauseState(false));
            quitButton.onClick.AddListener(QuitGame);
            mainMenuButton.onClick.AddListener(ToMainMenu);
        }

        private void PauseCallbacks(PlayerInput playerInput)
        {
            var pauseAction = playerInput.actions["Pause"];
            pauseAction.performed += OnPauseActionHandler;
        }

        private void OnPauseActionHandler(InputAction.CallbackContext ctx)
        {
            SetPauseState(!_isPaused);
        }

        private void ToMainMenu()
        {
            SetPauseState(false);
            SceneManager.LoadScene(MAIN_MENU_SCENE);
        }

        private static void QuitGame()
        {
            GameStaticFunctions.QuitGame();
        }

        private void SetPauseState(bool isPaused)
        {
            _isPaused = isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pauseMenuCanvas.DOFade(isPaused ? 255 : 0, fadeDuration);
            _isInteractable = isPaused;
        }
    }
}