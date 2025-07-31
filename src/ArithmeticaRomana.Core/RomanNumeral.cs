using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.Core.Parser;
using System.Numerics;

namespace ArithmeticaRomana.Core
{
    /// <summary>
    /// Represents a Roman numeral
    /// </summary>
    public readonly struct RomanNumeral : IComparable<RomanNumeral>, IEquatable<RomanNumeral>, IMinMaxValue<RomanNumeral>
    {
        private static readonly IRomanNumeralFormatter _defaultFormatter = new VinculumRomanNumeralFormatter();
        private static readonly IRomanNumeralParser _defaultParser = new VinculumRomanNumeralParser();

        private readonly int _value;
        private static readonly int _max = int.MaxValue;
        private static readonly int _min = 1;
        private static readonly RomanNumeral _maxValue = new(_max);
        private static readonly RomanNumeral _minValue = new(_min);
        public static RomanNumeral MaxValue => _maxValue;
        public static RomanNumeral MinValue => _minValue;

        /// <summary>
        /// Constructor for <see cref="RomanNumeral"/>.
        /// </summary>
        /// <param name="value">value between 1 and int.MaxValue</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RomanNumeral(int value)
        {
            if (value < _min || value > _max)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and int.MaxValue.");
            _value = value;
        }

        /// <summary>
        /// returns the integer value of <see cref="RomanNumeral"/>
        /// </summary>
        public int AsInteger => _value;

        /// <summary>
        /// returns the Roman numeral representation of this <see cref="RomanNumeral"/>.
        /// </summary>
        /// <returns>Roman numeral representation</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string AsRomanRepresentation(IRomanNumeralFormatter? formatter = null)
        {
            formatter ??= _defaultFormatter;
            return formatter.Format(_value);
        }

        /// <summary>
        /// Tries to parse a Roman numeral string into a <see cref="RomanNumeral"/>.
        /// </summary>
        /// <param name="roman">roman numeral representation</param>
        /// <param name="result">if succesful contains the RomanNumeral</param>
        /// <param name="parser">optional parser to use, defaults to <see cref="VinculumRomanNumeralParser"/></param>
        /// <returns>true if successful</returns>
        public static bool TryParse(string roman, out RomanNumeral result, IRomanNumeralParser? parser = null)
        {
            parser ??= _defaultParser;
            var parseResult = parser.Parse(roman);
            result = parseResult.RomanNumeral ?? default;
            return parseResult.IsSuccess;
        }

        public int CompareTo(RomanNumeral other)
        {
            return _value.CompareTo(other.AsInteger);
        }

        public bool Equals(RomanNumeral other)
        {
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (obj is RomanNumeral numeral)
                return Equals(numeral);

            if (obj is int intValue)
                return _value == intValue;

            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return AsRomanRepresentation();
        }

        #region operators
        public static RomanNumeral operator +(RomanNumeral left, RomanNumeral right)
        {
            return new RomanNumeral(left._value + right._value);
        }

        public static RomanNumeral operator -(RomanNumeral left, RomanNumeral right)
        {
            return new RomanNumeral(left._value - right._value);
        }

        public static RomanNumeral operator *(RomanNumeral left, RomanNumeral right)
        {
            return new RomanNumeral(left._value * right._value);
        }

        public static RomanNumeral operator /(RomanNumeral left, RomanNumeral right)
        {
            return new RomanNumeral(left._value / right._value);
        }

        public static RomanNumeral operator ++(RomanNumeral numeral)
        {
            return new RomanNumeral(numeral.AsInteger + 1);
        }

        public static RomanNumeral operator --(RomanNumeral numeral)
        {
            return new RomanNumeral(numeral.AsInteger - 1);
        }

        public static bool operator ==(RomanNumeral left, RomanNumeral right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RomanNumeral left, RomanNumeral right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(RomanNumeral left, RomanNumeral right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(RomanNumeral left, RomanNumeral right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(RomanNumeral left, RomanNumeral right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(RomanNumeral left, RomanNumeral right)
        {
            return left.CompareTo(right) >= 0;
        }
        #endregion
    }
}
