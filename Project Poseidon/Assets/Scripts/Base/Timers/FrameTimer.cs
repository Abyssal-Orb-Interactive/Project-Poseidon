using System;

namespace Base.Timers
{
    public class FrameTimer : Timer
    {
        private TimeInvoker _invoker;
        
        public FrameTimer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker) : base(delayTimeInSeconds, timeSource)
        {
            _invoker = invoker;
        }

        protected override void Subscribe()
        {
            _invoker.FrameUpdated += OnTimerTick;
        }

        protected override void Unsubscribe()
        {
            _invoker.FrameUpdated -= OnTimerTick;
        }

        public override void Dispose()
        {
            base.Dispose();
            _invoker = null;
        }
    }
}