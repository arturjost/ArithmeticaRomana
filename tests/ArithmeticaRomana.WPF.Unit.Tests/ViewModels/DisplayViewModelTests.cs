using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.WPF.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticaRomana.WPF.Unit.Tests.ViewModels
{
    public class DisplayViewModelTests
    {
        private readonly Mock<IRomanNumeralFormatter> _formatterMock;
        private readonly DisplayViewModel _viewModel;

        public DisplayViewModelTests()
        {
            _formatterMock = new Mock<IRomanNumeralFormatter>();
            _viewModel = new DisplayViewModel(_formatterMock.Object);
        }

        [Fact]
        public void RenderLeftNumber_WithPositiveValue_ShouldSetCorrectProperties()
        {
            // Arrange
            int value = 123;
            _formatterMock.Setup(f => f.Format(value)).Returns("CXXIII");

            // Act
            _viewModel.RenderLeftNumber(value);

            // Assert
            Assert.Equal("CXXIII", _viewModel.LeftRoman);
            Assert.Equal("123", _viewModel.LeftArabic);
        }

        [Fact]
        public void RenderLeftNumber_WithZeroValue_ShouldSetPropertiesToEmptyString()
        {
            // Arrange
            int value = 0;

            // Act
            _viewModel.RenderLeftNumber(value);

            // Assert
            Assert.Equal(string.Empty, _viewModel.LeftRoman);
            Assert.Equal(string.Empty, _viewModel.LeftArabic);
        }

        [Fact]
        public void RenderRightNumber_WithPositiveValue_ShouldSetCorrectProperties()
        {
            // Arrange
            int value = 456;
            _formatterMock.Setup(f => f.Format(value)).Returns("CDLVI");

            // Act
            _viewModel.RenderRightNumber(value);

            // Assert
            Assert.Equal("CDLVI", _viewModel.RightRoman);
            Assert.Equal("456", _viewModel.RightArabic);
        }

        [Fact]
        public void RenderRightNumber_WithZeroValue_ShouldSetPropertiesToEmptyString()
        {
            // Arrange
            int value = 0;

            // Act
            _viewModel.RenderRightNumber(value);

            // Assert
            Assert.Equal(string.Empty, _viewModel.RightRoman);
            Assert.Equal(string.Empty, _viewModel.RightArabic);
        }

        [Fact]
        public void RenderMainDisplay_InArabicMode_ShouldSetCorrectProperties()
        {
            // Arrange
            int value = 789;
            string operation = "+";
            _formatterMock.Setup(f => f.Format(value)).Returns("DCCLXXXIX");

            // Act
            _viewModel.RenderMainDisplay(true, value, operation);

            // Assert
            Assert.Equal("789", _viewModel.MainDisplayText);
            Assert.Equal("DCCLXXXIX", _viewModel.BottomDisplayText);
            Assert.Equal("+", _viewModel.Operation);
        }

        [Fact]
        public void RenderMainDisplay_InRomanMode_ShouldSetCorrectProperties()
        {
            // Arrange
            int value = 789;
            string operation = "-";
            _formatterMock.Setup(f => f.Format(value)).Returns("DCCLXXXIX");

            // Act
            _viewModel.RenderMainDisplay(false, value, operation);

            // Assert
            Assert.Equal("DCCLXXXIX", _viewModel.MainDisplayText);
            Assert.Equal("789", _viewModel.BottomDisplayText);
            Assert.Equal("-", _viewModel.Operation);
        }

        [Fact]
        public void RenderError_ShouldSetErrorText()
        {
            // Arrange
            string errorMessage = "Test error message.";

            // Act
            _viewModel.RenderError(errorMessage);

            // Assert
            Assert.Equal(errorMessage, _viewModel.ErrorDisplayText);
        }

        [Fact]
        public void ClearError_ShouldSetErrorTextToEmptyString()
        {
            // Arrange
            _viewModel.RenderError("Some error");

            // Act
            _viewModel.ClearError();

            // Assert
            Assert.Equal(string.Empty, _viewModel.ErrorDisplayText);
        }

        [Fact]
        public void ClearDisplay_ShouldResetAllProperties()
        {
            // Arrange
            _viewModel.RenderLeftNumber(100);
            _viewModel.RenderRightNumber(200);
            _viewModel.RenderMainDisplay(true, 300, "+");
            _viewModel.RenderError("An error occurred");

            // Act
            _viewModel.ClearDisplay();

            // Assert
            Assert.Equal(string.Empty, _viewModel.LeftArabic);
            Assert.Equal(string.Empty, _viewModel.LeftRoman);
            Assert.Equal(string.Empty, _viewModel.RightArabic);
            Assert.Equal(string.Empty, _viewModel.RightRoman);
            Assert.Equal(string.Empty, _viewModel.Operation);
            Assert.Equal(string.Empty, _viewModel.MainDisplayText);
            Assert.Equal(string.Empty, _viewModel.BottomDisplayText);
            Assert.Equal(string.Empty, _viewModel.ErrorDisplayText);
        }
    }
}
