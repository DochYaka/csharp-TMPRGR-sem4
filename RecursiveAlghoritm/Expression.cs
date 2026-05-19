namespace Alghoritms
{
    //public interface IComputed;

    ////4x-5=11
    ////4x=11+5
    ////4x=16
    ////x=4
    //public class Expression
    //{
    //    public Queue<IComputed> Variables = new();
    //    public Queue<Operator> Operators = new();
    //}

    //public class ExpressionComputings
    //{
    //    public Expression Calculate(Variable var1, Variable var2, Operator op)
    //    {
    //        Expression expression = new Expression();
    //        if (var1.Name == var2.Name)
    //        {
    //            Variable variable;
    //            switch (op)
    //            {
    //                case Operator.Add:
    //                    variable = new Variable(var1.Name, var1.Multiplier + var2.Multiplier);
    //                    break;
    //                case Operator.Mul:
    //                    variable = new Variable(var1.Name, var1.Multiplier * var2.Multiplier);
    //                    break;
    //                case Operator.Div:
    //                    variable = new Variable(var1.Name, var1.Multiplier / var2.Multiplier);
    //                    break;
    //                case Operator.Sub:
    //                    variable = new Variable(var1.Name, var1.Multiplier - var2.Multiplier);
    //                    break;
    //                default:
    //                    throw new Exception();
    //            }

    //            expression.Variables.Enqueue(variable);
    //        }
    //        else
    //        {
    //            expression.Variables.Enqueue(var1);
    //            expression.Variables.Enqueue(var2);
    //            expression.Operators.Enqueue(op);
    //        }

    //        return expression;
    //    }

    //    public Expression Calculate(Variable var1, NumericConstant num2, Operator op)
    //    {
    //        Expression expression = new Expression();

    //        expression.Variables.Enqueue(var1);
    //        expression.Variables.Enqueue(num2);
    //        expression.Operators.Enqueue(op);

    //        return expression;
    //    }

    //    public Expression Calculate(NumericConstant num1, NumericConstant num2, Operator op)
    //    {
    //        Expression expression = new Expression();

    //        NumericConstant variable;
    //        switch (op)
    //        {
    //            case Operator.Add:
    //                variable = new NumericConstant(num1.Value + num2.Value);
    //                break;
    //            case Operator.Mul:
    //                variable = new NumericConstant(num1.Value * num2.Value);
    //                break;
    //            case Operator.Div:
    //                variable = new NumericConstant(num1.Value / num2.Value);
    //                break;
    //            case Operator.Sub:
    //                variable = new NumericConstant(num1.Value - num2.Value);
    //                break;
    //            default:
    //                throw new Exception();
    //        }

    //        expression.Variables.Enqueue(variable);


    //        return expression;
    //    }
    //}

    //public class NumericConstant(int value) : IComputed
    //{
    //    public int Value { get; private set; } = value;
    //}

    //public class Variable(string name, int multiplier = 1) : IComputed
    //{
    //    public string Name { get; private set; } = name;
    //    public int Multiplier { get; private set; } = multiplier;
    //}

    //public enum Operator
    //{
    //    Mul,
    //    Div,
    //    Add,
    //    Sub
    //}

    //public static class OperatorInfo
    //{
    //    public static int GetPrecedence(Operator op)
    //    {
    //        return op switch
    //        {
    //            Operator.Add or Operator.Sub => 1,
    //            Operator.Mul or Operator.Div => 2,
    //            _ => throw new Exception()
    //        };
    //    }
    //}
}