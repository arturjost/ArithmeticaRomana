using ArithmeticaRomana.WPF.ViewModels;
using System.Windows.Controls;

namespace ArithmeticaRomana.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        public CalculatorView()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
        }
    }
}
