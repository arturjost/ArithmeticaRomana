using System.Windows;
using System.Windows.Input;

namespace ArithmeticaRomana.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculatorView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Clear");
                    break;
                case Key.Enter:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Calculate");
                    break;
                case Key.LeftCtrl:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Switch");
                    break;
                case Key.Delete:
                case Key.Back:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Delete");
                    break;
                case Key.OemPlus:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Addition");
                    break;
                case Key.OemMinus:
                    Calculator.ViewModel.ReceiveInputCommand.Execute("Subtraction");
                    break;
                default:
                    string input = string.Empty;
                    if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    {
                        input = e.Key.ToString().Last().ToString();
                    }
                    else if(e.Key == Key.I || e.Key == Key.V || e.Key == Key.X || e.Key == Key.L || e.Key == Key.C || e.Key == Key.D || e.Key == Key.M)
                    {
                        input += e.Key.ToString();
                    }
                    Calculator.ViewModel.ReceiveInputCommand.Execute(input);
                    break;
            }
        }
    }
}