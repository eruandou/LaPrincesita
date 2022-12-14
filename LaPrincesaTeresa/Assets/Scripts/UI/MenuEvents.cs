using Saves;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class MenuEvents : MonoBehaviour
{
    [Header("Buttons")] [SerializeField] private Button playButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button goBackMainMenuButton;
    [SerializeField] private Button overwriteSaveFileButton;
    [SerializeField] private Button cancelOverwriteSaveFileButton;

    [Header("Panels")] [SerializeField] private Panel mainPanel;
    [SerializeField] private Panel creditPanel;
    [SerializeField] private Panel helpPanel;
    [SerializeField] private Panel warningPanel;
    private Panel _currentlySelectedPanel;
    private bool _isMainMenu;

    private void Start()
    {
        FixedCallbacks();
        CheckMenusValidity();
        InputHandlingCallbacks();
        CheckContinueAvailable();

        ChangePanel(mainPanel);
    }

    private void InputHandlingCallbacks()
    {
        var inputHandler = FindObjectOfType<InputSystemUIInputModule>();
        inputHandler.cancel.ToInputAction().performed += OnGoBackToMain;
    }

    private void FixedCallbacks()
    {
        playButton.onClick.AddListener(OnNewGame);
        goBackMainMenuButton.onClick.AddListener(ToMainMenuCallback);
        quitButton.onClick.AddListener(GameStaticFunctions.QuitGame);
        overwriteSaveFileButton.onClick.AddListener(OverWriteSaveFile);
        overwriteSaveFileButton.onClick.AddListener(GoToLevelSelect);
        cancelOverwriteSaveFileButton.onClick.AddListener(CancelOverwriteSaveFile);
        continueButton.onClick.AddListener(GoToLevelSelect);
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

        if (hasHelp)
        {
            helpButton.onClick.AddListener(OnClickHelp);
        }

        if (hasCredits)
        {
            creditsButton.onClick.AddListener(OnClickCredits);
        }
    }

    private static void OverWriteSaveFile()
    {
        GameManager.Instance.DataSaver.ResetSaveData();
    }

    private void CancelOverwriteSaveFile()
    {
        ChangePanel(mainPanel);
    }


    private void CheckContinueAvailable()
    {
        continueButton.interactable = GameManager.Instance.DataSaver.GetSaveDataFound();
    }

    private void GoToLevelSelect()
    {
        GameManager.Instance.CustomSceneManager.LoadLevelSelect();
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

    private void OnNewGame()
    {
        //check if save file exists
        var saveFileFound = GameManager.Instance.DataSaver.GetSaveDataFound();
        if (saveFileFound)
        {
            ChangePanel(warningPanel);
            return;
        }

        OverWriteSaveFile();
        GoToLevelSelect();
    }


    private void OnClickHelp() => ChangePanel(helpPanel);

    private void OnClickCredits() => ChangePanel(creditPanel);

    private void OnGoBackToMain(InputAction.CallbackContext ctx)
    {
        ToMainMenuCallback();
    }

    #endregion
}