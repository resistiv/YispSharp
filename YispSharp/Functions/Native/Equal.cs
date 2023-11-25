using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Handles atomic equality.
    /// </summary>
    public class Equal : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object[] o = new object[2];

            for (int i = 0; i < 2; i++)
            {
                object arg = interpreter.Evaluate(arguments[i]);
                // "Returns () when either expression is a larger list."
                if (arg is List<object>)
                {
                    return null;
                }
                o[i] = arg;
            }

            return interpreter.IsEqual(o[0], o[1]) ? true : null;
        }
    }
}
