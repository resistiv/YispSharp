using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions
{
    public interface ICallable
    {
        public Range Arity();
        public object Call(Interpreter interpreter, List<SExpr> arguments);
    }
}
