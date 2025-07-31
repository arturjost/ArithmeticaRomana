using ArithmeticaRomana.Core.Formatter;

namespace ArithmeticaRomana.Core.Unit.Tests.Formatter
{
    public class VinculumRomanNumeralFormatterTests
    {
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
        public void Format_ValidInteger_ReturnsCorrectError(int value, string expectedRoman)
        {
            // Arrange
            var formatter = new VinculumRomanNumeralFormatter();

            // Act
            string romanRepresentation = formatter.Format(value);

            // Assert
            Assert.Equal(expectedRoman, romanRepresentation);
        }
    }
}
