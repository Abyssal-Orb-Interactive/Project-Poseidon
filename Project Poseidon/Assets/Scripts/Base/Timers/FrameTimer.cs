namespace Base.Timers
{
    public class FrameTimer : Timer
    {
        public FrameTimer(float delayTimeInSeconds) : base(delayTimeInSeconds, TimeInvoker.Instance.GetDeltaTime)
        {
        }

        public override void Subscribe()
        {
            TimeInvoker.Instance.FrameUpdated += OnTimerTick;
        }

        public override void Unsubscribe()
        {
            TimeInvoker.Instance.FrameUpdated -= OnTimerTick;
        }
    }
}