namespace ArithmeticaRomana.Core.Formatter
{
    /// <summary>
    /// Formats an integer value into a Roman numeral string.
    /// </summary>
    public interface IRomanNumeralFormatter
    {
        /// <summary>
        /// Function to format an integer value into a Roman numeral string.
        /// </summary>
        /// <param name="value">value between 1 - int.MaxValue</param>
        /// <returns>The roman numeral representation</returns>
        string Format(int value);     
    }
}
