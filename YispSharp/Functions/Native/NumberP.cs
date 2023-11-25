using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Checks if an object is a number.
    /// </summary>
    public class NumberP : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);
            return obj is double || (obj is SExpr.Atom a && a.Value is double) ? true : null;
        }
    }
}
