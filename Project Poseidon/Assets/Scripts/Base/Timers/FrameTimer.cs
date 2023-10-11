using System;
using UnityEditorInternal;

namespace Base.Timers
{
    public class FrameTimer : Timer
    {
        private TimeInvoker _invoker;
        
        public FrameTimer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker) : base(delayTimeInSeconds, timeSource)
        {
            _invoker = invoker;
        }

        public override void Subscribe()
        {
            _invoker.FrameUpdated += OnTimerTick;
        }

        public override void Unsubscribe()
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