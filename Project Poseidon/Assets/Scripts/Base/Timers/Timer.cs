using System;

namespace Base.Timers
{
    public delegate void TimerAction();
    public abstract class Timer : IDisposable
    {
        protected readonly CounterFloat Counter;

        public event TimerAction TimerFinished;
        public event TimerAction TimerPaused;
        public event TimerAction TimerResumed;
        public event TimerAction TimerTick;

        public float ElapsedTime => Counter.CurrentValue;
        public float DelayTimeInSeconds => Counter.TargetValue;
        public float RemainingTime => DelayTimeInSeconds - ElapsedTime;
        
        public bool IsPaused { get; protected set; }

        protected Timer(float delayTimeInSeconds, Func<float> timeSource)
        {
            Counter = new CounterFloat(0f, delayTimeInSeconds, timeSource);
            Counter.TargetReached += OnTimerEnds;
        }

        public void ReduceDelayTime(float reducingDelta)
        {
            if (reducingDelta < 0f) throw new ArgumentException("ReducingDelta must be positive");
            
            if (Counter.NatureOfCounting == NatureOfFunction.Increasing)
            {
                Counter.DecreaseTargetValue(-reducingDelta);
            }
            else
            {
                Counter.IncreaseTargetValue(reducingDelta);
            }
        }

        public void IncreaseDelayTime(float increasingDelta)
        {
            if (increasingDelta < 0f) throw new ArgumentException("IncreasingDelta must be positive");
            
            if (Counter.NatureOfCounting == NatureOfFunction.Increasing)
            {
                Counter.IncreaseTargetValue(increasingDelta);
            }
            else
            {
                Counter.DecreaseTargetValue(-increasingDelta);
            }
        }

        public void Start()
        {
            IsPaused = false;
            Subscribe();
        }

        public void Stop()
        {
            Unsubscribe();
            Counter.Reset();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Pause()
        {
            IsPaused = true;
            OnTimerPaused();
        }

        public void Resume()
        {
            IsPaused = false;
            OnTimerResumed();
        }
        
        public virtual void Dispose()
        {
            Counter.Dispose();
            Unsubscribe();
            TimerFinished = null;
            TimerPaused = null;
            TimerResumed = null;
            TimerTick = null;
            GC.SuppressFinalize(this);
        }

        private void OnTimerEnds()
        { 
            Stop();
            TimerFinished?.Invoke();
        }

        private void OnTimerPaused()
        {
            Unsubscribe();
            TimerPaused?.Invoke();
        }

        private void OnTimerResumed()
        {
            Subscribe();
            TimerResumed?.Invoke();
        }

        protected void OnTimerTick()
        {
            Counter.CalculateNextValue();
            TimerTick?.Invoke();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}


