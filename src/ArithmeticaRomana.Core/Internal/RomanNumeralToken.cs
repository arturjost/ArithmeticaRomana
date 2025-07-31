namespace ArithmeticaRomana.Core.Internal
{
    /// <summary>
    /// <para>Record for holding informations about a Roman numeral token</para> 
    /// <para>This doesn't validate <seealso cref="NumeralValue"/> with the <seealso cref="RomanNumeral"/>.</para> 
    /// <para>It just holds the value for the provided token, so it can be reused in other flavours.</para> 
    /// </summary>
    internal record RomanNumeralToken
    {
        /// <summary>
        /// RomanNumeral for example: "I", "V", "X"
        /// </summary>
        public string RomanNumeral { get; set; }

        /// <summary>
        /// The Value of the token for example: "I" => 1; "V" => 5
        /// </summary>
        public int NumeralValue { get; set; }

        /// <summary>
        /// The Base value of the token.
        /// Roman numerals have always a base value, thats repeats itself in power of 10
        /// for example: I(1), V(5), X(1), L(5), C(1), D(5), M(1)
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// The power of 10 to apply (the magnitude). Needed for comparison in some rules.
        /// </summary>
        public int Exponent { get; set; }

        /// <summary>
        /// Constructor for a <see cref="RomanNumeralToken"/>
        /// </summary>
        /// <param name="romanNumeral">example: "I", "V", "X"</param>
        /// <param name="value">the corresponding value example: 1, 5, 10</param>
        public RomanNumeralToken(string romanNumeral, int value)
        {
            RomanNumeral = romanNumeral;
            NumeralValue = value;
            CalculateBaseAndExponent();
        }

        /// <summary>
        /// <para>Calculates the <see cref="BaseValue"/> and <see cref="Exponent"/> for the current token.</para>
        /// <para>It uses <see cref="Math.Log10(double)"/> and <see cref="Math.Pow(double, double)"/> for calculation.</para>
        /// </summary>
        private void CalculateBaseAndExponent()
        {
            Exponent = (int)Math.Floor(Math.Log10(NumeralValue));
            BaseValue = (int)(NumeralValue / Math.Pow(10, Exponent));
        }
    }
}
