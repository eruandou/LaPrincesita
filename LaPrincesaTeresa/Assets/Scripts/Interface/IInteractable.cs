namespace Interfaces
{
    public interface IInteractable
    {
        void OnInteract(PlayerModel model);
        bool IsInteractable();

        void FinishedInteractionCallback();

    }
}