using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Checks if an object is a list.
    /// </summary>
    public class ListP : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);

            if (obj == null || obj is List<object> || obj is SExpr.List)
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
