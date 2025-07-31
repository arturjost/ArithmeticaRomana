
using System.Collections.Immutable;

namespace ArithmeticaRomana.Core.Internal
{
    /// <summary>
    /// <para>This implementation generates valid tokens for all provided strings.</para>
    /// <para>Designed for Apostrophus, Vinculum or three-sided box.</para>
    /// <para>It creates two <seealso cref="ImmutableList"/> for holding the <seealso cref="RomanNumeralToken"/>.</para>
    /// </summary>
    internal class RomanNumeralMap : IRomanNumeralMap
    {
        private readonly ImmutableList<RomanNumeralToken> _baseRomanTokens;
        private readonly ImmutableList<RomanNumeralToken> _romanTokens;

        /// <summary>
        /// <para>This constructor generates all possible Roman numerals tokens. You only have to provide the base numerals.</para>
        /// <para>Important: provide tokens from small to big. Correct: "I", "V", "X" Wrong: "M", "D", "C"</para>
        /// </summary>
        /// <param name="romanNumerals">"I", "V", "X", etc.</param>
        public RomanNumeralMap(params string[] romanNumerals)
        {
            List<RomanNumeralToken> baseTokens = [];
            List<RomanNumeralToken> subtractionTokens = [];
            int value = 1;
            string subtractionNumeral = string.Empty;
            int subtractionValue = 0;
            for (int i = 0; i < romanNumerals.Length; i++)
            {
                if (i == 0)
                {
                    subtractionNumeral = romanNumerals[i];
                    subtractionValue = 1;
                }

                baseTokens.Add(new RomanNumeralToken(romanNumerals[i], value));

                // start adding IV, IX, XL, XC, CD, CM  
                // stop if we reach the end of the array
                if (i < romanNumerals.Length - 1)
                {
                    string numeral = subtractionNumeral + romanNumerals[i + 1];
                    subtractionTokens.Add(new RomanNumeralToken(numeral,
                        (value * (i % 2 == 0 ? 5 : 2)) - subtractionValue));
                }

                if (i % 2 == 0)
                {
                    value *= 5; // V, L, D, etc...
                }
                else
                {
                    value *= 2; // X, C, M, etc...
                    subtractionValue = value;
                    subtractionNumeral = i + 1 < romanNumerals.Length ? romanNumerals[i + 1] : string.Empty;
                }
            }

            _baseRomanTokens = baseTokens.OrderByDescending(token => token.NumeralValue).ToImmutableList();
            _romanTokens = baseTokens.Concat(subtractionTokens).OrderByDescending(token => token.NumeralValue).ToImmutableList();
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
