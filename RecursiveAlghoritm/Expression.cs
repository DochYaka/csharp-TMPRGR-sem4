namespace Alghoritms
{
    public interface IComputed;

    //4x-5=11
    //4x=11+5
    //4x=16
    //x=4
    public class Expression
    {
        private Queue<IComputed> variables = new();
        private Queue<Operators> operators = new();

        public Expression Calculate(IComputed var1, IComputed var2)
        {
            throw new NotImplementedException();
        }
    }

    public class NumericConstant(int value) : IComputed
    {
        public int Value { get; private set; } = value;
    }

    public class Variable(string name, int multiplier = 1) : IComputed
    {
        public string Name { get; private set; } = name;
        public int Multiplier { get; private set; } = multiplier;
    }

    public enum Operators
    {
        Mul,
        Div,
        Add,
        Sub
    }

    public static class OperatorInfo
    {
        public static int GetPrecedence(Operators op)
        {
            return op switch
            {
                Operators.Add or Operators.Sub => 1,
                Operators.Mul or Operators.Div => 2,
                _ => throw new Exception()
            };
        }
    }
}