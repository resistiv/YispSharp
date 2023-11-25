using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Forcibly evaluates an expression.
    /// </summary>
    public class Eval : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);

            // If the expression is unevaluated, evaluate once more.
            if (obj is SExpr s)
            {
                return interpreter.Evaluate(s);
            }
            else
            {
                return obj;
            }
        }
    }
}
