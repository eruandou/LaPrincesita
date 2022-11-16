using UnityEngine;

public sealed class ResetableElementWithOutline : ResetableVisualElement, IRangeInteractable
{
    [field: SerializeField] public float RangeOutlineThickness { get; private set; }

    public void ResetOnStart()
    {
        interactionRenderer.material.SetFloat(OutlineThickness, 0);
    }

    [SerializeField] private Renderer interactionRenderer;
    private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");

    protected override void Awake()
    {
        base.Awake();
        ResetOnStart();
    }

    public void OnRangeChanged(bool isInRange)
    {
        interactionRenderer.material.SetFloat(OutlineThickness, isInRange ? RangeOutlineThickness : 0);
    }
}