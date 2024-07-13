using System;
using Base.Timers;

namespace Source
{
    public class TimeToTurnTracker : IDisposable
    {
        private readonly Timer _timer;
        private TimeInvoker _timeInvoker;

        private event Action TimeEnded;
        private event Action TimePaused;
        private event Action TimeResumed;
        private event Action TimeTicked;

        public float RemainingTime => _timer.RemainingTime;
        public float DelayTime => _timer.DelayTimeInSeconds;

        public TimeToTurnTracker(float timeToTurnInSeconds)
        {
            _timeInvoker = TimeInvoker.Instance;
            TimerFabric.Initialize(_timeInvoker);
            _timer = TimerFabric.Create(TimerType.FrameTimer, timeToTurnInSeconds);
            Subscribe();
        }

        private void Subscribe()
        {
            _timer.TimerFinished += OnTimerEnded;
            _timer.TimerPaused += OnTimePaused;
            _timer.TimerResumed += OnTimeResumed;
            _timer.TimerTick += OnTimeTicked;
        }
        
        private void UnSubscribe()
        {
            _timer.TimerFinished -= OnTimerEnded;
            _timer.TimerPaused -= OnTimePaused;
            _timer.TimerResumed -= OnTimeResumed;
            _timer.TimerTick -= OnTimeTicked;
        }
        
        public void Start()
        {
            _timer.Start();
        }

        private void Stop()
        {
            _timer.Stop();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Pause()
        {
            _timer.Pause();
        }

        public void Resume()
        {
            _timer.Resume();
        }
        
        public void UpdateTime()
        {
            _timeInvoker.UpdateTimer();
        }

        private void OnTimerEnded()
        {
            TimeEnded?.Invoke();
            Restart();
        }

        private void OnTimePaused()
        {
            TimePaused?.Invoke();
        }

        private void OnTimeResumed()
        {
            TimeResumed?.Invoke();
        }

        private void OnTimeTicked()
        {
            TimeTicked?.Invoke();
        }
        
        public void SubscribeToTimeToTurnEnded(Action action)
        {
            TimeEnded += action;
        }

        public void UnSubscribeToTimeToTurnEnded(Action action)
        {
            TimeEnded -= action;
        }
        
        public void SubscribeToTimePaused(Action action)
        {
            TimePaused += action;
        }

        public void UnSubscribeToTimePaused(Action action)
        {
            TimePaused -= action;
        }
        
        public void SubscribeToTimeResumed(Action action)
        {
            TimeResumed += action;
        }

        public void UnSubscribeToTimeResumed(Action action)
        {
            TimeResumed -= action;
        }
        
        public void SubscribeToTimeTicked(Action action)
        {
            TimeTicked += action;
        }

        public void UnSubscribeToTimeTicked(Action action)
        {
            TimeTicked -= action;
        }

        public void Dispose()
        {
            UnSubscribe();
            _timer?.Dispose();
            _timeInvoker = null;
            TimeEnded = null;
            TimePaused = null;
            TimeResumed = null;
            TimeTicked = null;
            GC.SuppressFinalize(this);
        }
    }
}
