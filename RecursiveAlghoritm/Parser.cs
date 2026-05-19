namespace Alghoritms
{
    public class ExpressionParser
    {
        public event PrintMessage? Event;
        public delegate void PrintMessage(string message);

        private const string wrongExpression = "Не удалось считать выражение";
        private char[] operators = { '+', '-', '*', '/' };

        public bool TryParse(string expression)
        {
            return CheckExpressionAccuracy(expression);
        }

        //4x-5=11 - true
        //+4x-5=11 - true
        //4abc12+12-92a=28ad-fea+x - true
        //((4x)-1)=2 - false
        //()4x-1=2 - false
        //4x-5=11- - false
        //4x-5+=11 - false
        //(4x+(5+22)/(34+y))+(12x-8y)=8 - true
        private bool CheckExpressionAccuracy(string expression, bool isRecursive = false)
        {
            Event?.Invoke(expression);

            bool hasOperator = false;
            bool isOperator = false;
            bool equalsFlag = false;

            for (int i = 0; i < expression.Length; i++)
            {
                var item = expression[i];
                if (operators.Contains(item))
                {
                    if (isOperator)
                        return false;

                    hasOperator = true;
                    isOperator = true;
                }
                else if(item != '=')
                    isOperator = false;

                if (item == '=')
                {
                    if (equalsFlag || isOperator)
                        return false;
                    equalsFlag = true;
                }

                //(4x+(5+22)/(34+y))+(12x-8y)=8
                //4x+(5+22)/(34+y) - ?
                //5+22 - true
                //34+y - true
                //12x-8y - true
                if (item == '(')
                {
                    int index = i + 1;
                    int startIndex = i + 1;
                    int rightBracketsCount = 0;
                    int leftBracketsCount = 1;
                    while (true)
                    {
                        if (index >= expression.Length || expression[index] == '=')
                            return false;

                        if (expression[index] == '(')
                            leftBracketsCount++;
                        else if (expression[index] == ')')
                        {
                            rightBracketsCount++;
                            if (rightBracketsCount == leftBracketsCount)
                            {
                                if (startIndex >= index)
                                    return false;
                                if (!CheckExpressionAccuracy(expression.Substring(startIndex, index - startIndex), true))
                                    return false;

                                i = index;
                                break;
                            }
                        }
                        index++;
                    }
                    continue;
                }

                if (item == ')')
                    return false;
            }

            if ((!equalsFlag && !isRecursive) || isOperator || !hasOperator)
                return false;
            return true;
        }

    }
}