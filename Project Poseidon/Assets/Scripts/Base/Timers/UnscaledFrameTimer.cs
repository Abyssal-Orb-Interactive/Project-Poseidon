using System;

namespace Base.Timers
{
    public class UnscaledFrameTimer : Timer
    {
        private TimeInvoker _invoker;
        
        public UnscaledFrameTimer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker) : base(delayTimeInSeconds, timeSource)
        {
            _invoker = invoker;
        }

        protected override void Subscribe()
        {
            _invoker.UnscaledFrameUpdated += OnTimerTick;
        }

        protected override void Unsubscribe()
        {
            _invoker.UnscaledFrameUpdated -= OnTimerTick;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _invoker = null;
        }
    }
}