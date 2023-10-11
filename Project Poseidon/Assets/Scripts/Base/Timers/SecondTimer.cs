namespace Base.Timers
{
    public class SecondTimer : Timer
    {
        public SecondTimer(float delayTimeInSeconds) : base(delayTimeInSeconds, TimeInvoker.Instance.GetTimeScale)
        {
        }

        public override void Subscribe()
        {
            TimeInvoker.Instance.SecondUpdated += OnTimerTick;
        }

        public override void Unsubscribe()
        {
            TimeInvoker.Instance.SecondUpdated -= OnTimerTick;
        }
    }
}