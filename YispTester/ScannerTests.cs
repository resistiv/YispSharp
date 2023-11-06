using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YispTester
{
    /// <summary>
    /// Tests the functionality of the <see cref="YispSharp.Scanner"/> class.
    /// </summary>
    /// <remarks>Partially adapted from Robert Nystrom's Lox scanner tests.</remarks>
    [Ignore] // No longer in place after initial scanner testing.
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public void EmptyTest()
        {
            string code = "";
            string expected = "Eof  null";

            string output = Tools.RunCode(code);
            
            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void SymbolsTest()
        {
            string code = "abc zyx a123 two-words three-more-words custom-function-bool? ???";
            string expected = "Symbol abc null\n" +
                              "Symbol zyx null\n" +
                              "Symbol a123 null\n" +
                              "Symbol two-words null\n" +
                              "Symbol three-more-words null\n" +
                              "Symbol custom-function-bool? null\n" +
                              "Symbol ??? null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void InvalidSymbolsTest()
        {
            string code = "-func 1symbol -123 -?";
            string expected = "Minus - null\n" +
                              "Symbol func null\n" +
                              "Number 1 1\n" +
                              "Symbol symbol null\n" +
                              "Minus - null\n" +
                              "Number 123 123\n" +
                              "Minus - null\n" +
                              "Symbol ? null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void KeywordsTest()
        {
            string code = "define set cons cond car cdr and? or? not? number? symbol? list? nil? eq?";
            string expected = "Define define null\n" +
                              "Set set null\n" +
                              "Cons cons null\n" +
                              "Cond cond null\n" +
                              "Car car null\n" +
                              "Cdr cdr null\n" +
                              "AndP and? null\n" +
                              "OrP or? null\n" +
                              "NotP not? null\n" +
                              "NumberP number? null\n" +
                              "SymbolP symbol? null\n" +
                              "ListP list? null\n" +
                              "NilP nil? null\n" +
                              "EqP eq? null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void NumbersTest()
        {
            string code = "123 123.456 0123 123.0";
            string expected = "Number 123 123\n" +
                              "Number 123.456 123.456\n" +
                              "Number 0123 123\n" +
                              "Number 123.0 123\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void OperationsTest()
        {
            string code = "+-*/=<>()";
            string expected = "Plus + null\n" +
                              "Minus - null\n" +
                              "Star * null\n" +
                              "Slash / null\n" +
                              "Equal = null\n" +
                              "LessThan < null\n" +
                              "GreaterThan > null\n" +
                              "LeftParentheses ( null\n" +
                              "RightParentheses ) null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void StringsTest()
        {
            string code = "\"\" \"string\"";
            string expected = "String \"\" \n" +
                              "String \"string\" string\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void CommentsTest()
        {
            string code = "code ; This is a comment 123 - +\n" +
                          "more-code ; One more for the road";
            string expected = "Symbol code null\n" +
                              "Symbol more-code null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void WhitespaceTest()
        {
            string code = "    space\t\t\ttab\n\nnewline\n \tend";
            string expected = "Symbol space null\n" +
                              "Symbol tab null\n" +
                              "Symbol newline null\n" +
                              "Symbol end null\n" +
                              "Eof  null";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }
    }
}