using System;
using System.Threading;
using System.Threading.Tasks;

namespace Source.Turn_State_Machine 
{ 
    public class Timer
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Func<float> _timeSource;

        public event Action TimeEnded;
        
        public float WaitTimeInSeconds { get; }
        public float ElapsedTime { get; private set; }

        public Timer(float waitTimeInSeconds, Func<float> timeSource)
        {
            WaitTimeInSeconds = waitTimeInSeconds;
            _timeSource = timeSource;
        }

        public async void StartTimerAsync()
        {
            ElapsedTime = 0f;
            _cancellationTokenSource = new CancellationTokenSource();

            while (ElapsedTime < WaitTimeInSeconds)
            {
                if (_timeSource != null)
                {
                    ElapsedTime += _timeSource();
                }
                else
                {
                    StopTimer();
                    return;
                }
                await Task.Yield();
            }
            OnTimeEnded();
            StopTimer();
        }

        public void StopTimer()
        {
            if (_cancellationTokenSource == null) return;
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        public void Destroy()
        {
            StopTimer();
            _timeSource = null;
            ElapsedTime = 0f;
            TimeEnded = null;
        }
        
        protected virtual void OnTimeEnded()
        {
            TimeEnded?.Invoke();
        }
    }
}

