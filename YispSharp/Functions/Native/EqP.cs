using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Checks if two objects occupy the same memory location.
    /// </summary>
    public class EqP : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object o1, o2;
            o1 = interpreter.Evaluate(arguments[0]);
            o2 = interpreter.Evaluate(arguments[1]);

            // Same symbol? Same memory location.
            if (o1 is SExpr.Atom a1 && o2 is SExpr.Atom a2 &&
                a1.Value is Token t1 && a2.Value is Token t2 &&
                t1.Type == TokenType.Symbol && t2.Type == TokenType.Symbol &&
                t1.Lexeme.Equals(t2.Lexeme))
            {
                return true;
            }
            // If not, check object reference
            else if (ReferenceEquals(o1, o2))
            {
                return true;
            }
            else
            {
                return null;
            }
        }
    }
}
