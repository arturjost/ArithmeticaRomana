namespace ArithmeticaRomana.Core.Parser
{
    public class RomanParserResult : IRomanParserResult
    {
        public bool IsSuccess { get; private set; }
        public RomanNumeral? RomanNumeral { get; set; }
        public RomanParserError ErrorType { get; private set; }
        public string? ErrorMessage { get; private set; }


        public RomanParserResult(RomanNumeral romanNumeral)
        {
            SetSuccess(romanNumeral);
        }

        public RomanParserResult(RomanParserError errorType, string errorMessage)
        {
            SetError(errorType, errorMessage);
        }

        public void SetSuccess(RomanNumeral romanNumeral)
        {
            IsSuccess = true;
            RomanNumeral = romanNumeral;
            ErrorType = RomanParserError.None;
        }

        public void SetError(RomanParserError errorType, string errorMessage)
        {
            IsSuccess = false;
            RomanNumeral = null;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
        }
    }
}
