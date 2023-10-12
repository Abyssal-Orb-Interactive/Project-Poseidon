using System;

namespace Base.Timers
{
    public static class TimerFabric
    {
        private const float ONE_SECOND = 1f;

        private static TimeInvoker _invoker;

        public static void Initialize(TimeInvoker invoker)
        {
            _invoker = invoker;
        }
        
        public static Timer Create(TimerType type, float delayTimeInSeconds)
        {
            if (_invoker == null) throw new ArgumentException("Before creating timers, you must initialize TimerFabric");
            
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
