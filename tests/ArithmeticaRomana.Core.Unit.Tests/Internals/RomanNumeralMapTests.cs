using ArithmeticaRomana.Core.Internal;

namespace ArithmeticaRomana.Core.Unit.Tests.Internals
{
    public class RomanNumeralMapTests
    {
        [Theory]
        [InlineData("I", "V", "X", "L", "C", "D", "M")]
        [InlineData("I", "V", "X")]
        [InlineData("I", "V", "X", "L", "C", "D", "M", "A", "B", "C")]
        [InlineData("I", "V", "X", "L", "C", "D", "ↀ", "ↁ", "ↂ", "ↇ", "ↈ", "IↃↃↃↃ", "CCCCIↃↃↃↃ")]
        public void Constructor_WithValidInput_ShouldCreateCorrectMap(params string[] tokens)
        {
            // Arrange 
            string[] tokenValues = tokens;
            int expectedCount = tokens.Length;

            // Act
            RomanNumeralMap map = new RomanNumeralMap(tokens);
            List<RomanNumeralToken> baseTokens = map.BaseTokensByValue().ToList();
            List<RomanNumeralToken> allTokens = map.TokensByValue().ToList();

            // Assert
            Assert.Equal(expectedCount, baseTokens.Count); // we provided all base tokens, so it should be exactly that size
            Assert.Equal((expectedCount * 2) - 1, allTokens.Count); // allTokens.Count should be exactly (expectedCount * 2) - 1
            Assert.Equal(tokens[0], allTokens[allTokens.Count - 1].RomanNumeral); // first tokens needs to be the last in TokensByValue
            Assert.Equal(tokens[tokens.Length - 1], allTokens[0].RomanNumeral); // last tokens needs to be the first token in TokensByValue
        }
    }
}
