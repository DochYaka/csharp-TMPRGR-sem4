using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghoritms
{
    public class AlghoritmPoliz
    {
        public sealed record OperatorInfo(int Priority, bool RightAssociative, int Arity);

        public static readonly Dictionary<string, OperatorInfo> Operators = new()
        {
            ["+"] = new OperatorInfo(1, false, 2),
            ["-"] = new OperatorInfo(1, false, 2),

            ["*"] = new OperatorInfo(2, false, 2),
            ["/"] = new OperatorInfo(2, false, 2),

            ["^"] = new OperatorInfo(3, true, 2),

            ["u+"] = new OperatorInfo(4, true, 1),
            ["u-"] = new OperatorInfo(4, true, 1)
        };

        public static string ConvertToPoliz(string expression)
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
                    while (operators.Count > 0 && operators.Peek() != "(")
                        output.Append(operators.Pop()).Append(' ');

                    if (operators.Count == 0)
                        throw new Exception("Ошибка скобок");

                    operators.Pop();

                    expectOperand = false;

                    continue;
                }

                if ((token == "+" || token == "-") && expectOperand)
                    token = token == "+" ? "u+" : "u-";

                if (!Operators.ContainsKey(token))
                    throw new Exception($"Недопустимый символ: {token}");

                var current = Operators[token];

                while (operators.Count > 0 &&
                       operators.Peek() != "(" &&
                       Operators.ContainsKey(operators.Peek()) &&
                       ShouldPop(Operators[operators.Peek()], current))
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

        public static bool ShouldPop(OperatorInfo stackOp, OperatorInfo currentOp)
        {
            if (stackOp.Priority > currentOp.Priority)
                return true;

            return stackOp.Priority == currentOp.Priority && !currentOp.RightAssociative;
        }

        public static int EvaluatePoliz(string poliz)
        {
            var stack = new Stack<int>();

            var tokens = poliz.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int value))
                {
                    stack.Push(value);

                    continue;
                }

                if (!Operators.TryGetValue(token, out var opInfo))
                    throw new Exception($"Неизвестный оператор: {token}");

                if (stack.Count < opInfo.Arity)
                    throw new Exception("Ошибка ПОЛИЗ");

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
                throw new Exception("Ошибка вычисления");

            return stack.Pop();
        }

        public static string SolveEquation(string equation)
        {
            equation = equation.Replace(" ", "");

            string[] parts = equation.Split('=');

            if (parts.Length != 2)
                throw new Exception("Неверное уравнение");

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
                    return $"x = {x}";
            }

            throw new Exception("Решение не найдено");
        }

        public static int PowInt(int @base, int exponent)
        {
            if (exponent < 0)
                throw new Exception("Степень должна быть >= 0");

            int result = 1;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result *= @base;

                @base *= @base;

                exponent >>= 1;
            }

            return result;
        }
    }
}
