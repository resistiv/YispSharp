using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Eval : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);
            if (obj is SExpr s)
            {
                return interpreter.Evaluate(s);
            }
            else
            {
                return obj;
            }
        }
    }
}
