using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Handles an "and" comparison of two arguments.
    /// </summary>
    public class AndP : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object left = interpreter.Evaluate(arguments[0]);

            // Separate evaluations to properly implement short-circuiting.
            if (interpreter.IsTruthy(left))
            {
                object right  = interpreter.Evaluate(arguments[1]);
                return interpreter.IsTruthy(right) ? true : null;
            }
            else
            {
                return null;
            }
        }
    }
}
