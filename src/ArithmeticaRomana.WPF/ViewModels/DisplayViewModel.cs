using ArithmeticaRomana.Core.Formatter;

namespace ArithmeticaRomana.WPF.ViewModels
{
    public class DisplayViewModel : NotifyPropertyChangedViewModel
    {
        // Left Side 
        private string _leftArabic = string.Empty;
        public string LeftArabic
        {
            get { return _leftArabic; }
            set { SetField(ref _leftArabic, value); }
        }

        private string _leftRoman = string.Empty;
        public string LeftRoman
        {
            get { return _leftRoman; }
            set { SetField(ref _leftRoman, value); }
        }

        // Right Side
        private string _rightArabic = string.Empty;
        public string RightArabic
        {
            get { return _rightArabic; }
            set { SetField(ref _rightArabic, value); }
        }

        private string _rightRoman = string.Empty;
        public string RightRoman
        {
            get { return _rightRoman; }
            set { SetField(ref _rightRoman, value); }
        }


        private string _operation = string.Empty;
        public string Operation
        {
            get { return _operation; }
            set { SetField(ref _operation, value); }
        }


        // Main Display
        private string _mainDisplayText = string.Empty;
        public string MainDisplayText
        {
            get { return _mainDisplayText; }
            set { SetField(ref _mainDisplayText, value); }
        }

        private string _bottomDisplayText = string.Empty;
        public string BottomDisplayText
        {
            get { return _bottomDisplayText; }
            set { SetField(ref _bottomDisplayText, value); }
        }

        private string _errorDisplayText = string.Empty;
        public string ErrorDisplayText
        {
            get { return _errorDisplayText; }
            set { SetField(ref _errorDisplayText, value); }
        }


        private readonly IRomanNumeralFormatter _formatter;
        public DisplayViewModel(IRomanNumeralFormatter formatter)
        {
            _formatter = formatter;
        }

        public void RenderLeftNumber(int value)
        {
            LeftRoman = value > 0 ? _formatter.Format(value) : string.Empty;
            LeftArabic = value == 0 ? string.Empty : value.ToString("N0");
        }
        public void RenderRightNumber(int value)
        {
            RightRoman = value > 0 ? _formatter.Format(value) : string.Empty;
            RightArabic = value == 0 ? string.Empty : value.ToString("N0");
        }
        public void RenderMainDisplay(bool arabic, int value, string operation)
        {
            Operation = operation;
            if (arabic)
            {
                MainDisplayText = value.ToString("N0");
                BottomDisplayText = _formatter.Format(value);
            }
            else
            {
                MainDisplayText = _formatter.Format(value);
                BottomDisplayText = value.ToString("N0");
            }
        }
        public void RenderError(string error)
        {
            ErrorDisplayText = error;
        }
        public void ClearError()
        {
            ErrorDisplayText = string.Empty;
        }
        public void ClearDisplay()
        {
            LeftArabic = string.Empty;
            LeftRoman = string.Empty;
            RightArabic = string.Empty;
            RightRoman = string.Empty;
            Operation = string.Empty;
            MainDisplayText = string.Empty;
            BottomDisplayText = string.Empty;
            ClearError();
        }
    }
}
