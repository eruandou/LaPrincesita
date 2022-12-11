using System;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : Panel
{
    [SerializeField] private Button forwardButton, backButton;
    [SerializeField] private Panel[] menuScreens;
    private Panel _currentScreen;
    private int _tabIndex;

    private void Awake()
    {
        forwardButton.onClick.AddListener(() => ToTabByIndex(++_tabIndex));
        backButton.onClick.AddListener(() => ToTabByIndex(--_tabIndex));

        ToTabByIndex(0);
    }

    private void ToTabByIndex(int tabIndex)
    {
        _tabIndex = Mathf.Clamp(_tabIndex, 0, menuScreens.Length - 1);
        if (_currentScreen != default)
        {
            _currentScreen.Close();
        }

        _currentScreen = menuScreens[_tabIndex];
        _currentScreen.Open();

        forwardButton.gameObject.SetActive(_tabIndex != menuScreens.Length - 1);
        backButton.gameObject.SetActive(_tabIndex != 0);
    }
}