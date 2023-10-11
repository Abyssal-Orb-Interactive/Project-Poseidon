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

        public override void Subscribe()
        {
            _invoker.UnscaledFrameUpdated += OnTimerTick;
        }

        public override void Unsubscribe()
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