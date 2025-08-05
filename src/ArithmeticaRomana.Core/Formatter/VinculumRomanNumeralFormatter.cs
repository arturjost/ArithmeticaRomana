using ArithmeticaRomana.Core.Internal;
using System.Text;


namespace ArithmeticaRomana.Core.Formatter
{
    /// <summary>
    /// Default implementation of the <see cref="IRomanNumeralFormatter"/>
    /// </summary>
    public class VinculumRomanNumeralFormatter : IRomanNumeralFormatter
    {
        public string Format(int value)
        {
            // We iterate through all known Roman numerals sorted by value
            // we check how many times the current value fits into the provided value
            // if the division is greater than 3, we throw an exception because
            // the value is too high to be represented in Roman numerals,
            // otherwise we append to the StringBuilder
            StringBuilder sb = new();
            var numerals = Internals.Vinculum.TokensByValue().ToList();
            for (int i = 0; i < numerals.Count; i++)
            {
                var romanNumeral = numerals[i];
                var division = value / romanNumeral.NumeralValue;
                if (division > 3)
                    throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} exceeds the maximum representable Roman numeral.");

                if (division > 0)
                {
                    value -= romanNumeral.NumeralValue * division;
                    if(sb.Length == 0 && i > 0 && i < numerals.Count - 1 && romanNumeral.BaseValue == 1 && romanNumeral.Exponent % 3 == 0) 
                    {
                        romanNumeral = numerals[i + 1];
                    }
                    for (int j = 0; j < division; j++)
                    {
                        sb.Append(romanNumeral.RomanNumeral);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
