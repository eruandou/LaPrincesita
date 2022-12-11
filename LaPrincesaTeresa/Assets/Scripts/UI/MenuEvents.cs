using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class MenuEvents : MonoBehaviour
{
    [SerializeField] private string playLevel = "Totorial part1 FIX";

    [Header("Buttons")] [SerializeField] private Button playButton;
    [SerializeField] private Button levelSelectorButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button goBackMainMenuButton;

    [Header("Panels")] [SerializeField] private Panel mainPanel;
    [SerializeField] private Panel levelSelectorPanel;
    [SerializeField] private Panel creditPanel;
    [SerializeField] private Panel helpPanel;

    private Panel _currentlySelectedPanel;
    private bool _isMainMenu;

    private void Start()
    {
        FixedCallbacks();
        CheckMenusValidity();
        ChangePanel(mainPanel);
        InputHandlingCallbacks();
    }

    private void InputHandlingCallbacks()
    {
        var inputHandler = FindObjectOfType<InputSystemUIInputModule>();
        inputHandler.cancel.ToInputAction().performed += OnGoBackToMain;
    }

    private void FixedCallbacks()
    {
        playButton.onClick.AddListener(OnClickPlay);
        goBackMainMenuButton.onClick.AddListener(ToMainMenuCallback);
        quitButton.onClick.AddListener(GameStaticFunctions.QuitGame);
    }

    private void ToMainMenuCallback()
    {
        if (_isMainMenu)
            return;
        ChangePanel(mainPanel);
    }

    private void CheckMenusValidity()
    {
        var hasCredits = creditPanel != null && creditsButton != null;
        var hasHelp = helpPanel != null && helpButton != null;
        var hasLevelSelector = levelSelectorButton != null && levelSelectorPanel != null;

        if (hasHelp)
        {
            helpButton.onClick.AddListener(OnClickHelp);
        }

        if (hasCredits)
        {
            creditsButton.onClick.AddListener(OnClickCredits);
        }

        if (hasLevelSelector)
        {
            levelSelectorButton.onClick.AddListener(OnClickLevelSelector);
        }
    }


    private void ChangePanel(Panel panelToOpen)
    {
        if (_currentlySelectedPanel == panelToOpen)
            return;

        if (_currentlySelectedPanel != default)
        {
            _currentlySelectedPanel.Close();
        }

        _currentlySelectedPanel = panelToOpen;
        _currentlySelectedPanel.Open();

        _isMainMenu = panelToOpen == mainPanel;
        goBackMainMenuButton.gameObject.SetActive(!_isMainMenu);
    }

    #region OnClick

    private void OnClickPlay() => SceneManager.LoadScene(playLevel);

    private void OnClickLevelSelector() => ChangePanel(levelSelectorPanel);

    private void OnClickHelp() => ChangePanel(helpPanel);

    private void OnClickCredits() => ChangePanel(creditPanel);

    private void OnGoBackToMain(InputAction.CallbackContext ctx)
    {
        ToMainMenuCallback();
    }

    #endregion
}