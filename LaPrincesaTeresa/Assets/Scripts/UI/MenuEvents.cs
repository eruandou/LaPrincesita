using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class MenuEvents : MonoBehaviour
{
    public string playLevel = "Totorial part1 FIX";

    [Header("Buttons")]
    public Button playButton;
    public Button levelSelectorButton;
    public Button creditsButton;
    public Button helpButton;
    public Button quitButton;
    public Button goBackMainMenuButton;

    [Header("Panels")]
    public Panel mainPanel;
    public Panel levelSelectorPanel;
    public Panel creditPanel;
    public Panel helpPanel;

    private List<Panel> allPanels = new List<Panel>();
    private bool hasCredits, hasHelp, hasLevelSelector, isMainMenu;

    private void Start()
    {
        hasCredits = creditPanel != null && creditsButton != null;
        hasHelp = helpPanel != null && helpButton != null;
        hasLevelSelector = levelSelectorButton != null && levelSelectorPanel != null;

        playButton.onClick.AddListener(OnClickPlay);
        goBackMainMenuButton.onClick.AddListener(OnGoBackToMain);
        quitButton.onClick.AddListener(OnClickQuit);

        mainPanel.gameObject.SetActive(true);
        allPanels.Add(mainPanel);

        if (hasHelp)
        {
            helpButton.onClick.AddListener(OnClickHelp);
            helpPanel.gameObject.SetActive(true);
            allPanels.Add(helpPanel);
        }

        if (hasCredits)
        {
            creditsButton.onClick.AddListener(OnClickCredits);
            creditPanel.gameObject.SetActive(true);
            allPanels.Add(creditPanel);
        }

        if (hasLevelSelector)
        {
            levelSelectorButton.onClick.AddListener(OnClickLevelSelector);
            levelSelectorPanel.gameObject.SetActive(true);
            allPanels.Add(levelSelectorPanel);
        }

        ChangePanel(mainPanel);
    }

    //private void Update()
    //{
    //    if (isMainMenu) return;
    //    if (Input.GetKey(KeyCode.Escape)) //TODO add check if input escape is pressed and is not main menu, return to main
    //        OnGoBackToMain();
    //}

    private void ChangePanel(Panel panelToOpen)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            if(allPanels[i] == panelToOpen)
                allPanels[i].Open();
            else
                allPanels[i].Close();
        }

        isMainMenu = panelToOpen == mainPanel;
        goBackMainMenuButton.gameObject.SetActive(!isMainMenu);
    }

    #region OnClick
    public void OnClickPlay()
    {
        SceneManager.LoadScene(playLevel);
    }

    public void OnClickLevelSelector()
    {
        ChangePanel(levelSelectorPanel);
    }

    public void OnClickHelp()
    {
        ChangePanel(helpPanel);
    }

    public void OnClickCredits()
    {
        ChangePanel(creditPanel);
    }

    public void OnClickSanbox()
    {
        SceneManager.LoadScene("SandBox");
    }

    public void OnGoBackToMain()
    {
        ChangePanel(mainPanel);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
    #endregion
}
