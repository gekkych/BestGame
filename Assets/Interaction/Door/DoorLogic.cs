using System;

namespace Interaction.Door
{
    public class DoorLogic
    {
        public bool IsOpening { get; private set; }
        public bool IsClosing { get; private set; }
        public bool IsOpen { get; private set; }
        public float Progress => _progress;
        public float OpenTime => _openTime;

        private float _openTime;
        private float _maxAngle;
        private float _currentAngle;
        private float _progress; 

        public event Action OnStartOpening;
        public event Action<float> OnProgressChanged;

        public DoorLogic(float openTime, float maxAngle)
        {
            _openTime = Math.Max(0.01f, openTime);
            _maxAngle = maxAngle;
        }

        public void Open()
        {
            if (IsOpen || IsOpening) return;

            IsOpening = true;
            IsClosing = false;
            
            OnStartOpening?.Invoke();
        }

        public void Close()
        {
            if (!IsOpen && !IsOpening || IsClosing) return;

            IsClosing = true;
            IsOpening = false;
        }

        public void AddProgress(float deltaTime)
        {
            if (!IsOpening && !IsClosing) return;
            
            float step = deltaTime / _openTime;
            _progress += IsOpening ? step : -step;
            
            _progress = Math.Clamp(_progress, 0f, 1f);
            
            _currentAngle = _progress * _maxAngle;
            OnProgressChanged?.Invoke(_currentAngle);
            
            if (_progress >= 1f && IsOpening)
            {
                IsOpening = false;
                IsOpen = true;
            }
            else if (_progress <= 0f && IsClosing)
            {
                IsClosing = false;
                IsOpen = false;
            }
        }
        
        public void ForceReset()
        {
            _progress = 0f;
            _currentAngle = 0f;
            IsOpen = false;
            IsOpening = false;
            IsClosing = false;
            OnProgressChanged?.Invoke(0f);
        }
    }
}