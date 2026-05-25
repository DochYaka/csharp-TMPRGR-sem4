using System.Text;

namespace Alghoritms
{
    public class ExpressionSolver
    {
        //Для тестирования(потом удалить)
        public event PrintMessage? Event;
        public delegate void PrintMessage(string message);

        internal enum TokenType
        {
            Digit, Operator, Variable, Equals, RightBracket, LeftBracket
        }

        internal class Token(string value, TokenType type)
        {
            public string Value { get; } = value;
            public TokenType Type { get; } = type;
        }

        private readonly HashSet<string> operators;
        private readonly Dictionary<char, Func<double, double, double>> _operations;

        public ExpressionSolver()
        {
            operators = ["+", "-", "*", "/", "^", "u-"];
            _operations = new Dictionary<char, Func<double, double, double>>
            {
                ['+'] = (a, b) => a + b,
                ['-'] = (a, b) => a - b,
                ['*'] = (a, b) => a * b,
                ['/'] = (a, b) => a / b,
                ['^'] = (a, b) => Math.Pow(a, b)
            };
        }

        public string Solve(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Выражение пустое!");

            List<Token> tokens = Tokenize(expression);

            if (!ValidateExpression(tokens))
                throw new Exception("Выражение записано неверно!");

            throw new NotImplementedException();
        }

        private int GetPrecedence(Token token)
        {
            if (token.Type != TokenType.Operator)
                return -1;

            return token.Value switch
            {
                "+" or "-" => 1,
                "*" or "/" => 2,
                "^" => 3,
                "u-" => 4,
                _ => throw new Exception("Недопустимый оператор")
            };
        }

        //Проверяет выражение на корретный ввод
        private bool ValidateExpression(List<Token> tokens)
        {
            if (tokens.Count == 0)
                return false;

            int bracketBalance = 0;
            bool hasOperator = false;
            bool lastWasOperator = false;
            bool equalsFlag = false;

            foreach (var item in tokens)
            {
                if (item.Type == TokenType.Operator)
                {
                    if (lastWasOperator)
                        return false;

                    if (!hasOperator)
                        hasOperator = true;

                    lastWasOperator = true;
                }
                else if (item.Type == TokenType.Equals)
                {
                    if (equalsFlag || lastWasOperator)
                        return false;
                    equalsFlag = true;
                    lastWasOperator = false;
                }
                else if (item.Type == TokenType.LeftBracket)
                {
                    lastWasOperator = false;
                    bracketBalance++;
                    hasOperator = false;
                }
                else if (item.Type == TokenType.RightBracket)
                {
                    lastWasOperator = false;
                    bracketBalance--; 
                    if (bracketBalance < 0 || !hasOperator)
                        return false;
                }
                else
                {
                    lastWasOperator = false;
                }
            }

            return !(!equalsFlag || lastWasOperator || !hasOperator || bracketBalance != 0);
        }

        //Разбивает выражение на токены
        private List<Token> Tokenize(string expression)
        {
            List<Token> tokens = new();
            StringBuilder currentNumber = new StringBuilder();

            bool isDouble = false;

            for(int i = 0; i < expression.Length; i++)
            {
                var ch = expression[i];

                if (char.IsDigit(ch) || ch == '.')
                {
                    if (ch == '.' && !isDouble)
                        isDouble = true;
                    else
                        throw new Exception("Ошибочный токен");

                    currentNumber.Append(ch);
                }
                
            }

            throw new NotImplementedException();

            return tokens;
        } 
    }
}