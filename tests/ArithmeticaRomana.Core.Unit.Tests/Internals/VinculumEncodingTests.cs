using ArithmeticaRomana.Core.Internal;

namespace ArithmeticaRomana.Core.Unit.Tests.Internals
{
    public class VinculumEncodingTests
    {
        private readonly VinculumEncoding _encoding;
        public VinculumEncodingTests()
        {
            // Arrange: Instantiate the class once for all tests.
            _encoding = new VinculumEncoding();
        }

        [Fact]
        public void VinculumEncoding_ImmutableList_ShouldHaveCorrectCount()
        {
            // Act
            var baseTokens = _encoding.BaseTokensByValue().ToList();
            var tokens = _encoding.TokensByValue().ToList();

            // Assert
            Assert.Equal(21, baseTokens.Count);
            Assert.Equal(39, tokens.Count);
        }

        [Theory]
        [InlineData("I\u0305", 1000)]
        [InlineData("M", 1000)]
        [InlineData("M\u0305\u0305", 1000000000)]
        [InlineData("C\u0305M\u0305", 900000)]
        [InlineData("I\u0305V\u0305", 4000)]
        [InlineData("IV", 4)]
        public void VinculumEncoding_ImmutableList_ShouldContainNumeral(string numeral, int value)
        {
            // Act
            var tokens = _encoding.TokensByValue().ToList();

            // Assert
            Assert.Contains(tokens, t => t.RomanNumeral == numeral && t.NumeralValue == value);
        }

    }
}
