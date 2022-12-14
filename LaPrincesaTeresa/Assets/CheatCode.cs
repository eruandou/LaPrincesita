using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private Button secretButton;

    private Queue<char> codeCheck;

    private void Awake()
    {
        secretButton.onClick.AddListener(ToSandbox);
    }

    private static void ToSandbox()
    {
        GameManager.Instance.CustomSceneManager.ChangeScene("SandBoxFinal");
    }
}