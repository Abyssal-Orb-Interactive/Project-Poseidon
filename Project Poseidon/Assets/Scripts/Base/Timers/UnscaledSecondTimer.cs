using System;

namespace Base.Timers
{
    public class UnscaledSecondTimer : Timer
    {
        private TimeInvoker _invoker;
        
        public UnscaledSecondTimer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker) : base(delayTimeInSeconds, timeSource)
        {
            _invoker = invoker;
        }

        public override void Subscribe()
        {
            _invoker.UnscaledSecondUpdated += OnTimerTick;
        }

        public override void Unsubscribe()
        {
            _invoker.UnscaledSecondUpdated -= OnTimerTick;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _invoker = null;
        }
    }
}