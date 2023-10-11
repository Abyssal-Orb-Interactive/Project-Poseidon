using System;

namespace Base.Timers
{
    public class SecondTimer : Timer
    {
        private TimeInvoker _invoker;
        public SecondTimer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker) : base(delayTimeInSeconds, timeSource)
        {
            _invoker = invoker;
        }

        protected override void Subscribe()
        {
            _invoker.SecondUpdated += OnTimerTick;
        }

        protected override void Unsubscribe()
        {
            _invoker.SecondUpdated -= OnTimerTick;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _invoker = null;
        }
    }
}