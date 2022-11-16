namespace Interface
{
    public interface IInteractable
    {
        void OnInteract(PlayerModel model);

        void FinishedInteractionCallback();
        void OnRangeChanged(bool isInRange);
    }
}