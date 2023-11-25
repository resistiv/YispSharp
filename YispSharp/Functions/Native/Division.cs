using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Handles numeric division.
    /// </summary>
    public class Division : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            double[] d = new double[2];

            // Evaluate each argument
            for (int i = 0; i < 2; i++)
            {
                object arg = interpreter.Evaluate(arguments[i]);
                if (arg is not double)
                {
                    throw new RuntimeException($"Operand '{interpreter.Stringify(arg)}' is not a number.");
                }
                d[i] = (double)arg;
            }

            // Special case
            if (d[1] == 0)
            {
                throw new RuntimeException("Division by zero.");
            }

            return d[0] / d[1];
        }
    }
}
