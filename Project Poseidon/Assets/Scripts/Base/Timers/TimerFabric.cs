using System;

namespace Base.Timers
{
    public static class TimerFabric
    {
        private const float ONE_SECOND = 1f;
        
        private static readonly TimeInvoker _invoker = TimeInvoker.Instance;

        public static Timer Create(TimerType type, float delayTimeInSeconds)
        {
            Timer timer = type switch
            {
                TimerType.FrameTimer => new FrameTimer(delayTimeInSeconds, _invoker.GetDeltaTime, _invoker),
                TimerType.UnscaledFrameTimer => new UnscaledFrameTimer(delayTimeInSeconds, _invoker.GetDeltaTime,
                    _invoker),
                TimerType.SecondTimer => new SecondTimer(delayTimeInSeconds, () => ONE_SECOND, _invoker),
                TimerType.UnscaledSecondTimer =>
                    new UnscaledSecondTimer(delayTimeInSeconds, () => ONE_SECOND, _invoker),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return timer;
        }
    }
}
