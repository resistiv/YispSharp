using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class OrP : ICallable
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
                return true;
            }
            else
            {
                object right = interpreter.Evaluate(arguments[1]);
                return interpreter.IsTruthy(right) ? true : null;
            }
        }
    }
}
