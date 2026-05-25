using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Calculator;

public partial class MainWindow : Window
{
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

        { Key.Add, "＋" },
        { Key.OemPlus, "＋" },

        { Key.Subtract, "－" },
        { Key.OemMinus, "－" },

        { Key.Multiply, "×" },
        { Key.Divide, "÷" },

        { Key.Decimal, "," },
        { Key.OemComma, "," },
        { Key.OemPeriod, "," },

        { Key.Back, "←" },
        { Key.Delete, "C" },

        { Key.Enter, "=" }
    };

    private readonly Dictionary<Key, string> ShiftMap = new()
    {
        { Key.D9, "(" },
        { Key.D0, ")" }
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        bool shift = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);

        string value = null;

        if (shift && ShiftMap.TryGetValue(e.Key, out string shiftValue))
        {
            value = shiftValue;
        }
        else if (KeyMap.TryGetValue(e.Key, out string normalValue))
        {
            value = normalValue;
        }

        if (value == null)
            return;

        HandleInput(value);

        e.Handled = true;
    }

    private void HandleInput(string value)
    {
        SimulateClick(value);

        switch (value)
        {
            case "C":
                Clear();
                break;

            case "←":
                Backspace();
                break;

            case "=":
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
        {
            HandleInput(button.Content.ToString());
        }
    }

    private void AddText(string value)
    {
        if (Display.Text == "0")
            Display.Text = value;
        else
            Display.Text += value;
    }

    private void Clear()
    {
        Display.Text = "0";
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
    }

    private async void SimulateClick(string content)
    {
        Button button = FindButtonByContent(content);

        if (button == null)
            return;

        button.Background = Brushes.LightGray;
        button.RenderTransform = new ScaleTransform(0.95, 0.95);

        await Task.Delay(100);

        button.ClearValue(Button.BackgroundProperty);
        button.ClearValue(Button.RenderTransformProperty);
    }

    private Button FindButtonByContent(string content)
    {
        foreach (var child in Buttons.Children)
        {
            if (child is Button button &&
                button.Content?.ToString() == content)
            {
                return button;
            }
        }

        return null;
    }
}