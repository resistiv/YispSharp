using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YispTester
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public void AddOneTest()
        {
            string code = "(define add-one (a) (+ a 1))\n" +
                          "(add-one 1)\n" +
                          "(add-one 2)\n" +
                          "(add-one 57)\n" +
                          // Tests local variable shadowing
                          "(set a 27)\n" +
                          "(add-one 26)";
            string expected = "2\n" +
                              "3\n" +
                              "58\n" +
                              "27";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void FibonacciTest()
        {
            string code = "(define fib (n) (cond (< n 1) () (= n 1) 0 (= n 2) 1 't (+ (fib (- n 1)) (fib (- n 2)))))\n" +
                          "(fib 1)\n" +
                          "(fib 2)\n" +
                          "(fib 5)\n" +
                          "(fib 12)\n" +
                          "(fib 24)\n";
            string expected = "0\n" +
                              "1\n" +
                              "3\n" +
                              "89\n" +
                              "28657";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void BlackjackTest()
        {
            string code = "(define blackjack (hand) (count-cards hand 0 0))\n" +
                          "(define count-cards (hand aces score) (cond (nil? hand) (cond (and? (> aces 0) (< score 12)) (+ 10 score) 't score) (> (car hand) 10) (count-cards (cdr hand) aces (+ 10 score)) (> (car hand) 1) (count-cards (cdr hand) aces (+ (car hand) score)) 't (count-cards (cdr hand) (+ 1 aces) (+ 1 score))))\n" +
                          "(blackjack (list 1 11))\n" +
                          "(blackjack (list 1 1))\n" +
                          "(blackjack (list 1 1 1 13))\n" +
                          "(blackjack (list 7 8 1 1))\n" +
                          "(blackjack (list 1 1 1 1 1))\n" +
                          "(blackjack (list 2 11 1 7))";
            string expected = "21\n" +
                              "12\n" +
                              "13\n" +
                              "17\n" +
                              "15\n" +
                              "20";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void BowlingTest()
        {
            string code = "(define bowling (balls) (next-frame balls 1))\n" +
                          "(define two-balls (balls) (+ (car balls) (car (cdr balls))))\n" +
                          "(define three-balls (balls) (+ (two-balls balls) (car (cdr (cdr balls)))))\n" +
                          "(define strike? (balls) (= (car balls) 10))\n" +
                          "(define spare? (balls) (= (two-balls balls) 10))\n" +
                          "(define next-frame (balls frame) (cond (= frame 10) (cond (> (two-balls balls) 9) (three-balls balls) 't (two-balls balls)) (strike? balls) (+ (three-balls balls) (next-frame (cdr balls) (+ frame 1))) (spare? balls) (+ (three-balls balls) (next-frame (cdr (cdr balls)) (+ frame 1))) 't (+ (two-balls balls) (next-frame (cdr (cdr balls)) (+ frame 1)))))\n" +
                          "(bowling (list 10 10 10 10 10 10 10 10 10 10 10 10))\n" +
                          "(bowling (list 10 3 2 7 1 10 10 2 8 4 5 3 6 5 5 8 2 3))\n" +
                          "(bowling (list 10 5 5 10 7 3 4 3 5 5 3 5 1 0 10 4 4))\n" +
                          "(bowling (list 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0))";
            string expected = "300\n" +
                              "133\n" +
                              "129\n" +
                              "0";

            string output = Tools.RunCode(code);

            Assert.AreEqual(expected, output);
        }
    }
}