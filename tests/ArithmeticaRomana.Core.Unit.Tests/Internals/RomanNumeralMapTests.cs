using ArithmeticaRomana.Core.Internal;

namespace ArithmeticaRomana.Core.Unit.Tests.Internals
{
    public class RomanNumeralMapTests
    {
        [Fact]
        public void Constructor_WithValidInput_ShouldCreateCorrectMap()
        {
            // Arrange 
            string[] tokens = ["I", "V", "X", "L", "C", "D", "M"];
            int expectedTokenCount = tokens.Length;

            // Act
            RomanNumeralMap map = new RomanNumeralMap(tokens);
            List<RomanNumeralToken> baseTokens = map.BaseTokensByValue().ToList();
            List<RomanNumeralToken> allTokens = map.TokensByValue().ToList();

            // Assert
            Assert.NotNull(map);
            Assert.Equal(expectedTokenCount, baseTokens.Count); // we provided all base tokens, so it should be exactly that size
            Assert.Equal((expectedTokenCount * 2) - 1, allTokens.Count); // allTokens.Count should be exactly (expectedCount * 2) - 1
            Assert.Equal(tokens[0], allTokens[allTokens.Count - 1].RomanNumeral); // first tokens needs to be the last in TokensByValue
            Assert.Equal(tokens[tokens.Length - 1], allTokens[0].RomanNumeral); // last tokens needs to be the first token in TokensByValue
        }

        [Fact]
        public void Constructor_WithModifier_ShouldCreateCorrectMap()
        {
            // Arrange 
            string[] tokens = ["I", "V", "X", "L", "C", "D", "M"];
            string modifier = "^";
            int expectedTokenCount = tokens.Length;

            // Act
            RomanNumeralMap map = new RomanNumeralMap(tokens, modifier);
            List<RomanNumeralToken> baseTokens = map.BaseTokensByValue().ToList();
            List<RomanNumeralToken> allTokens = map.TokensByValue().ToList();

            // Assert
            Assert.NotNull(map);
        }

    }
}
