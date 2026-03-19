namespace Interaction.Interfaces
{
    public interface IInteractable
    {
        void StartInteraction(Interactor interactor);
        void UpdateInteraction(float deltaTime);
        void StopInteraction();
        void CancelInteraction();
    }
}