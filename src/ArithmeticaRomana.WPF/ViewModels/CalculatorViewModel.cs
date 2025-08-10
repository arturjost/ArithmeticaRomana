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

        private DisplayViewModel _display;
        public DisplayViewModel Display
        {
            get { return _display; }
            set { SetField(ref _display, value); }
        }

        #endregion

        #region Calculator Logic
        private string? _currentOperation = string.Empty;
        private int? _lastValue;
        private int? _operationValue;
        private int _displayValue = 0;

        private void SetOperation(string operation)
        {
            _currentOperation = operation ?? string.Empty;
            _operationValue = null;
            _lastValue = _displayValue;
            lastInputs.Clear();
            RenderDisplay();
        }

        public void SetOperationValue(int value)
        {
            _operationValue = value;
        }

        public void Evaluate()
        {
            if (string.IsNullOrWhiteSpace(_currentOperation))
                return;

            if (!_lastValue.HasValue || _operationValue.HasValue)
                _lastValue = _displayValue;

            if (!_operationValue.HasValue)
                _operationValue = _displayValue;

            _displayValue = _currentOperation switch
            {
                "+" => _lastValue.Value + _operationValue.Value,
                "-" => _lastValue.Value - _operationValue.Value,
                _ => _displayValue
            };

            if (!_operationValue.HasValue)
                _operationValue = _lastValue;

            RenderDisplay();
        }

        #endregion

        private readonly IRomanNumeralFormatter _formatter;
        private readonly IRomanNumeralParser _parser;
        public CalculatorViewModel(IRomanNumeralFormatter formatter, IRomanNumeralParser parser)
        {
            _formatter = formatter;
            _parser = parser;
            _display = new DisplayViewModel(formatter);
            RomanMode = false;
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
                        Evaluate();
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
                        ProcessInput(input);
                        break;
                }
            });
            RenderDisplay();
        }

        private void Switch()
        {
            RomanMode = !RomanMode;
            var input = string.Join("", lastInputs.Reverse());
            lastInputs.Clear();
            if (RomanMode)
            {
                var result = _formatter.Format(_displayValue);
                foreach (var ch in result)
                {
                    lastInputs.Push(ch.ToString());
                }
            }
            else
            {
                var result = _displayValue.ToString();
                foreach (var ch in result)
                {
                    lastInputs.Push(ch.ToString());
                }
            }

            RenderDisplay();
        }

        private void RenderDisplay()
        {
            if (_lastValue.HasValue)
                Display.RenderLeftNumber(_lastValue.Value);

            if (_operationValue.HasValue)
                Display.RenderRightNumber(_operationValue.Value);
            else
                Display.RenderRightNumber(0);

            if (RomanMode)
                Display.RenderMainDisplay(ArabicMode, _displayValue, _currentOperation!);
            else
                Display.RenderMainDisplay(ArabicMode, _displayValue, _currentOperation!);
        }

        private void Clear()
        {
            _operationValue = null;
            _lastValue = null;
            _displayValue = 0;
            _currentOperation = string.Empty;
            lastInputs.Clear();
            Display.ClearDisplay();
            RenderDisplay();
        }


        private readonly Stack<string> lastInputs = new();
        private void ProcessInput(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                lastInputs.Push(input);

            var currentInput = string.Join("", lastInputs.Reverse());
            if (currentInput.Length == 0)
            {
                _displayValue = 0;
                lastInputs.Clear();
            }
            else if (ArabicMode)
            {
                if (int.TryParse(currentInput, out int newValue))
                {
                    _displayValue = newValue;
                    Display.ClearError();
                }
                else
                {
                    DeleteLastInput();
                    Display.RenderError("The maximum possible value is 2.147.483.647!");
                }
            }
            else if (RomanMode)
            {
                var result = _parser.Parse(currentInput);
                if (result.IsSuccess)
                {
                    _displayValue = result.RomanNumeral!.Value.AsInteger;
                    Display.ClearError();
                }
                else
                {
                    DeleteLastInput();
                    Display.RenderError(result.ErrorMessage!);
                }
            }
            else if (currentInput.Length > 0)
            {
                _displayValue = int.MaxValue;
            }

            RenderDisplay();
        }
        private void DeleteLastInput()
        {
            if (lastInputs.TryPop(out string? _))
            {
                ProcessInput(string.Empty);
            }
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
