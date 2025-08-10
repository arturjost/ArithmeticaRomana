using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArithmeticaRomana.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für RomanKeypadView.xaml
    /// </summary>
    public partial class RomanKeypadView : UserControl
    {
        private static readonly DependencyProperty SendInputCommandProperty = DependencyProperty.Register(nameof(SendInputCommand), typeof(ICommand), typeof(RomanKeypadView));
        public ICommand SendInputCommand
        {
            get => (ICommand)GetValue(SendInputCommandProperty);
            set => SetValue(SendInputCommandProperty, value);
        }

        private Button[] _numeralButtons;
        public RomanKeypadView()
        {
            InitializeComponent();
            _numeralButtons = [IButton, VButton, XButton, LButton, CButton, DButton, MButton];
        }

        private int currenShift = 0;
        private void Shift_Click(object sender, RoutedEventArgs e)
        {
            if (currenShift < 2)
                currenShift++;
            else
                currenShift = 0;

            for (int i = 0; i < _numeralButtons.Length; i++)
            {
                _numeralButtons[i].Content = _numeralButtons[i].Name[0].ToString();
                for (int j = 0; j < currenShift; j++)
                {
                    _numeralButtons[i].Content += "\u0305";
                }
            }
        }
    }
}
