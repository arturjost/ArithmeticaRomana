using ArithmeticaRomana.Core.Parser;

namespace ArithmeticaRomana.Core.Unit.Tests.Parser
{
    public class VinculumRomanNumeralParserTests
    {
        [Theory]
        [InlineData("I", 1)] // Smallest possible value
        [InlineData("III", 3)] // Repetition of the smallest value
        [InlineData("IV", 4)] // Basic subtraction rule (e.g., 5 - 1)
        [InlineData("IX", 9)] // Another basic subtraction rule (e.g., 10 - 1)
        [InlineData("XLIX", 49)] // Combination of subtraction and addition (40 + 9)
        [InlineData("LXXXVIII", 88)] // Repetitions and additions (50 + 30 + 8)
        [InlineData("CDXCIV", 494)] // Multiple subtractions and combinations (400 + 90 + 4)
        [InlineData("CMXCIX", 999)] // Largest subtraction combination below 1000 (900 + 90 + 9)
        [InlineData("MMXIV", 2014)] // A common modern year (2000 + 10 + 4)
        [InlineData("MMMCMXCIX", 3999)] // The largest representable value in standard Roman numerals
        [InlineData("XIX", 19)]
        public void Parse_ValidRomanNumeral_ReturnsTrueAndCorrectValue(string input, int expectedValue)
        {
            // Arrange
            var parser = new VinculumRomanNumeralParser();
            // Act
            var result = parser.Parse(input);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.RomanNumeral);
            Assert.Equal(expectedValue, result.RomanNumeral.Value.AsInteger);
        }

        [Theory]
        [InlineData("IIII", RomanParserError.InvalidRepetition)] // Invalid: Four identical consecutive numerals are not allowed (max 3, except certain subtractions like IV)
        [InlineData("VV", RomanParserError.InvalidRepetition)] // Invalid: V, L, and D cannot be repeated
        [InlineData("IL", RomanParserError.InvalidSubtraction)] // Invalid: I can only be subtracted from V and X, not from L
        [InlineData("IC", RomanParserError.InvalidSubtraction)] // Invalid: I can only be subtracted from V and X, not from C
        [InlineData("VX", RomanParserError.InvalidSubtraction)] // Invalid: V cannot be used for subtraction
        [InlineData("XLX", RomanParserError.InvalidSequence)] // Invalid: A smaller numeral cannot be placed between two larger identical numerals (e.g., 40 is XL, not XLX)
        [InlineData("MCMXCIVX", RomanParserError.InvalidSequence)] // Invalid: Multiple subtractions in sequence or incorrect order (e.g., IVX is invalid)
        [InlineData("MDCLXVIIV", RomanParserError.InvalidSequence)] // Invalid: Combination of IV and V is incorrect; a numeral cannot be simultaneously added and subtracted in such a way
        [InlineData("MMMM", RomanParserError.InvalidRepetition)] // Invalid: Four M's are not allowed in standard notation (maximum is 3999, which is MMMCMXCIX)
        [InlineData(" ", RomanParserError.MalformedInput)] // Invalid: Empty string
        [InlineData("K", RomanParserError.MalformedInput)] // Invalid: Non-Roman numeral character
        [InlineData("MCMXCVXI", RomanParserError.InvalidSubtraction)] // Invalid: Incorrect sequence or invalid subtraction/addition (e.g., XI is 11, but VXI is invalid in this context)
        [InlineData("VIV", RomanParserError.InvalidSequence)]
        public void Parse_InvalidRomanNumeral_ReturnsCorrectError(string input, RomanParserError error)
        {
            // Arrange
            var parser = new VinculumRomanNumeralParser();
            // Act
            var result = parser.Parse(input);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.ErrorType);
        }
    }
}
