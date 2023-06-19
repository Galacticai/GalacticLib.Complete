using System;
using GalacticLib.Math.Numerics.Formulas;

namespace GalacticLib.Math.Numerics.Formulas
{
    public struct CSOperator {
        private static ArgumentOutOfRangeException _Operator_Invalid(string s, bool isBetween2Values)
            => new($"The provided operator is not a valid C# operator ({(isBetween2Values ? $"x {s} y" : $"{s}x")}).");
        private static NotImplementedException _CSOperator_NotImplimented(Sign csOperator)
            => new($"The provided C# operator is not implimented ({csOperator}). Please contact the developer.");


        public enum Sign {
            /// <summary> x + y </summary>
            Add,
            /// <summary> x - y </summary>
            Subtract,
            /// <summary> x * y </summary>
            Multiply,
            /// <summary> x / y </summary>
            Divide,

            /// <summary> x % y </summary>
            Modulo,

            /// <summary> +x </summary>
            Positive,
            /// <summary> -x </summary>
            Negative,

            /// <summary> ~x </summary>
            BitInverse,

            /// <summary> x &lt;&lt; y </summary>
            BitShiftLeft,
            /// <summary> x >> y </summary>
            BitShiftRight,
            //? Not available in C# 9
            //! /// <summary> x >>> y </summary>
            //! BitShiftRightUnsigned
        }


        #region Shortcuts

        public bool IsUnary => SignIsUnary(Value);

        #endregion
        #region Methods
        /// <summary> Convert a known operator string to <see cref="Sign"/> </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <returns> The equiveland <see cref="Sign"/>,
        /// <br/> otherwise <see cref="ArgumentOutOfRangeException"/> is thrown if the operator is invalid/unsupported </returns>
        public static Sign SignFromString(string s, bool isBetween2Values)
            => s switch {
                "+" => isBetween2Values ? Sign.Add : Sign.Positive,
                "-" => isBetween2Values ? Sign.Subtract : Sign.Negative,
                "*" => isBetween2Values ? Sign.Multiply : throw _Operator_Invalid(s, isBetween2Values),
                "/" => isBetween2Values ? Sign.Divide : throw _Operator_Invalid(s, isBetween2Values),
                "%" => isBetween2Values ? Sign.Modulo : throw _Operator_Invalid(s, isBetween2Values),
                "~" => isBetween2Values ? throw _Operator_Invalid(s, isBetween2Values) : Sign.BitInverse,
                "<<" => isBetween2Values ? Sign.BitShiftLeft : throw _Operator_Invalid(s, isBetween2Values),
                ">>" => isBetween2Values ? Sign.BitShiftRight : throw _Operator_Invalid(s, isBetween2Values),
                //? Not available in C# 9
                //! ">>>" => isBetween2Values ? Sign.BitShiftRightUnsigned : throw Operator_Invalid(s, isBetween2Values),

                _ => throw _Operator_Invalid(s, isBetween2Values)
            };
        /// <summary> Check whether a <see cref="Sign"/> is meant to be between 2 values (x ... y) or not (...x) </summary>
        /// <returns> true if the <see cref="Sign"/> is meant to be </returns>
        public static bool SignIsUnary(Sign value)
            => value switch {
                Sign.Add
                or Sign.Subtract
                or Sign.Multiply
                or Sign.Divide
                or Sign.Modulo
                or Sign.BitShiftLeft
                or Sign.BitShiftRight => false,

                Sign.Positive
                or Sign.Negative
                or Sign.BitInverse => true,
                //? Not available in C# 9
                //! or Sign.BitShiftRightUnsigned => false,

                _ => throw _CSOperator_NotImplimented(value)
            };

        #endregion
        #region Overrides

        public override string ToString()
            => Value switch {
                Sign.Add
                or Sign.Positive => "+",
                Sign.Subtract
                or Sign.Negative => "-",
                Sign.Multiply => "*",
                Sign.Divide => "/",

                Sign.Modulo => "%",

                Sign.BitInverse => "~",
                Sign.BitShiftLeft => "<<",
                Sign.BitShiftRight => ">>",
                //? Not available in C# 9
                //!  Sign.BitShiftRightUnsigned => ">>>",

                _ => throw _CSOperator_NotImplimented(Value)
            };

        #endregion
        #region Operators

        public static implicit operator string(CSOperator csOperator)
            => csOperator.ToString();
        public static implicit operator CSOperator(Sign sign)
            => new(sign);
        public static implicit operator Sign(CSOperator csOperator)
            => csOperator.Value;

        #endregion
        #region this object
        public Sign Value { get; }
        public CSOperator(Sign value) {
            Value = value;
        }
        public CSOperator(string rawValue, bool isBetween2Values) {
            Value = SignFromString(rawValue, isBetween2Values);
        }
        #endregion
    }

}
