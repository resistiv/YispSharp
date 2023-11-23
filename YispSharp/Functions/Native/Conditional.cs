using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Conditional : ICallable
    {
        public Range Arity()
        {
            return new Range(2, -1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            // Need cond-result pair
            if (arguments.Count % 2 != 0)
            {
                throw new RuntimeException("Expected an even number of arguments for a conditional structure.");
            }

            // Traverse each pair
            for (int i = 0; i < arguments.Count; i += 2)
            {
                object cond = interpreter.Evaluate(arguments[i]);
                if (interpreter.IsTruthy(cond))
                {
                    return interpreter.Evaluate(arguments[i + 1]);
                }
            }

            throw new RuntimeException("Malformed conditional structure, no condition evaluated to true.");
        }
    }
}
