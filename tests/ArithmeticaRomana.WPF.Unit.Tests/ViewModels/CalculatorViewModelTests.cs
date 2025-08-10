using ArithmeticaRomana.Core;
using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.Core.Parser;
using ArithmeticaRomana.WPF.ViewModels;
using Moq;

namespace ArithmeticaRomana.WPF.Unit.Tests.ViewModels
{
    public class CalculatorViewModelTests
    {
        private readonly Mock<IRomanNumeralFormatter> _formatterMock;
        private readonly Mock<IRomanNumeralParser> _parserMock;
        private readonly CalculatorViewModel _viewModel;

        public CalculatorViewModelTests()
        {
            _formatterMock = new Mock<IRomanNumeralFormatter>();
            _parserMock = new Mock<IRomanNumeralParser>();

            // Set up the formatter mock to return a simple roman numeral string for a given integer
            _formatterMock.Setup(f => f.Format(It.IsAny<int>())).Returns((int i) =>
            {
                // Simple logic for the test to work, e.g., 1 -> "I", 5 -> "V", etc.
                return i switch
                {
                    1 => "I",
                    2 => "II",
                    5 => "V",
                    10 => "X",
                    _ => i.ToString()
                };
            });
            _viewModel = new CalculatorViewModel(_formatterMock.Object, _parserMock.Object);
        }

        [Fact]
        public void ViewModel_ShouldBeInitializedWithArabicMode()
        {
            // Assert
            Assert.False(_viewModel.RomanMode);
            Assert.True(_viewModel.ArabicMode);
        }

        [Fact]
        public void ViewModel_ShouldInitializeDisplayViewModel()
        {
            // Assert
            Assert.NotNull(_viewModel.Display);
        }

        [Fact]
        public void RomanMode_SwitchToRoman_ShouldChangeModeCorrectly()
        {
            // Arrange
            _viewModel.RomanMode = false;

            // Act
            _viewModel.RomanMode = true;

            // Assert
            Assert.True(_viewModel.RomanMode);
            Assert.False(_viewModel.ArabicMode);
        }

        [Fact]
        public void ArabicMode_SwitchToArabic_ShouldChangeModeCorrectly()
        {
            // Arrange
            _viewModel.RomanMode = true;

            // Act
            _viewModel.ArabicMode = true;

            // Assert
            Assert.True(_viewModel.ArabicMode);
            Assert.False(_viewModel.RomanMode);
        }

        [Fact]
        public void Addition_ShouldCalculateCorrectly()
        {
            // Arrange
            _viewModel.ReceiveInputCommand.Execute("10");
            _viewModel.ReceiveInputCommand.Execute("Addition");
            _viewModel.ReceiveInputCommand.Execute("5");

            // Act
            _viewModel.ReceiveInputCommand.Execute("Calculate");

            // Assert
            Assert.Equal(15, GetPrivateFieldValue<int>("_displayValue"));
        }

        [Fact]
        public void Subtraction_ShouldCalculateCorrectly()
        {
            // Arrange
            _viewModel.ReceiveInputCommand.Execute("10");
            _viewModel.ReceiveInputCommand.Execute("Subtraction");
            _viewModel.ReceiveInputCommand.Execute("5");

            // Act
            _viewModel.ReceiveInputCommand.Execute("Calculate");

            // Assert
            Assert.Equal(5, GetPrivateFieldValue<int>("_displayValue"));
        }

        [Fact]
        public void Evaluate_WithoutOperation_ShouldDoNothing()
        {
            // Arrange
            _viewModel.ReceiveInputCommand.Execute("10");

            // Act
            _viewModel.ReceiveInputCommand.Execute("Calculate");

            // Assert
            Assert.Equal(10, GetPrivateFieldValue<int>("_displayValue"));
        }

        [Fact]
        public void ProcessInput_ShouldUpdateDisplayValueInArabicMode()
        {
            // Arrange
            _viewModel.ArabicMode = true;

            // Act
            _viewModel.ReceiveInputCommand.Execute("1");
            _viewModel.ReceiveInputCommand.Execute("2");

            // Assert
            Assert.Equal(12, GetPrivateFieldValue<int>("_displayValue"));
        }

        [Fact]
        public void ProcessInput_ShouldUpdateDisplayValueInRomanMode()
        {
            // Arrange
            _viewModel.RomanMode = true;
            _parserMock.Setup(p => p.Parse("I")).Returns(new RomanParserResult(new RomanNumeral(1)));

            // Act
            _viewModel.ReceiveInputCommand.Execute("I");

            // Assert
            Assert.Equal(1, GetPrivateFieldValue<int>("_displayValue"));
        }

        [Fact]
        public void Clear_ShouldResetAllValues()
        {
            // Arrange
            _viewModel.ReceiveInputCommand.Execute("10");
            _viewModel.ReceiveInputCommand.Execute("Addition");
            _viewModel.ReceiveInputCommand.Execute("5");
            _viewModel.ReceiveInputCommand.Execute("Calculate");

            // Act
            _viewModel.ReceiveInputCommand.Execute("Clear");

            // Assert
            Assert.Null(GetPrivateFieldValue<int?>("_lastValue"));
            Assert.Null(GetPrivateFieldValue<int?>("_operationValue"));
            Assert.Equal(0, GetPrivateFieldValue<int>("_displayValue"));
            Assert.Equal(string.Empty, GetPrivateFieldValue<string>("_currentOperation"));
            var lastInputsStack = GetPrivateFieldValue<Stack<string>>("lastInputs");
            Assert.Empty(lastInputsStack);
        }

        [Fact]
        public void Delete_ShouldRemoveLastInput()
        {
            // Arrange
            _viewModel.ReceiveInputCommand.Execute("1");
            _viewModel.ReceiveInputCommand.Execute("2");

            // Act
            _viewModel.ReceiveInputCommand.Execute("Delete");

            // Assert
            Assert.Equal(1, GetPrivateFieldValue<int>("_displayValue"));
        }

        private T GetPrivateFieldValue<T>(string fieldName)
        {
            var fieldInfo = typeof(CalculatorViewModel).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)fieldInfo!.GetValue(_viewModel)!;
        }
    }
}
