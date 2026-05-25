using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
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

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                AddDigit(digit);
                e.Handled = true;
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string digit = (e.Key - Key.NumPad0).ToString();
                AddDigit(digit);
                e.Handled = true;
            }
            else switch (e.Key)
                {
                    case Key.Add:
                    case Key.OemPlus:
                        AddOperation("＋");
                        e.Handled = true;
                        break;

                    case Key.Subtract:
                    case Key.OemMinus:
                        AddOperation("－");
                        e.Handled = true;
                        break;

                    case Key.Multiply:
                        AddOperation("×");
                        e.Handled = true;
                        break;

                    case Key.Divide:
                        AddOperation("÷");
                        e.Handled = true;
                        break;

                    case Key.Enter:
                        Calculate();
                        e.Handled = true;
                        break;

                    case Key.Escape:
                       this.Close();
                        e.Handled = true;
                        break;

                    case Key.Back:
                        Backspace();
                        e.Handled = true;
                        break;

                    case Key.Delete:
                        Clear();
                        e.Handled = true;
                        break;

                    case Key.Decimal:
                    case Key.OemPeriod:
                    case Key.OemComma:
                        AddComma();
                        e.Handled = true;
                        break;

                    case Key.OemOpenBrackets:
                        AddChar("(");
                        e.Handled = true;
                        break;

                    case Key.OemCloseBrackets:
                        AddChar(")");
                        e.Handled = true;
                        break;
                }
        }

        private void MouseToButton_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as Button;
            var key = button.Content.ToString();

            switch (key)
            {
                case "C":
                    Clear();
                    break;
                case "←":
                    Backspace();
                    break;
                case "=":
                    break;
                default:
                    AddChar(key);
                    break;
            }
        }

        private void AddDigit(string digit)
        {
            if (Display.Text == "0")
                Display.Text = digit;
            else
                Display.Text += digit;
        }

        private void AddOperation(string op)
        {
            Display.Text += op;
        }

        private void Calculate()
        {
        }

        private void Clear() => Display.Text = "0";

        private void Backspace()
        {
            if (Display.Text.Length > 1)
                Display.Text = Display.Text[0..^1];
            else
                Display.Text = "0";
        }

        private void AddComma()
        {
            if (!Display.Text.Contains(","))
                Display.Text += ",";
        }

        private void AddChar(string key)
        {
            if (Display.Text == "0")
                Display.Text = key;
            else
                Display.Text += key;
        }

        private void Button_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}