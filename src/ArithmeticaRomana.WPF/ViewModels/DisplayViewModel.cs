using ArithmeticaRomana.Core.Formatter;

namespace ArithmeticaRomana.WPF.ViewModels
{
    public class DisplayViewModel : NotifyPropertyChangedViewModel
    {
        private string _leftDisplayText = string.Empty;
        public string LeftDisplayText
        {
            get { return _leftDisplayText; }
            set { SetField(ref _leftDisplayText, value); }
        }


        private string _rightDisplayText = string.Empty;
        public string RightDisplayText
        {
            get { return _rightDisplayText; }
            set { SetField(ref _rightDisplayText, value); }
        }


        private string _operationDisplayText = string.Empty;
        public string OperationDisplayText
        {
            get { return _operationDisplayText; }
            set { SetField(ref _operationDisplayText, value); }
        }


        private string _mainDisplayText = string.Empty;
        public string MainDisplayText
        {
            get { return _mainDisplayText; }
            set { SetField(ref _mainDisplayText, value); }
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

        private void Render(bool arabic, int value, int operationValue, string operation)
        {
            if (operationValue > 0)
                LeftDisplayText = $"({operationValue}) " + _formatter.Format(operationValue);
            RightDisplayText = $"({value}) " + _formatter.Format(value);

            if (arabic)
            {
                MainDisplayText = value.ToString();
                OperationDisplayText = operation;
            }
            else
            {
                MainDisplayText = _formatter.Format(value);
                OperationDisplayText = operation;
            }
        }
        public void RenderArabic(int value, int operationValue, string operation)
        {
            Render(true, value, operationValue, operation);
        }
        public void RenderRoman(int value, int operationValue, string operation)
        {
            Render(false, value, operationValue, operation);
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
            LeftDisplayText = string.Empty;
            RightDisplayText = string.Empty;
            OperationDisplayText = string.Empty;
            MainDisplayText = string.Empty;
            ClearError();
        }
    }
}
