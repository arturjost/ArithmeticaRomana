using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.Core.Parser;
using ArithmeticaRomana.WPF.Commands;
using System.Diagnostics;

namespace ArithmeticaRomana.WPF.ViewModels
{
    public class CalculatorViewModel : NotifyPropertyChangedViewModel
    {
        #region Display & Modes

        private bool _romanMode;
        public bool RomanMode
        {
            get { return _romanMode; }
            set
            {
                SetField(ref _romanMode, value);
                OnPropertyChanged(nameof(ArabicMode));
            }
        }
        public bool ArabicMode
        {
            get { return !_romanMode; }
            set
            {
                SetField(ref _romanMode, !value);
                OnPropertyChanged(nameof(RomanMode));
            }
        }

        private DisplayViewModel _display = new DisplayViewModel(_formatter);
        public DisplayViewModel Display
        {
            get { return _display; }
            set { SetField(ref _display, value); }
        }

        #endregion

        private static readonly IRomanNumeralFormatter _formatter = new VinculumRomanNumeralFormatter();
        private static readonly IRomanNumeralParser _parser = new VinculumRomanNumeralParser();

        private string _currentOperation = string.Empty;
        private int _operationValue = 0;
        private int _currentValue = 0;
        private int _resultValue = 0;
        public CalculatorViewModel()
        {
            RomanMode = true;

            _receiveInputCommand = new DelegateCommand<string>(input =>
            {
                Debug.WriteLine(input);
                switch (input)
                {
                    case null:
                        break;
                    case "Addition":
                        SetOperation("+");
                        break;
                    case "Subtraction":
                        SetOperation("-");
                        break;
                    case "Calculate":
                        Calculate();
                        break;
                    case "Clear":
                        Clear();
                        break;
                    case "Delete":
                        DeleteLastInput();
                        break;
                    case "Switch":
                        Switch();
                        break;
                    default:
                        lastInputs.Push(input);
                        ProcessInput();
                        break;
                }
            });
        }

        private void SetOperation(string operation)
        {
            _currentOperation = operation;
            _operationValue = _currentValue;
            _currentValue = 0;
            _resultValue = 0;
            lastInputs.Clear();
            RenderDisplay();
        }

        private void Calculate()
        {
            if (!string.IsNullOrWhiteSpace(_currentOperation))
            {
                switch (_currentOperation)
                {
                    case "+":
                        if (_resultValue == 0)
                            _resultValue = _currentValue + _operationValue;
                        else
                            _resultValue += _operationValue; ;
                        break;
                    case "-":
                        _resultValue = _currentValue - _operationValue;
                        break;
                }
            }
            RenderDisplay();
        }

        private void Switch()
        {
            RomanMode = !RomanMode;
            var input = string.Join("", lastInputs.Reverse());
            lastInputs.Clear();
            if (RomanMode)
            {
                var result = _formatter.Format(_resultValue);
                foreach (var ch in result)
                {
                    lastInputs.Push(ch.ToString());
                }
            }
            else
            {
                var result = _resultValue.ToString();
                foreach (var ch in result)
                {
                    lastInputs.Push(ch.ToString());
                }
            }

            RenderDisplay();
        }

        private void RenderDisplay()
        {
            var mainValue = _resultValue > 0 ? _resultValue : _currentValue;
            if (RomanMode)
                Display.RenderRoman(mainValue, _operationValue, _currentOperation);
            else
                Display.RenderArabic(mainValue, _operationValue, _currentOperation);
        }

        private void Clear()
        {
            _operationValue = 0;
            _currentValue = 0;
            _resultValue = 0;
            _currentOperation = string.Empty;
            lastInputs.Clear();
            Display.ClearDisplay();
        }


        private Stack<string> lastInputs = new Stack<string>();
        private void DeleteLastInput()
        {
            if (lastInputs.TryPop(out string? lastInput))
            {
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            var newInput = string.Join("", lastInputs.Reverse());
            if (newInput.Length == 0)
            {
                _currentValue = 0;
            }
            else if (ArabicMode)
            {
                if (int.TryParse(newInput, out int newValue))
                {
                    _currentValue = newValue;
                    Display.ClearError();
                }
                else
                {
                    Display.RenderError("The maximum possible value is 2.147.483.647!");
                }
            }
            else if (RomanMode)
            {
                var result = _parser.Parse(newInput);
                if (result.IsSuccess)
                {
                    _currentValue = result.RomanNumeral!.Value.AsInteger;
                    Display.ClearError();
                }
                else
                {
                    Display.RenderError(result.ErrorMessage!);
                }
            }
            else if (newInput.Length > 0)
            {
                _currentValue = int.MaxValue;
            }
            _resultValue = 0;
            RenderDisplay();
        }

        // Commands
        private DelegateCommand<string> _receiveInputCommand;
        public DelegateCommand<string> ReceiveInputCommand
        {
            get { return _receiveInputCommand; }
            set { SetField(ref _receiveInputCommand, value); }
        }
    }
}
