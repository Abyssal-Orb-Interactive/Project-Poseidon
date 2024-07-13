using System;

namespace Base
{
    public abstract class CounterBase<T> : IDisposable
    {
        public T StartValue { get; protected set; }
        public T TargetValue { get; protected set; }
        public T CurrentValue { get; protected set; }
        public NatureOfFunction NatureOfCounting { get; protected set; }
        
        protected Func<T> _function;
        
        public event Action TargetReached;
        public event Action ValueChanged;

        protected CounterBase(T initialValue, T targetValue, Func<T> function)
        {
            StartValue = initialValue;
            CurrentValue = StartValue;
            TargetValue = targetValue;
            _function = function;
        }

        public abstract bool IsCounterValid();

        public abstract void CalculateNextValue();

        public abstract void IncreaseTargetValue(T delta);
        public abstract void DecreaseTargetValue(T delta);
        
        public virtual void Reset()
        {
            CurrentValue = StartValue;
        }

        protected virtual void OnTargetReached()
        {
            TargetReached?.Invoke();
            Reset();
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }
        
        public void Dispose()
        {
            ValueChanged = null;
            TargetReached = null;
            StartValue = default;
            TargetValue = default;
            Reset();
            _function = null;
            NatureOfCounting = NatureOfFunction.Constant;
            GC.SuppressFinalize(this);
        }
        
    }
}