using System.Collections.Immutable;

namespace ArithmeticaRomana.Core.Internal
{
    /// <summary>
    /// Implementation of <see cref="IRomanEncoding"/> for Vinculum notation of Roman numerals.
    /// <para>It creates a <see cref="Dictionary{TKey, TValue}"/> and two <see cref="ImmutableList"/> with <seealso cref="RomanNumeralToken"/></para>
    /// </summary>
    internal class VinculumEncoding : IRomanEncoding
    {
        private readonly Dictionary<string, int> _numeralDictionary;
        private readonly ImmutableList<RomanNumeralToken> _baseRomanTokens;
        private readonly ImmutableList<RomanNumeralToken> _romanTokens;

        /// <summary>
        /// Constructor of <see cref="VinculumEncoding"/> it generates all tokens with method <seealso cref="GenerateNumeralDictionary"/>.
        /// </summary>
        public VinculumEncoding()
        {
            _numeralDictionary = [];
            GenerateNumeralDictionary();

            List<RomanNumeralToken> baseTokens = [];
            List<RomanNumeralToken> romanTokens = [];
            foreach (var numeral in _numeralDictionary.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
            {
                RomanNumeralToken token = new RomanNumeralToken(numeral.Key, numeral.Value);
                if (token.BaseValue == 1 || token.BaseValue == 5)
                {
                    baseTokens.Add(token);
                }
                romanTokens.Add(token);
            }
            _baseRomanTokens = [.. baseTokens];
            _romanTokens = [.. romanTokens];
        }

        /// <summary>
        /// <para>This function generates all possible Roman numerals tokens.</para>
        /// </summary>
        private void GenerateNumeralDictionary()
        {
            string vinculum = "\u0305";
            string[] numerals = ["I", "V", "X", "L", "C", "D", "M"];

            int vinculumRounds = 0;
            int value = 1;
            string subNumeral = numerals[0];
            int subValue = value;
            string curNumeral;
            for (int numeralIndex = 0; numeralIndex < numerals.Length; numeralIndex++)
            {
                curNumeral = numerals[numeralIndex];
                for (int vinculumIndex = 0; vinculumIndex < vinculumRounds; vinculumIndex++)
                {
                    curNumeral += vinculum;
                }
                if (subNumeral == string.Empty)
                    subNumeral = curNumeral;

                if (numeralIndex > 0 && numeralIndex % 2 == 0)
                {
                    value *= 2;
                    _numeralDictionary.Add($"{subNumeral}{curNumeral}", value - subValue);
                    subNumeral = curNumeral;
                    subValue = value;
                }
                else if (numeralIndex > 0)
                {
                    value *= 5;
                }
                _numeralDictionary.Add(curNumeral, value);

                if (curNumeral != subNumeral)
                {
                    _numeralDictionary.Add($"{subNumeral}{curNumeral}", value - subValue);
                }

                if (numeralIndex == numerals.Length - 1 && vinculumRounds < 2)
                {
                    vinculumRounds++;
                    subNumeral = string.Empty;
                    numeralIndex = -1;
                }
            }
        }

        public IEnumerable<RomanNumeralToken> BaseTokensByValue()
        {
            return _baseRomanTokens;
        }

        public IEnumerable<RomanNumeralToken> TokensByValue()
        {
            return _romanTokens;
        }
    }
}
