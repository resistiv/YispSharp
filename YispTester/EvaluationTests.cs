using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YispTester
{
    [TestClass]
    public class EvaluationTests
    {
        [TestMethod]
        public void AdditionTest()
        {
            string code = "(+ 1 2)\n" +
                          "(+ 3 4)\n" +
                          "(+ (+ 1 2) 3)\n" +
                          "(+ 1 (+ 2 3))\n" +
                          "(+ 0.25 0.75)";
            string expected = "3\n" +
                              "7\n" +
                              "6\n" +
                              "6\n" +
                              "1";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            string code = "(- 2 1)\n" +
                          "(- 1 2)\n" +
                          "(- (- 27 20) 4)\n" +
                          "(- 150 (- 40 31))\n" +
                          "(- 1.25 0.25)";
            string expected = "1\n" +
                              "-1\n" +
                              "3\n" +
                              "141\n" +
                              "1";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            string code = "(* 1 2)\n" +
                          "(* 7 3)\n" +
                          "(* (* 2 3) 4)\n" +
                          "(* 25 (* 4 3))\n" +
                          "(* 4 0.25)";
            string expected = "2\n" +
                              "21\n" +
                              "24\n" +
                              "300\n" +
                              "1";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void DivisionTest()
        {
            string code = "(/ 4 2)\n" +
                          "(/ 10 2)\n" +
                          "(/ (/ 50 5) 5)\n" +
                          "(/ 63 (/ 27 3))\n" +
                          "(/ 1 4)\n" +
                          "(/ 4 0.25)";
            string expected = "2\n" +
                              "5\n" +
                              "2\n" +
                              "7\n" +
                              "0.25\n" +
                              "16";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void EqualsTest()
        {
            string code = "(= 1 1)\n" +
                          "(= 30 30)\n" +
                          "(= (+ 3 10) 13)\n" +
                          "(= 28 (+ 19 9))\n" +
                          "(= 0.25 0.25)\n" +
                          "(= 1 2)\n" +
                          "(= (1 2) 1)\n" +
                          "(= 3 (4 5))\n" +
                          "(= (1 2 3) (1 2 3))\n" +
                          "(= () ())";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "()\n" +
                              "()\n" +
                              "()\n" +
                              "()\n" +
                              "t";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void LessThanTest()
        {
            string code = "(< 1 2)\n" +
                          "(< 3 9)\n" +
                          "(< (+ 1 2) 5)\n" +
                          "(< 2 (+ 7 5))\n" +
                          "(< 1.25 3.65)" +
                          "(< 1 1)\n" +
                          "(< 2 1)\n" +
                          "(< 3.73 3.72)";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "()\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void GreaterThanTest()
        {
            string code = "(> 2 1)\n" +
                          "(> 9 3)\n" +
                          "(> (+ 3 2) 3)\n" +
                          "(> 12 (+ 1 1))\n" +
                          "(> 3.65 1.25)\n" +
                          "(> 1 1)\n" +
                          "(> 1 2)\n" +
                          "(> 3.72 3.73)";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "()\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ConsTest()
        {
            string code = "(cons 1 ())\n" +
                          "(cons 1 (cons 2 ()))\n" +
                          "(cons 1 (cons 2 (cons 3 ())))\n" +
                          "(cons 1 2)\n" +
                          "(cons 1 (2 3 4 5))\n" +
                          "(cons (1 2) (3 4 5))\n" +
                          "(cons (1 2) ())\n" +
                          "(cons () ())\n" +
                          "(cons () (cons () ()))";
            string expected = "(1)\n" +
                              "(1 2)\n" +
                              "(1 2 3)\n" +
                              "(1 . 2)\n" +
                              "(1 2 3 4 5)\n" +
                              "((1 2) 3 4 5)\n" +
                              "((1 2))\n" +
                              "(())\n" +
                              "(() ())";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void CarCdrTest()
        {
            string code = "(car (1 2 3 4 5))\n" +
                          "(cdr (1 2 3 4 5))\n" +
                          "(car (cdr (1 2 3 4 5)))\n" +
                          "(cdr (cdr (1 2 3 4 5)))\n" +
                          "(car (cdr (cdr (1 2 3 4 5))))\n" +
                          "(cdr (cdr (cdr (1 2 3 4 5))))\n" +
                          "(car (cdr (cdr (cdr (1 2 3 4 5)))))\n" +
                          "(cdr (cdr (cdr (cdr (1 2 3 4 5)))))\n" +
                          "(car (cdr (cdr (cdr (cdr (1 2 3 4 5))))))\n" +
                          "(cdr (cdr (cdr (cdr (cdr (1 2 3 4 5))))))";
            string expected = "1\n" +
                              "(2 3 4 5)\n" +
                              "2\n" +
                              "(3 4 5)\n" +
                              "3\n" +
                              "(4 5)\n" +
                              "4\n" +
                              "(5)\n" +
                              "5\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NumberPTest()
        {
            string code = "(number? 1)\n" +
                          "(number? 10)\n" +
                          "(number? (+ 1 2))\n" +
                          "(number? 1.2345)\n" +
                          "(number? ())\n" +
                          "(number? (1 2 3 4 5))\n" +
                          // FIXME: Change to quote once implemented
                          "(number? abc)";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "()\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SymbolPTest()
        {
            string code = "(symbol? ())\n" +
                          "(symbol? 123)\n" +
                          "(symbol? (1 2 3 4 5))\n" +
                          "(symbol? 1.2345)\n" +
                          // FIXME: Change to quote once implemented
                          "(symbol? abc)\n";
            string expected = "()\n" +
                              "()\n" +
                              "()\n" +
                              "()\n" +
                              "t";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ListPTest()
        {
            string code = "(list? ())\n" +
                          "(list? 123)\n" +
                          "(list? (1 2 3 4 5))\n" +
                          "(list? 1.2345)\n" +
                          // FIXME: Change to quote once implemented
                          "(list? abc)";
            string expected = "t\n" +
                              "()\n" +
                              "t\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NilPTest()
        {
            string code = "(nil? ())\n" +
                          "(nil? 123)\n" +
                          "(nil? (1 2 3 4 5))\n" +
                          "(nil? 1.2345)\n" +
                          // FIXME: Change to quote once implemented
                          "(nil? abc)";
            string expected = "t\n" +
                              "()\n" +
                              "()\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void AndPTest()
        {
            string code = "(and? 1 1)\n" +
                          "(and? 1 (1 2 3))\n" +
                          "(and? (1 2 3) 1)\n" +
                          "(and? (= 1 1) (= 1 2))\n" +
                          "(and? (= 1 1) (= 1 1))\n" +
                          "(and? 1 ())\n" +
                          "(and? () ())";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "()\n" +
                              "t\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void OrPTest()
        {
            string code = "(or? 1 1)\n" +
                          "(or? () (1 2 3))\n" +
                          "(or? (1 2 3) ())\n" +
                          "(or? (= 1 1) (= 1 2))\n" +
                          "(or? (= 1 1) (= 1 1))\n" +
                          "(or? () ())";
            string expected = "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "t\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NotPTest()
        {
            // FIXME: Change to quote once implemented
            string code = "(not? t)\n" +
                          "(not? ())\n" +
                          "(not? (= 1 1))\n" +
                          "(not? 1)\n" +
                          "(not? (1 2 3 4 5))";
            string expected = "()\n" +
                              "t\n" +
                              "()\n" +
                              "()\n" +
                              "()";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }
    }
}