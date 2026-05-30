using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Alghoritms;

namespace Calculator;

public partial class MainWindow : Window
{
    private bool IsCalculated = false;

    private readonly Dictionary<Key, string> KeyMap = new()
    {
        { Key.D0, "0" },
        { Key.NumPad0, "0" },

        { Key.D1, "1" },
        { Key.NumPad1, "1" },

        { Key.D2, "2" },
        { Key.NumPad2, "2" },

        { Key.D3, "3" },
        { Key.NumPad3, "3" },

        { Key.D4, "4" },
        { Key.NumPad4, "4" },

        { Key.D5, "5" },
        { Key.NumPad5, "5" },

        { Key.D6, "6" },
        { Key.NumPad6, "6" },

        { Key.D7, "7" },
        { Key.NumPad7, "7" },

        { Key.D8, "8" },
        { Key.NumPad8, "8" },

        { Key.D9, "9" },
        { Key.NumPad9, "9" },

        { Key.Add, "+" },

        { Key.Subtract, "-" },
        { Key.OemMinus, "-" },

        { Key.Multiply, "*" },
        { Key.Divide, "/" },

        { Key.OemPlus, "=" },

        { Key.Back, "←" },
        { Key.Delete, "C" },

        { Key.Enter, "Вычислить!" },

        { Key.X, "x" },
        { Key.Y, "y" },
        { Key.Z, "z" }
    };

    private readonly Dictionary<Key, string> ShiftMap = new()
    {
        { Key.D9, "(" },
        { Key.D0, ")" },
        { Key.D6, "^" },
        { Key.D8, "*" },
        { Key.OemPlus, "+" }
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        bool shift = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

        string value = null;

        if (e.Key == Key.Escape)
            Close();

        if (shift && ShiftMap.TryGetValue(e.Key, out string shiftValue))
            value = shiftValue;
        else if (KeyMap.TryGetValue(e.Key, out string normalValue))
            value = normalValue;

        if (value == null)
            return;

        SimulateClick(value);

        HandleKey(value);

        e.Handled = true;
    }

    private void HandleKey(string value)
    {
        switch (value)
        {
            case "C":
                Clear();
                break;

            case "←":
                Backspace();
                break;

            case "Вычислить!":
                Calculate();
                break;

            default:
                AddText(value);
                break;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button button)
            HandleKey(button.Content.ToString());
    }

    private void AddText(string value)
    {
        if (IsCalculated && char.IsDigit(value[0]))
        {
            Display.Text = value;

            IsCalculated = false;

            return;
        }

        if (IsCalculated)
            IsCalculated = false;

        if (Display.Text == "0")
            Display.Text = value;
        else
            Display.Text += value;
    }

    private void Clear()
    {
        Display.Text = "0";
        IsCalculated = false;
    }

    private void Backspace()
    {
        if (Display.Text.Length > 1)
            Display.Text = Display.Text[..^1];
        else
            Display.Text = "0";
    }

    private void Calculate()
    {
        try
        {
            string expression = Display.Text;

            expression = expression
                .Replace("×", "*")
                .Replace("÷", "/")
                .Replace("＋", "+")
                .Replace("－", "-");

            if (expression.Contains("x"))
            {
                string result = AlghoritmPoliz.SolveEquation(expression);

                Display.Text = result;
            }
            else
            {
                string poliz = AlghoritmPoliz.ConvertToPoliz(expression);

                int result = AlghoritmPoliz.EvaluatePoliz(poliz);

                Display.Text = result.ToString();
            }

            IsCalculated = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async void SimulateClick(string content)
    {
        Button button = FindButtonByContent(content);

        if (button == null)
            return;

        button.Background = Brushes.LightGray;

        button.RenderTransform =
            new ScaleTransform(0.95, 0.95);

        button.RenderTransformOrigin =
            new Point(0.5, 0.5);

        await Task.Delay(120);

        button.ClearValue(Button.BackgroundProperty);

        button.ClearValue(Button.RenderTransformProperty);

        button.ClearValue(Button.RenderTransformOriginProperty);
    }

    private Button FindButtonByContent(string content)
    {
        foreach (var child in Buttons.Children)
        {
            if (child is Button button && button.Content?.ToString() == content)
                return button;
        }

        return null;
    }

    private void GuideButtonClick(object sender, RoutedEventArgs e)
    {
        var guideWindow = new Window()
    }
}