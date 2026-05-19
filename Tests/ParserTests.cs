using Alghoritms;

namespace Tests
{
    [TestClass]
    public sealed class ParserTests
    {
        private ExpressionParser parser = new ExpressionParser();

        //4x-5=11 - true
        //+4x-5=11 - true
        //4abc12+12-92a=28ad-fea+x - true
        //((4x)-1)=2 - false
        //()4x-1=2 - false
        //4x-5=11- - false
        //4x-5+=11 - false
        //(4x+(5+22)/(34+y))+(12x-8y)=8 - true
        [TestMethod]
        [DataRow("4x-5=11")]
        [DataRow("4abc12+12-92a=28ad-fea+x")]
        [DataRow("(4x+(5+22)/(34+y))+(12x-8y)=8")]
        public void TestParse(string expression)
        {
            Assert.IsTrue(parser.TryParse(expression));
        }

        [TestMethod]
        [DataRow("((4x)-1)=2")]
        [DataRow("()4x-1=2")]
        [DataRow("4x-5=11-")]
        [DataRow("4x-5+=11")]
        public void TestParse2(string expression)
        {
            Assert.IsTrue(!parser.TryParse(expression));
        }
    }
}
