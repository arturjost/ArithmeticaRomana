namespace ArithmeticaRomana.Core.Internal
{
    /// <summary>
    /// Our interface for a Roman numeral map, this will help us with parsing and formatting
    /// It will provide us with sorted tokens for different flavours like Vinculum or Apostrophus 
    /// </summary>
    internal interface IRomanNumeralMap
    {
        /// <summary>
        /// This function returns only base values ordered by value.
        /// </summary>
        /// <returns>example: "M", "D", "C", "L", "X", "V", "I"</returns>
        IEnumerable<RomanNumeralToken> BaseTokensByValue();

        /// <summary>
        /// This function returns all possible values ordered by value.
        /// </summary>
        /// <returns>example: "M", "CM", "D", "CD", "C" etc.</returns>
        IEnumerable<RomanNumeralToken> TokensByValue();
    }
}
