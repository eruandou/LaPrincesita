using System;
using Attributes;
using UI;
using UnityEditor;
using UnityEngine;

public class GeneralCanvasController : MonoBehaviour
{
    [ReadOnlyInspector, SerializeField] private InGameCanvas inGameCanvas;
    [ReadOnlyInspector, SerializeField] private InventorySystemUI inventorySystemUI;

    public InGameCanvas GetInGameCanvas() => inGameCanvas;
    public InventorySystemUI GetInventoryUI() => inventorySystemUI;

    public static GeneralCanvasController instance;

    private void Awake()
    {
        inventorySystemUI.Initialize();
     
    }

    private void Start()
    {
        inventorySystemUI.DisableCanvas();
    }
#if UNITY_EDITOR
    [ContextMenu("Get canvas child")]
    private void GetCanvasChild()
    {
        inGameCanvas = GetComponentInChildren<InGameCanvas>();
        inventorySystemUI = GetComponentInChildren<InventorySystemUI>();
        EditorUtility.SetDirty(gameObject);
    }
#endif
}