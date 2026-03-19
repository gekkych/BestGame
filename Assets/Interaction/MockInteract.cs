using Interaction.Interfaces;
using UnityEngine;

namespace Interaction
{
    public class MockInteract : MonoBehaviour, IInteractable
    {
        public void StartInteraction(Interactor interactor)
        {
            Debug.Log("Starting interaction");
        }
        
        public void UpdateInteraction(float deltaTime)
        {
            Debug.Log("Updating interaction");
        }

        public void StopInteraction()
        {
            Debug.Log("Stopping interaction");
        }
        
        public void CancelInteraction()
        {
            Debug.Log("Canceling interaction");
        }
    }
}