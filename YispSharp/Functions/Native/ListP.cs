using System.Security.AccessControl;
using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
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
