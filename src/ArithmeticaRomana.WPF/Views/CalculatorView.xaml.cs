using ArithmeticaRomana.Core.Formatter;
using ArithmeticaRomana.Core.Parser;
using ArithmeticaRomana.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ArithmeticaRomana.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        private static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CalculatorViewModel), typeof(CalculatorView));
        public CalculatorViewModel ViewModel
        {
            get => (CalculatorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public CalculatorView()
        {
            InitializeComponent();
            ViewModel ??= new CalculatorViewModel(new VinculumRomanNumeralFormatter(), new VinculumRomanNumeralParser());
        }
    }
}
