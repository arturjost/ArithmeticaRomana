namespace ArithmeticaRomana.Core.Internal
{
    /// <summary>
    /// Interface that provide us with the sorted tokens for given encoding
    /// </summary>
    internal interface IRomanEncoding
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
