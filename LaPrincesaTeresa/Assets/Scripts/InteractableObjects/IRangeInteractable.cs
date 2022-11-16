public interface IRangeInteractable
{
    void OnRangeChanged(bool isInRange);
    float RangeOutlineThickness { get; }
    void ResetOnStart();
}