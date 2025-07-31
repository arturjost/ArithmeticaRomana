namespace ArithmeticaRomana.Core.Parser
{
    /// <summary>
    /// Enum for errors <see cref="IRomanParserResult"/>
    /// </summary>
    public enum RomanParserError
    {
        None,
        InvalidRepetition,
        InvalidSubtraction,
        OutOfRange,
        MalformedInput
    }

    /// <summary>
    /// Interface for the result of parsing a Roman numeral.
    /// </summary>
    public interface IRomanParserResult
    {
        /// <summary>
        /// Property indicating whether the parsing was successful.
        /// </summary>
        bool IsSuccess { get; }
        /// <summary>
        /// The parsed Roman numeral if the parsing was successful.
        /// </summary>
        RomanNumeral? RomanNumeral { get; set; }
        /// <summary>
        /// Error type if the parsing failed.
        /// </summary>
        RomanParserError ErrorType { get; }
        /// <summary>
        /// Error message if the parsing failed.
        /// </summary>
        string? ErrorMessage { get; }

        /// <summary>
        /// Sets the result of a successful parsing operation.
        /// </summary>
        /// <param name="romanNumeral">Parsed RomanNumeral</param>
        void SetSuccess(RomanNumeral romanNumeral);
        /// <summary>
        /// Sets the result of a failed parsing operation.
        /// </summary>
        /// <param name="errorType"><see cref="RomanParserError"/> enum</param>
        /// <param name="errorMessage">Detailed error message</param>
        void SetError(RomanParserError errorType, string errorMessage);
    }
}
