using System;
using GalacticLib.Math.Numerics.Formulas;
using GalacticLib.Math.Numerics.Numbers;

namespace GalacticLib.Math.Numerics.Formulas
{
    internal class CSFormulaPart {
        private static ArgumentOutOfRangeException _Operator_Invalid(CSOperator _operator)
            => new($"The provided operator is not valid in this context ({_operator}).");


        public Number<double> Value { get; }
#nullable enable
        public Number<double>? Value2 { get; }
#nullable restore
        public CSFormulaPart(CSOperator _operator, Number<double> value, Number<double>? value2 = null) {

            if ((value2 == null && !_operator.IsUnary)
                || (value2 != null && _operator.IsUnary)) {
                throw _Operator_Invalid(_operator);
            }
        }
    }
}
