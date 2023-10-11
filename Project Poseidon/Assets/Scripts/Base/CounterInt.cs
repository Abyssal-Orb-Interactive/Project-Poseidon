using System;

namespace Base
{
    public class CounterInt : CounterBase<int>

    {
        public CounterInt(int initialValue, int targetValue, Func<int> function) : base(initialValue, targetValue, function)
        {
            NatureOfCounting = FunctionNatureDeterminant.DetermineIntFunctionNature(function);
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
        
            if(!IsCounterValid()) OnTargetReached();
        }

        public override void IncreaseTargetValue(int delta)
        {
            if (delta < 0) throw new ArgumentException("You can't Increase Target Value with negative delta");

            TargetValue += delta;
        }

        public override void DecreaseTargetValue(int delta)
        {
            if(delta > 0f) throw new ArgumentException("You can't Decrease Target Value with positive delta");

            TargetValue += delta;
        }
    }
}