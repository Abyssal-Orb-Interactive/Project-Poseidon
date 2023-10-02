using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Source.Turn_State_Machine 
{ 
    public class TimerUniTaskAdapter
    {
        private readonly Timer _timer;

        public TimerUniTaskAdapter(float waitTimeInSeconds)
        {
            _timer = new Timer(waitTimeInSeconds, () => UnityEngine.Time.deltaTime);
        }

        public float WaitTimeInSeconds => _timer.WaitTimeInSeconds;
        public float ElapsedTime => _timer.ElapsedTime;
        
        public async UniTask StartTimerAsync()
        {
            await UniTask.SwitchToMainThread();
            await StartTimerInternalAsync();
        }
        
        private async Task StartTimerInternalAsync()
        {
            var completionSource = new UniTaskCompletionSource();

            _timer.TimeEnded += () =>
            {
                completionSource.TrySetResult();
            };

            _timer.StartTimerAsync();

            await completionSource.Task;
        }

        public void StopTimer()
        {
            
            _timer.StopTimer();
        }

        public void Destroy()
        {
            StopTimer();
            _timer.Destroy();
        }

        public async UniTask WaitAsync()
        {
            await StartTimerAsync();
        }

        public event Action TimeEnded
        {
            add => _timer.TimeEnded += value;
            remove => _timer.TimeEnded -= value;
        }
    }
}
