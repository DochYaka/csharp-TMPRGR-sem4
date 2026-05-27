using Alghoritms;
using Moq;

namespace Tests
{
    [TestClass]
    public sealed class SolverTests 
    {
        private ExpressionSolver solver = new ExpressionSolver();

        [TestMethod]
        [DataRow("4x-5=11")]
        [DataRow("4abc12+12-92a=28ad-fea+x")]
        [DataRow("(4x+(5+22)/(34+y))+(12x-8y)=8")]
        public void TestChecking(string expression)
        {
            var mock = new Mock<ExpressionSolver>();
            mock.Setup(x => x.CheckExpressionAccuracy("2+3", false))
                .Returns(true);
            Assert.IsTrue(solver.CheckExpressionAccuracy(expression));
        }

        [TestMethod]
        [DataRow("((4x)-1)=2")]
        [DataRow("()4x-1=2")]
        [DataRow("4x-5=11-")]
        [DataRow("4x-5+=11")]
        [DataRow("(4x-5=11")]
        [DataRow("4x-5)=11")]
        public void TestChecking2(string expression)
        {
            Assert.IsTrue(!solver.CheckExpressionAccuracy(expression));
        }
    }
}
