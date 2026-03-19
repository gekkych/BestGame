using DG.Tweening;
using Interaction.Interfaces;
using UnityEngine;

namespace Interaction.Door
{
    public class DoorInteract : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private Door _door;
        [SerializeField] private Ease _easeType = Ease.InOutQuad;

        private float _openDuration = 0.5f;
        private DoorLogic _logic;
        private Tween _moveTween;
        private bool _isOpened;

        private void Awake()
        {
            _logic = _door.Logic;
            _openDuration = _logic.OpenTime;
        }

        public void StartInteraction(Interactor interactor)
        {
            if (_isOpened || _moveTween != null) return;
            
            _logic.Open(); 
            
            float timer = 0f;
            _moveTween = DOTween.To(() => timer, x => 
                {
                    float delta = x - timer;
                    timer = x;
                    
                    _logic.AddProgress(delta);
                    Debug.Log(_logic.Progress);
                }, _openDuration, _openDuration)
                .SetEase(_easeType)
                .OnComplete(() => 
                {
                    _isOpened = true;
                    _moveTween = null;
                });
        }
        
        public void ResetDoorInstant()
        {
            _moveTween?.Kill(); 
            _moveTween = null;
            _isOpened = false;
            _logic?.ForceReset();
        }
        
        public void UpdateInteraction(float deltaTime) { }
        public void StopInteraction() { }
        public void CancelInteraction() { }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}