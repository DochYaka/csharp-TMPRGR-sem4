using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Calculator;

public partial class MainWindow : Window
{
    private bool IsCalculated = false;

    private sealed record OperatorInfo(
        int Priority,
        bool RightAssociative,
        int Arity);

    private static readonly Dictionary<string, OperatorInfo> Operators = new()
    {
        ["+"] = new OperatorInfo(1, false, 2),
        ["-"] = new OperatorInfo(1, false, 2),

        ["*"] = new OperatorInfo(2, false, 2),
        ["/"] = new OperatorInfo(2, false, 2),

        ["^"] = new OperatorInfo(3, true, 2),

        ["u+"] = new OperatorInfo(4, true, 1),
        ["u-"] = new OperatorInfo(4, true, 1)
    };

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
        {
            value = shiftValue;
        }
        else if (KeyMap.TryGetValue(e.Key, out string normalValue))
        {
            value = normalValue;
        }

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
        {
            HandleKey(button.Content.ToString());
        }
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
        {
            IsCalculated = false;
        }

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
                string result = SolveEquation(expression);

                Display.Text = result;
            }
            else
            {
                string poliz = ConvertToPoliz(expression);

                int result = EvaluatePoliz(poliz);

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

    private static string ConvertToPoliz(string expression)
    {
        var output = new StringBuilder();

        var operators = new Stack<string>();

        var number = new StringBuilder();

        bool expectOperand = true;

        void FlushNumber()
        {
            if (number.Length > 0)
            {
                output.Append(number).Append(' ');

                number.Clear();

                expectOperand = false;
            }
        }

        for (int i = 0; i < expression.Length; i++)
        {
            char ch = expression[i];

            if (char.IsWhiteSpace(ch))
            {
                FlushNumber();
                continue;
            }

            if (char.IsDigit(ch))
            {
                number.Append(ch);
                continue;
            }

            FlushNumber();

            string token = ch.ToString();

            if (token == "(")
            {
                operators.Push(token);

                expectOperand = true;

                continue;
            }

            if (token == ")")
            {
                while (operators.Count > 0 &&
                       operators.Peek() != "(")
                {
                    output.Append(operators.Pop()).Append(' ');
                }

                if (operators.Count == 0)
                    throw new Exception("Ошибка скобок");

                operators.Pop();

                expectOperand = false;

                continue;
            }

            if ((token == "+" || token == "-") && expectOperand)
            {
                token = token == "+" ? "u+" : "u-";
            }

            if (!Operators.ContainsKey(token))
            {
                throw new Exception($"Недопустимый символ: {token}");
            }

            var current = Operators[token];

            while (operators.Count > 0 &&
                   operators.Peek() != "(" &&
                   Operators.ContainsKey(operators.Peek()) &&
                   ShouldPop(
                       Operators[operators.Peek()],
                       current))
            {
                output.Append(operators.Pop()).Append(' ');
            }

            operators.Push(token);

            expectOperand = true;
        }

        FlushNumber();

        while (operators.Count > 0)
        {
            string op = operators.Pop();

            if (op == "(")
                throw new Exception("Ошибка скобок");

            output.Append(op).Append(' ');
        }

        return output.ToString().Trim();
    }

    private static bool ShouldPop(
        OperatorInfo stackOp,
        OperatorInfo currentOp)
    {
        if (stackOp.Priority > currentOp.Priority)
            return true;

        return stackOp.Priority == currentOp.Priority
               && !currentOp.RightAssociative;
    }

    private static int EvaluatePoliz(string poliz)
    {
        var stack = new Stack<int>();

        var tokens = poliz.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string token in tokens)
        {
            if (int.TryParse(token, out int value))
            {
                stack.Push(value);

                continue;
            }

            if (!Operators.TryGetValue(token, out var opInfo))
            {
                throw new Exception($"Неизвестный оператор: {token}");
            }

            if (stack.Count < opInfo.Arity)
            {
                throw new Exception("Ошибка ПОЛИЗ");
            }

            if (opInfo.Arity == 1)
            {
                int a = stack.Pop();

                int unaryResult = token switch
                {
                    "u+" => a,
                    "u-" => -a,
                    _ => throw new Exception("Ошибка унарного оператора")
                };

                stack.Push(unaryResult);

                continue;
            }

            int b = stack.Pop();

            int a2 = stack.Pop();

            int result = token switch
            {
                "+" => a2 + b,

                "-" => a2 - b,

                "*" => a2 * b,

                "/" when b != 0 => a2 / b,

                "/" => throw new DivideByZeroException(),

                "^" => PowInt(a2, b),

                _ => throw new Exception("Неизвестный оператор")
            };

            stack.Push(result);
        }

        if (stack.Count != 1)
        {
            throw new Exception("Ошибка вычисления");
        }

        return stack.Pop();
    }

    private static string SolveEquation(string equation)
    {
        equation = equation.Replace(" ", "");

        string[] parts = equation.Split('=');

        if (parts.Length != 2)
        {
            throw new Exception("Неверное уравнение");
        }

        string left = parts[0];

        string right = parts[1];

        for (int x = -1000; x <= 1000; x++)
        {
            string leftExpr = left.Replace("x", x.ToString());

            string rightExpr = right.Replace("x", x.ToString());

            string leftPoliz = ConvertToPoliz(leftExpr);

            string rightPoliz = ConvertToPoliz(rightExpr);

            int leftValue = EvaluatePoliz(leftPoliz);

            int rightValue = EvaluatePoliz(rightPoliz);

            if (leftValue == rightValue)
            {
                return $"x = {x}";
            }
        }

        throw new Exception("Решение не найдено");
    }

    private static int PowInt(int @base, int exponent)
    {
        if (exponent < 0)
        {
            throw new Exception("Степень должна быть >= 0");
        }

        int result = 1;

        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                result *= @base;
            }

            @base *= @base;

            exponent >>= 1;
        }

        return result;
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
            if (child is Button button &&
                button.Content?.ToString() == content)
            {
                return button;
            }
        }

        return null;
    }
}