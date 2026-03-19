using DG.Tweening;
using Interaction.Interfaces;
using UnityEngine;

namespace Interaction.Valve
{
    public class ValveInteract : MonoBehaviour, IInteractable
    {
        [Header("Valve Reference")]
        [SerializeField] private Valve _valve;
        private bool completed = false;
        private ValveLogic _logic;
        private Tween _decreaseTween;

        private void Start()
        {
            _logic = _valve.Logic;
            _logic.OnCompleted += Completed;
        }

        public void StartInteraction(Interactor interactor)
        {
            _decreaseTween?.Kill();
        }

        public void UpdateInteraction(float deltaTime)
        {
            _logic.AddProgress(deltaTime);
        }

        public void StopInteraction()
        {
            StartDecrease();
        }

        public void CancelInteraction()
        {
            StartDecrease();
        }

        private void StartDecrease()
        {
            if (completed) return;
            _decreaseTween?.Kill();
            float duration = (_logic.CurrentInteractionTime / _logic.InteractionTime) * 3f;

            _decreaseTween = DOTween.To(
                () => _logic.CurrentInteractionTime,
                x => _logic.CurrentInteractionTime = x,
                0f,
                duration).
                OnKill((() => _decreaseTween = null)).
                OnComplete(() => _decreaseTween = null);
        }

        private void Completed()
        {
            _decreaseTween?.Kill();
            if (!completed) completed = true;
        }
    }
}