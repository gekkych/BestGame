using System;

namespace Interaction.Valve
{
    public class ValveLogic
    {
        private bool _isCompleted = false;
        public event Action<float> OnProgressChanged;
        public event Action OnCompleted;
        
        public float InteractionTime { get; }

        private float _currentInteractionTime;
        public float CurrentInteractionTime
        {
            get => _currentInteractionTime;
            set
            {
                if (_isCompleted) return;
                _currentInteractionTime = value;
                OnProgressChanged?.Invoke(_currentInteractionTime);
            }
        }

        public ValveLogic(float interactionTime) 
        { 
            InteractionTime = interactionTime;
            CurrentInteractionTime = 0.0f;
        }

        private bool CheckStop()
        {
            return _currentInteractionTime >= InteractionTime;
        }
        
        public void AddProgress(float deltaTime)
        {
            if (_isCompleted) return;
            if (CheckStop())
            {
                _isCompleted = true;
                OnCompleted?.Invoke();
                return;
            }
            CurrentInteractionTime += deltaTime;
        }
    }
}