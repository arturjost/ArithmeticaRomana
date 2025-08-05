
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
        /// <param name="modifier">
        /// instead of providing all different numerals, you can provide the base numerals and the special modifier of <br />
        /// the specific notation for example for Vinculum: \u0305
        /// </param>
        public RomanNumeralMap(string[] romanNumerals, string? modifier = null)
        {
            if (romanNumerals.Length == 0)
            {
                _baseRomanTokens = ImmutableList<RomanNumeralToken>.Empty;
                _romanTokens = ImmutableList<RomanNumeralToken>.Empty;
                return;
            }

            List<RomanNumeralToken> baseTokens = [];
            List<RomanNumeralToken> subtractionTokens = [];

            // this loop for base tokens
            int modifierLoop = 0;
            int value = 1;
            for (int i = 0; i < romanNumerals.Length; i++)
            {
                // add the provided base tokens, if we are in a modifer loop we will add the token + modifier
                var romanNumeral = AddModifier(romanNumerals[i], modifier, modifierLoop);
                baseTokens.Add(new RomanNumeralToken(romanNumeral, value));

                // we multiply to get to the next value in the progression
                if (i % 2 == 0)
                    value *= 5; // V, L, D, etc...
                else
                    value *= 2; // X, C, M, etc...

                // if a modifer is provided we will need to generate all other base tokens
                if (i == romanNumerals.Length - 1 && modifier != null && modifierLoop < 2)
                {
                    modifierLoop += 1;
                    baseTokens.Add(new RomanNumeralToken(AddModifier(romanNumerals[0], modifier, modifierLoop), value / 5));
                    i = 0;
                }
            }

            // now for the special subtraction tokens IV, IX etc.
            int modifierSize = modifierLoop;
            string subtractionNumeral = romanNumerals[0];
            value = 1;
            int subtractionValue = value;
            for (int i = 0; i < romanNumerals.Length - 1; i++)
            {
                // we multiply to get to the next value in the progression
                if (i % 2 == 0)
                {
                    value *= 5;
                    subtractionTokens.Add(new RomanNumeralToken(subtractionNumeral + AddModifier(romanNumerals[i + 1], modifier, modifierSize - modifierLoop), (value - subtractionValue)));
                }
                else if (i != 0)
                {
                    value *= 2;
                    int modifierCount = modifierSize - modifierLoop;
                    subtractionTokens.Add(new RomanNumeralToken(subtractionNumeral + AddModifier(romanNumerals[i + 1], modifier, modifierSize - modifierLoop), (value - subtractionValue)));

                    subtractionNumeral = i == romanNumerals.Length - 2 ? AddModifier(romanNumerals[0], modifier, modifierSize + 1 - modifierLoop) : AddModifier(romanNumerals[i + 1], modifier, modifierSize - modifierLoop);
                    subtractionValue = value;
                }

                if (i == romanNumerals.Length - 2 && modifierLoop > 0)
                {
                    modifierLoop--;
                    i = -1;
                }
            }

            _baseRomanTokens = baseTokens.OrderByDescending(token => token.NumeralValue).ThenBy(x => x.RomanNumeral).ToImmutableList();
            _romanTokens = baseTokens.Concat(subtractionTokens).OrderByDescending(token => token.NumeralValue).ThenBy(x => x.RomanNumeral).ToImmutableList();
        }

        /// <summary>
        /// Adds the modifier to a numeral as many times as specified in <paramref name="count"/>.
        /// </summary>
        /// <param name="numeral">The Roman numeral</param>
        /// <param name="modifier">Modifier to add</param>
        /// <param name="count">How many times should the modifier be added?</param>
        /// <returns>Modified Roman numeral</returns>
        private static string AddModifier(string numeral, string? modifier, int count)
        {
            if (modifier is null || count == 0)
                return numeral;

            string result = numeral;
            for (int i = 0; i < count; i++)
            {
                result += modifier;
            }
            return result;
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
