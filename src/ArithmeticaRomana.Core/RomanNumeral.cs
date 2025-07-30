using System.Text;

namespace ArithmeticaRomana.Core
{
    /// <summary>
    /// Represents a Roman numeral and provides methods to parse a string representation
    /// </summary>
    public readonly struct RomanNumeral
    {
        /// <summary>
        /// Array of tuples containing Roman numeral strings and their corresponding integer values
        /// in descending order of value.
        /// </summary>
        private static readonly (string roman, int value)[] _romanNumerals =
            [
                ("M", 1000),
                ("CM", 900),
                ("D", 500),
                ("CD", 400),
                ("C", 100),
                ("XC", 90),
                ("L", 50),
                ("XL", 40),
                ("X", 10),
                ("IX", 9),
                ("V", 5),
                ("IV", 4),
                ("I", 1)
            ];

        /// <summary>
        /// Searches for the value of a Roman numeral array.
        /// </summary>
        /// <param name="roman">roman numeral to search for</param>
        /// <returns>returns the corresponding value, if not found 0</returns>
        private static int FindValue(string roman)
        {
            for (int i = 0; i < _romanNumerals.Length; i++)
            {
                if (_romanNumerals[i].roman.Equals(roman))
                    return _romanNumerals[i].value;
            }
            return 0;
        }

        private readonly int _value;

        /// <summary>
        /// Constructor for <see cref="RomanNumeral"/>.
        /// </summary>
        /// <param name="value">value between 1 and 3999</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RomanNumeral(int value)
        {
            if (value < 1 || value > 3999)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 1 and 3999.");
            _value = value;
        }

        /// <summary>
        /// returns the integer value of <see cref="RomanNumeral"/>
        /// </summary>
        public int AsInteger => _value;

        /// <summary>
        /// returns the Roman numeral representation of the value.
        /// </summary>
        /// <returns>roman numeral representation</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string AsRoman()
        {
            // We iterate through the _romanNumerals, we check
            // how many times the current value fits into the value 
            // if the division is greater than 3, we throw an exception because
            // the value is too high to be represented in Roman numerals,
            // otherwise we append to the StringBuilder
            StringBuilder sb = new();
            int value = _value;
            for (int i = 0; i < _romanNumerals.Length; i++)
            {
                var numeralValue = _romanNumerals[i];
                var division = value / numeralValue.value;
                if (division > 3)
                    throw new ArgumentOutOfRangeException(nameof(_value), $"Value {_value} exceeds the maximum representable Roman numeral.");

                if (division > 0)
                {
                    value -= numeralValue.value * division;
                    for (int j = 0; j < division; j++)
                    {
                        sb.Append(numeralValue.roman);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tries to parse a Roman numeral string into a <see cref="RomanNumeral"/>.
        /// </summary>
        /// <param name="roman">roman numeral for example: "IX"</param>
        /// <param name="result">readonly struct</param>
        /// <returns>true if successful</returns>
        public static bool TryParse(string roman, out RomanNumeral result)
        {
            // If the string is null or white space, we can't parse it
            if (string.IsNullOrWhiteSpace(roman))
            {
                result = default;
                return false;
            }

            // We move the string from right to left, we check the current character
            // and the previous character and we apply subtraction rules and
            // add to the total value of the roman numeral
            int total = 0;
            int numeralRepetition = 0;
            for (int i = roman.Length - 1; i >= 0; i--)
            {
                // We need to check the previous char and the current char
                // so we can look if the subtraction rules are valid
                string? previousChar = i == roman.Length - 1 ? null : roman[i + 1].ToString();
                string currentChar = roman[i].ToString();

                // Check if character is a valid roman character
                // If we can't find a value, that means its not a valid character
                int currentValue = FindValue(currentChar);
                if (currentValue == 0)
                {
                    result = default;
                    return false;
                }

                // Check if subtraction is valid
                int previousValue = previousChar == null ? 0 : FindValue(previousChar);
                if (!SubtractionRulesValid(currentValue, previousValue))
                {
                    result = default;
                    return false;
                }

                // Count up if the numeral repeats itself and check repetition rules
                numeralRepetition = (currentChar == previousChar) ? numeralRepetition + 1 : 0;
                if (!RepetitionRulesValid(currentChar, numeralRepetition))
                {
                    result = default;
                    return false;
                }

                // Calculate the total value
                if (currentValue < previousValue)
                    // If the current value is smaller than the previous value,
                    // we need to subtract it from the total
                    total -= currentValue;
                else
                    // Otherwise, we can add it to the total
                    total += currentValue;
            }
            result = new RomanNumeral(total);
            return true;
        }


        /// <summary>
        /// Checks if the subtraction rules are valid for the given values.
        /// </summary>
        /// <param name="currentValue">int value of current numeral</param>
        /// <param name="previousValue">int value of the previous numeral</param>
        /// <returns>true if rules are valid</returns>
        private static bool SubtractionRulesValid(int currentValue, int previousValue)
        {
            // If the current value is greater than or equal to the previous value,
            // it's not a subtraction scenario, so it's inherently "valid" for this rule.
            // This handles cases like VI (6), where V is followed by I.
            if (currentValue >= previousValue)
                return true;

            // If we've reached here, it means currentValue < previousValue, indicating a potential subtraction.
            // Only I (1), X (10), and C (100) can be subtracted.
            if (currentValue != 1 && currentValue != 10 && currentValue != 100)
                return false;

            // Check for the specific valid subtraction pairs.
            // If it's not one of these exact pairs, it's invalid.
            if (currentValue == 1) // I can only subtract from V or X
            {
                if (previousValue == 5 || previousValue == 10)
                    return true;
            }
            else if (currentValue == 10) // X can only subtract from L or C
            {
                if (previousValue == 50 || previousValue == 100)
                    return true;
            }
            else if (currentValue == 100) // C can only subtract from D or M
            {
                if (previousValue == 500 || previousValue == 1000)
                    return true;
            }

            // If none of the specific valid subtraction conditions were met, it's an invalid subtraction.
            return false;
        }

        /// <summary>
        /// V, L and D can not be repeated.
        /// </summary>
        private static readonly string[] _forbiddenNumerals = { "V", "L", "D" };

        /// <summary>
        /// Checks if the repetition rules are valid for the given numeral.
        /// <para>V, L and D can not be repeated</para>
        /// <para>I, X, C and M can be used up to 3 times</para>
        /// </summary>
        /// <param name="numeral">roman numeral</param>
        /// <param name="repetitionCount">numeral repeated</param>
        /// <returns>true if rules are valid</returns>
        private static bool RepetitionRulesValid(string numeral, int repetitionCount)
        {
            // V, L and D can only be used once
            if (_forbiddenNumerals.Contains(numeral) && repetitionCount >= 1)
                return false;

            // I, X, C and M can be used up to 3 times
            if (repetitionCount > 2)
                return false;

            return true;
        }
    }
}
