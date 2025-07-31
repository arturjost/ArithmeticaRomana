namespace ArithmeticaRomana.Core.Parser
{
    /// <summary>
    /// The interface of a Roman numeral parser.
    /// </summary>
    public interface IRomanNumeralParser
    {
        /// <summary>
        /// Parses a string into its corresponding RomanParseResult.
        /// </summary>
        /// <param name="romanRepresentation">should be a roman numeral representation</param>
        /// <returns>A detailed result</returns>
        IRomanParserResult Parse(string romanRepresentation);
    }
}
