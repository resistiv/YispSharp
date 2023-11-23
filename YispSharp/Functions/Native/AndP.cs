using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class AndP : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object left = interpreter.Evaluate(arguments[0]);
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
