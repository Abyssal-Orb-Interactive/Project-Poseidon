using System;

namespace Base
{
    public class CounterFloat : CounterBase<float>

    {
        public CounterFloat(float initialValue, float targetValue, Func<float> function) : base(initialValue,
            targetValue, function)
        {
            NatureOfCounting = FunctionNatureDeterminant.DetermineFloatFunctionNature(function);
            if (!IsCounterValid()) throw new ArgumentException("Values of this counter is invalid");
        }

        public override bool IsCounterValid()
        {
            return _function != null && ((NatureOfCounting == NatureOfFunction.Increasing && CurrentValue < TargetValue) ||
                                         (NatureOfCounting == NatureOfFunction.Decreasing && CurrentValue > TargetValue));
        }

        public override void CalculateNextValue()
        {
            CurrentValue += _function(); 
            OnValueChanged(); 
            if (!IsCounterValid()) OnTargetReached();
        }

        public override void IncreaseTargetValue(float delta)
        {
            if (delta < 0f) throw new ArgumentException("You can't Increase Target Value with negative delta");

            TargetValue += delta;
        }

        public override void DecreaseTargetValue(float delta)
        {
            if(delta > 0f) throw new ArgumentException("You can't Decrease Target Value with positive delta");

            TargetValue += delta;
        }
    }
}