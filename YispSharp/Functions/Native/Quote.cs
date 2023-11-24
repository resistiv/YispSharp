using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Quote : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            return arguments[0];
        }
    }
}
