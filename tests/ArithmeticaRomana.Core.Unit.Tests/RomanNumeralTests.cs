namespace ArithmeticaRomana.Core.Unit.Tests
{
    public class RomanNumeralTests
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
        public void TryParse_ValidRomanNumeral_ReturnsTrueAndCorrectValue(string input, int expectedValue)
        {
            // Arrange

            // Act
            bool success = RomanNumeral.TryParse(input, out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(expectedValue, result.AsInteger);
        }

        [Theory]
        [InlineData("IIII")] // Invalid: Four identical consecutive numerals are not allowed (max 3, except certain subtractions like IV)
        [InlineData("VV")] // Invalid: V, L, and D cannot be repeated
        [InlineData("IL")] // Invalid: I can only be subtracted from V and X, not from L
        [InlineData("IC")] // Invalid: I can only be subtracted from V and X, not from C
        [InlineData("VX")] // Invalid: V cannot be used for subtraction
        [InlineData("XLX")] // Invalid: A smaller numeral cannot be placed between two larger identical numerals (e.g., 40 is XL, not XLX)
        [InlineData("MCMXCIVX")] // Invalid: Multiple subtractions in sequence or incorrect order (e.g., IVX is invalid)
        [InlineData("MDCLXVIIV")] // Invalid: Combination of IV and V is incorrect; a numeral cannot be simultaneously added and subtracted in such a way
        [InlineData("MMMM")] // Invalid: Four M's are not allowed in standard notation (maximum is 3999, which is MMMCMXCIX)
        [InlineData(" ")] // Invalid: Empty string
        [InlineData("K")] // Invalid: Non-Roman numeral character
        [InlineData("MCMXCVXI")] // Invalid: Incorrect sequence or invalid subtraction/addition (e.g., XI is 11, but VXI is invalid in this context)
        public void TryParse_InvalidRomanNumeral_ReturnsFalse(string input)
        {
            // Arrange

            // Act
            bool success = RomanNumeral.TryParse(input, out var result);

            // Assert
            Assert.False(success);
            Assert.Equal(default(RomanNumeral), result);
        }

        [Theory]
        [InlineData(1, "I")] // Smallest positive integer
        [InlineData(3, "III")] // Repetition of the 'I' numeral
        [InlineData(4, "IV")] // Basic subtraction rule (e.g., 5 - 1)
        [InlineData(9, "IX")] // Another basic subtraction rule (e.g., 10 - 1)
        [InlineData(49, "XLIX")] // Combination of subtraction and addition (40 + 9)
        [InlineData(88, "LXXXVIII")] // Repetitions and additions (50 + 30 + 8)
        [InlineData(494, "CDXCIV")] // Multiple subtractions and combinations (400 + 90 + 4)
        [InlineData(999, "CMXCIX")] // Largest subtraction combination below 1000
        [InlineData(2014, "MMXIV")] // A common modern year example
        [InlineData(3999, "MMMCMXCIX")] // The largest value representable in standard Roman numerals
        public void ToRoman_ValidInteger_ReturnsCorrectRomanNumeral(int value, string expectedRoman)
        {
            // Arrange

            // Act
            RomanNumeral romanNumeral = new(value);
            string romanRepresentation = romanNumeral.AsRoman();

            // Assert
            Assert.Equal(expectedRoman, romanRepresentation);
        }
    }
}
