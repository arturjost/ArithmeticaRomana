using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.Core.Parser;
using Moq;
using System.Runtime.InteropServices;

namespace ArithmeticaRomana.Core.Unit.Tests
{
    public class RomanNumeralTests
    {
        // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
        // structs are small and commonly short-lived or are commonly embedded in other objects.
        // It has an instance size under 16 bytes.
        [Fact]
        public void RomanNumeral_InstanceSize_ShouldBeUnder16Bytes()
        {
            // Arrange
            var size = Marshal.SizeOf<RomanNumeral>();
            // Act & Assert
            Assert.True(size <= 16, $"The size of RomanNumeral is {size} bytes, which exceeds the recommended maximum of 16 bytes.");
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(3, 1, 4)]
        [InlineData(5, 5, 10)]
        [InlineData(9, 1, 10)]
        [InlineData(50, 50, 100)]
        [InlineData(40, 10, 50)]
        [InlineData(50, 4, 54)]
        [InlineData(1000, 500, 1500)]
        [InlineData(2000, 1999, 3999)]
        [InlineData(3998, 1, 3999)]
        public void AdditionOperator_InRange_ShouldReturnCorrectValue(int first, int second, int expectedValue)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1 + numeral2;
            // Assert
            Assert.Equal(expectedValue, result.AsInteger);
        }

        [Theory]
        [InlineData(5, 1, 4)]
        [InlineData(10, 5, 5)]
        [InlineData(50, 10, 40)]
        [InlineData(100, 10, 90)]
        [InlineData(1000, 100, 900)]
        [InlineData(3999, 1, 3998)]
        [InlineData(3999, 3998, 1)]
        [InlineData(54, 4, 50)]
        [InlineData(100, 99, 1)]
        [InlineData(1999, 99, 1900)]
        public void SubtractionOperator_InRange_ShouldReturnCorrectValue(int first, int second, int expectedValue)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1 - numeral2;
            // Assert
            Assert.Equal(expectedValue, result.AsInteger);
        }

        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(5, 2, 10)]
        [InlineData(10, 3, 30)]
        [InlineData(100, 5, 500)]
        [InlineData(500, 2, 1000)]
        [InlineData(1234, 1, 1234)]
        [InlineData(1000, 3, 3000)]
        [InlineData(1999, 2, 3998)]
        [InlineData(15, 2, 30)]
        [InlineData(25, 4, 100)]
        public void MultiplicationOperator_InRange_ShouldReturnCorrectValue(int first, int second, int expectedValue)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1 * numeral2;
            // Assert
            Assert.Equal(expectedValue, result.AsInteger);
        }

        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(10, 5, 2)]
        [InlineData(30, 3, 10)]
        [InlineData(500, 5, 100)]
        [InlineData(1000, 2, 500)]
        [InlineData(1234, 1, 1234)]
        [InlineData(3999, 3999, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(100, 4, 25)]
        [InlineData(3000, 3, 1000)]
        public void DivisionOperator_InRange_ShouldReturnCorrectValue(int first, int second, int expectedValue)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1 / numeral2;
            // Assert
            Assert.Equal(expectedValue, result.AsInteger);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(4, 5)]
        [InlineData(9, 10)]
        [InlineData(49, 50)]
        [InlineData(99, 100)]
        [InlineData(499, 500)]
        [InlineData(999, 1000)]
        [InlineData(1234, 1235)]
        [InlineData(3998, 3999)]
        [InlineData(3, 4)]
        public void IncrementOperator_InRange_ShouldReturnCorrectValue(int initialValue, int expectedValue)
        {
            // Arrange
            var numeral = new RomanNumeral(initialValue);
            // Act
            numeral++;
            // Assert
            Assert.Equal(expectedValue, numeral.AsInteger);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(5, 4)]
        [InlineData(10, 9)]
        [InlineData(50, 49)]
        [InlineData(100, 99)]
        [InlineData(500, 499)]
        [InlineData(1000, 999)]
        [InlineData(1235, 1234)]
        [InlineData(3999, 3998)]
        [InlineData(123, 122)]
        public void DecrementOperator_InRange_ShouldReturnCorrectValue(int initialValue, int expectedValue)
        {
            // Arrange
            var numeral = new RomanNumeral(initialValue);
            // Act
            numeral--;
            // Assert
            Assert.Equal(expectedValue, numeral.AsInteger);
        }

        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(4, 5, -1)]
        [InlineData(99, 100, -1)]
        [InlineData(1, 3999, -1)]
        [InlineData(1, 1, 0)]
        [InlineData(42, 42, 0)]
        [InlineData(3999, 3999, 0)]
        [InlineData(2, 1, 1)]
        [InlineData(5, 4, 1)]
        [InlineData(100, 99, 1)]
        [InlineData(3999, 1, 1)]
        public void CompareTo_InRange_ShouldReturnCorrectValue(int first, int second, int expectedSign)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1.CompareTo(numeral2);
            // Assert
            Assert.Equal(expectedSign, result);
        }


        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(4, 4, true)]
        [InlineData(100, 100, true)]
        [InlineData(3999, 3999, true)]
        [InlineData(1, 2, false)]
        [InlineData(5, 4, false)]
        [InlineData(10, 11, false)]
        [InlineData(3998, 3999, false)]
        [InlineData(7, 7, true)]
        [InlineData(1234, 1423, false)]
        public void EqualsOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);

            // Act
            var result = (numeral1 == numeral2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, !true)]
        [InlineData(4, 4, !true)]
        [InlineData(100, 100, !true)]
        [InlineData(3999, 3999, !true)]
        [InlineData(1, 2, !false)]
        [InlineData(5, 4, !false)]
        [InlineData(10, 11, !false)]
        [InlineData(3998, 3999, !false)]
        [InlineData(7, 7, !true)]
        [InlineData(1234, 1423, !false)]
        public void NotEqualsOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);

            // Act
            var result = (numeral1 != numeral2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(4, 5, true)]
        [InlineData(99, 100, true)]
        [InlineData(1, 3999, true)]
        [InlineData(49, 50, true)]
        [InlineData(999, 1000, true)]
        [InlineData(2, 1, false)]
        [InlineData(5, 4, false)]
        [InlineData(10, 10, false)]
        [InlineData(3999, 1, false)]
        public void LessThanOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = (numeral1 < numeral2);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, true)]
        [InlineData(4, 4, true)]
        [InlineData(4, 5, true)]
        [InlineData(5, 4, false)]
        [InlineData(99, 100, true)]
        [InlineData(3999, 3999, true)]
        [InlineData(2, 1, false)]
        [InlineData(123, 2000, true)]
        [InlineData(100, 99, false)]
        public void LessThanOrEqualOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = (numeral1 <= numeral2);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(5, 4, true)]
        [InlineData(100, 99, true)]
        [InlineData(3999, 1, true)]
        [InlineData(1, 1, false)]
        [InlineData(1, 2, false)]
        [InlineData(4, 5, false)]
        [InlineData(99, 100, false)]
        [InlineData(20, 99, false)]
        [InlineData(2099, 3099, false)]
        public void GreaterThanOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = (numeral1 > numeral2);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(2, 1, true)]
        [InlineData(5, 4, true)]
        [InlineData(100, 99, true)]
        [InlineData(3999, 3999, true)]
        [InlineData(1, 2, false)]
        [InlineData(4, 5, false)]
        [InlineData(99, 100, false)]
        [InlineData(1, 3999, false)]
        [InlineData(1212, 2121, false)]
        public void GreaterThanOrEqualOperator_InRange_ShouldReturnCorrectBoolean(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = (numeral1 >= numeral2);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(42)]
        [InlineData(99)]
        [InlineData(100)]
        [InlineData(2121)]
        [InlineData(3999)]
        public void GetHashCode_ShouldReturnConsistentValue(int value)
        {
            // Arrange
            var numeral1 = new RomanNumeral(value);
            var numeral2 = new RomanNumeral(value);
            // Act
            var hashCode1 = numeral1.GetHashCode();
            var hashCode2 = numeral2.GetHashCode();
            // Assert
            Assert.Equal(hashCode1, hashCode2);
            Assert.Equal(hashCode1, new RomanNumeral(value).GetHashCode());
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(5, 5, true)]
        [InlineData(42, 42, true)]
        [InlineData(3999, 3999, true)]
        [InlineData(1, 2, false)]
        [InlineData(5, 4, false)]
        [InlineData(100, "C", false)]
        [InlineData(1, 123.123, false)]
        [InlineData(5, long.MaxValue, false)]
        [InlineData(1000, float.MinValue, false)]
        public void Equals_Object_ShouldReturnCorrectValue(int value, object obj, bool expected)
        {
            // Arrange
            var numeral = new RomanNumeral(value);
            // Act
            var result = numeral.Equals(obj);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(5, 5, true)]
        [InlineData(42, 42, true)]
        [InlineData(3999, 3999, true)]
        [InlineData(100, 100, true)]
        [InlineData(1, 2, false)]
        [InlineData(5, 4, false)]
        [InlineData(1, 2898, false)]
        [InlineData(5, 2323, false)]
        [InlineData(1000, 5, false)]
        public void Equals_RomanNumeral_ShouldReturnCorrectValue(int first, int second, bool expected)
        {
            // Arrange
            var numeral1 = new RomanNumeral(first);
            var numeral2 = new RomanNumeral(second);
            // Act
            var result = numeral1.Equals(numeral2);
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryParse_Mock_ShouldReturnTrueForValidInput()
        {
            // Arrange
            Mock<IRomanNumeralParser> mockParser = new();
            mockParser.Setup(p => p.Parse(It.IsAny<string>()))
                .Returns(new RomanParserResult(new RomanNumeral(12)));

            // Act
            var success = RomanNumeral.TryParse("XII", out RomanNumeral result, mockParser.Object);

            // Assert
            Assert.True(success);
            Assert.Equal(12, result.AsInteger);
        }

        [Fact]
        public void TryParse_Mock_ShouldReturnFalseForInvalidInput()
        {
            // Arrange
            Mock<IRomanNumeralParser> mockParser = new();
            mockParser.Setup(p => p.Parse(It.IsAny<string>()))
                .Returns(new RomanParserResult(RomanParserError.MalformedInput, ""));

            // Act
            var success = RomanNumeral.TryParse("NNNN", out RomanNumeral result, mockParser.Object);

            // Assert
            Assert.False(success);
            Assert.Equal(default, result.AsInteger);
        }

        [Fact]
        public void AsRomanRepresentation_Mock_ShouldReturnFormattedString()
        {
            // Arrange
            Mock<IRomanNumeralFormatter> mockFormatter = new();
            mockFormatter.Setup(f => f.Format(It.IsAny<int>()))
                .Returns("XII");
            var numeral = new RomanNumeral(12);
            // Act
            var result = numeral.AsRomanRepresentation(mockFormatter.Object);
            // Assert
            Assert.Equal("XII", result);
        }
    }
}
