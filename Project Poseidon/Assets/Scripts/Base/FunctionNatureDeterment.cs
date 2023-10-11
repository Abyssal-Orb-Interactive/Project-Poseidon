using System;

namespace Base
{
    public static class FunctionNatureDeterminant
    {
        public static NatureOfFunction DetermineFloatFunctionNature(Func<float> function)
        {
            const float epsilon = 1e-6f;
            var nature = NatureOfFunction.Constant;

            var startValue = function();
            var nextValue = startValue + function();

            if (Math.Abs(nextValue - startValue) < epsilon)
            {
                nature = NatureOfFunction.Constant;
            }
            else if (nextValue > startValue)
            {
                nature = NatureOfFunction.Increasing;
            }
            else if (nextValue < startValue)
            {
                nature = NatureOfFunction.Decreasing;
            }

            return nature;
        }
        
        public static NatureOfFunction DetermineIntFunctionNature(Func<int> function)
        {
            const float epsilon = 1e-6f;
            var nature = NatureOfFunction.Constant;

            var startValue = function();
            var nextValue = startValue + function();

            if (Math.Abs(nextValue - startValue) < epsilon)
            {
                nature = NatureOfFunction.Constant;
            }
            else if (nextValue > startValue)
            {
                nature = NatureOfFunction.Increasing;
            }
            else if (nextValue < startValue)
            {
                nature = NatureOfFunction.Decreasing;
            }

            return nature;
        }
    }
}