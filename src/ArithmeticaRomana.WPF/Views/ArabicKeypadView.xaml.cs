using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArithmeticaRomana.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für ArabicKeypadView.xaml
    /// </summary>
    public partial class ArabicKeypadView : UserControl
    {
        private static readonly DependencyProperty SendInputCommandProperty = DependencyProperty.Register(nameof(SendInputCommand), typeof(ICommand), typeof(ArabicKeypadView));
        public ICommand SendInputCommand
        {
            get => (ICommand)GetValue(SendInputCommandProperty);
            set => SetValue(SendInputCommandProperty, value);
        }

        public ArabicKeypadView()
        {
            InitializeComponent();
        }
    }
}
