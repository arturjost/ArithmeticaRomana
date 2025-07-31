using ArithmeticaRomana.Core.Internal;

namespace ArithmeticaRomana.Core.Unit.Tests.Internals
{
    public class RomanNumeralTokenTests
    {
        [Theory]
        [InlineData("I", 1, 1, 0)]
        [InlineData("X", 10, 1, 1)]
        [InlineData("C", 100, 1, 2)]
        [InlineData("M", 1000, 1, 3)]
        [InlineData("V", 5, 5, 0)]
        [InlineData("L", 50, 5, 1)]
        [InlineData("D", 500, 5, 2)]
        [InlineData("A", 500_000, 5, 5)]
        [InlineData("XYZ", 1_000_000, 1, 6)]
        [InlineData("M\u0305M\u0305", 2_000_000, 2, 6)]
        public void Constructor_WithValidInput_ShouldSetPropertiesCorrectly(string numeral, int value, int expectedBaseValue, int expectedExponent)
        {
            // Arrange & Act
            var token = new RomanNumeralToken(numeral, value);
            // Assert
            Assert.Equal(expectedBaseValue, token.BaseValue);
            Assert.Equal(expectedExponent, token.Exponent);
        }
    }
}
