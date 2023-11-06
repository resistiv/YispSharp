using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YispTester
{
    /// <summary>
    /// Tests the functionality of the <see cref="YispSharp.Parser"/> class.
    /// </summary>
    /// <remarks>Partially adapted from Robert Nystrom's Lox parsing tests.</remarks>
    [TestClass]
    public class ParserTests
    {
        /// <remarks>Adapted from <see href="https://github.com/munificent/craftinginterpreters/blob/master/test/expressions/parse.lox"/></remarks>
        [TestMethod]
        public void BasicParseTest()
        {
            string code = "(+ (- 5 (- 3 1)) (- 0 1))";
            string expected = "(+ (- 5 (- 3 1)) (- 0 1))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void DefineTest()
        {
            string code = "(define add-two (a b) (+ a b))\n" +
                          "(define example-function (a b c) (+ a (- b c)))\n" +
                          "(define no-args () (+ x y))";
            string expected = "(define add-two (list a b) (+ a b))\n" +
                              "(define example-function (list a b c) (+ a (- b c)))\n" +
                              "(define no-args nil (+ x y))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SetTest()
        {
            string code = "(set example-var 123)\n" +
                          "(set example-var (+ 1 (- 2 (/ 3 (* 4 5)))))\n" +
                          "(set nil-var ())";
            string expected = "(set example-var 123)\n" +
                              "(set example-var (+ 1 (- 2 (/ 3 (* 4 5)))))\n" +
                              "(set nil-var nil)";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }
    }
}