using ArithmeticaRomana.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ArithmeticaRomana.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für DisplayView.xaml
    /// </summary>
    public partial class DisplayView : UserControl
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(DisplayViewModel), typeof(DisplayView));
        public DisplayViewModel ViewModel
        {
            get => (DisplayViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public DisplayView()
        {
            InitializeComponent();
        }
    }
}
