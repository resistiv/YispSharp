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

        [TestMethod]
        public void ConsTest()
        {
            string code = "(cons 1 ())\n" +
                          "(cons 1 (cons 2 (cons 3 ())))\n" +
                          "(cons (+ 3 4) (cons (/ 10 5) (cons (* 7 2) (cons (- 2 1) ()))))";
            string expected = "(cons 1 nil)\n" +
                              "(cons 1 (cons 2 (cons 3 nil)))\n" +
                              "(cons (+ 3 4) (cons (/ 10 5) (cons (* 7 2) (cons (- 2 1) nil))))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void CondTest()
        {
            string code = "(cond ((= 1 2) ()))\n" +
                          "(cond ((= a 1) 2) ((> a 1) (/ a 2)) ((< a 1) ()))";
            string expected = "(cond (list (= 1 2) nil))\n" +
                              "(cond (list (= a 1) 2) (list (> a 1) (/ a 2)) (list (< a 1) nil))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void CarTest()
        {
            string code = "(car (1 2 3 4 5))\n" +
                          "(car (cons 3 (cons 2 (cons 1 ()))))";
            string expected = "(car (list 1 2 3 4 5))\n" +
                              "(car (cons 3 (cons 2 (cons 1 nil))))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void CdrTest()
        {
            string code = "(cdr (cdr (1 2 3 4 5)))\n" +
                          "(cdr (cons 3 (cons 2 (cons 1 ()))))";
            string expected = "(cdr (cdr (list 1 2 3 4 5)))\n" +
                              "(cdr (cons 3 (cons 2 (cons 1 nil))))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void AndPTest()
        {
            string code = "(and? (+ 1 2) ())\n" +
                          "(and? () (+ 1 2))\n" +
                          "(and? (+ 1 (/ 10 5)) (* 2 (- 8 7)))";
            string expected = "(and? (+ 1 2) nil)\n" +
                              "(and? nil (+ 1 2))\n" +
                              "(and? (+ 1 (/ 10 5)) (* 2 (- 8 7)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void OrPTest()
        {
            string code = "(or? (+ 1 2) ())\n" +
                          "(or? () (+ 1 2))\n" +
                          "(or? (+ 1 (/ 10 5)) (* 2 (- 8 7)))";
            string expected = "(or? (+ 1 2) nil)\n" +
                              "(or? nil (+ 1 2))\n" +
                              "(or? (+ 1 (/ 10 5)) (* 2 (- 8 7)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NotPTest()
        {
            string code = "(not? ())\n" +
                          "(not? 1)\n" +
                          "(not? (1 2 3))\n" +
                          "(not? t)";
            string expected = "(not? nil)\n" +
                              "(not? 1)\n" +
                              "(not? (list 1 2 3))\n" +
                              "(not? True)";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NumberPTest()
        {
            string code = "(number? 1)\n" +
                          "(number? (1))\n" +
                          "(number? ())\n" +
                          "(number? test)\n" +
                          "(number? (+ 37 (- 7 5)))";
            string expected = "(number? 1)\n" +
                              "(number? (list 1))\n" +
                              "(number? nil)\n" +
                              "(number? test)\n" +
                              "(number? (+ 37 (- 7 5)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SymbolPTest()
        {
            string code = "(symbol? 1)\n" +
                          "(symbol? (1))\n" +
                          "(symbol? ())\n" +
                          "(symbol? test)\n" +
                          "(symbol? (+ 37 (- 7 5)))";
            string expected = "(symbol? 1)\n" +
                              "(symbol? (list 1))\n" +
                              "(symbol? nil)\n" +
                              "(symbol? test)\n" +
                              "(symbol? (+ 37 (- 7 5)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ListPTest()
        {
            string code = "(list? 1)\n" +
                          "(list? (1))\n" +
                          "(list? ())\n" +
                          "(list? test)\n" +
                          "(list? (+ 37 (- 7 5)))";
            string expected = "(list? 1)\n" +
                              "(list? (list 1))\n" +
                              "(list? nil)\n" +
                              "(list? test)\n" +
                              "(list? (+ 37 (- 7 5)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NilPTest()
        {
            string code = "(nil? 1)\n" +
                          "(nil? (1))\n" +
                          "(nil? ())\n" +
                          "(nil? test)\n" +
                          "(nil? (+ 37 (- 7 5)))";
            string expected = "(nil? 1)\n" +
                              "(nil? (list 1))\n" +
                              "(nil? nil)\n" +
                              "(nil? test)\n" +
                              "(nil? (+ 37 (- 7 5)))";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void EqPTest()
        {
            string code = "(eq? a a)\n" +
                          "(eq? a b)\n" +
                          "(eq? (1 2 3 4 5) (1 2 3 4 5))\n" +
                          "(eq? (+ 1 2) 3)";
            string expected = "(eq? a a)\n" +
                              "(eq? a b)\n" +
                              "(eq? (list 1 2 3 4 5) (list 1 2 3 4 5))\n" +
                              "(eq? (+ 1 2) 3)";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void OperationsTest()
        {
            string code = "(+ 1 2)\n" +
                          "(- 1 2)\n" +
                          "(/ 10 5)\n" +
                          "(* 5 3)\n" +
                          "(= 1 1)\n" +
                          "(= 1 2)";
            string expected = "(+ 1 2)\n" +
                              "(- 1 2)\n" +
                              "(/ 10 5)\n" +
                              "(* 5 3)\n" +
                              "(= 1 1)\n" +
                              "(= 1 2)";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }
    }
}