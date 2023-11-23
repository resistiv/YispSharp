using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Cdr : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);
            if (obj is List<object> cdrList)
            {
                if (cdrList.Count == 2)
                {
                    return cdrList[1];
                }
                else
                {
                    return cdrList.Skip(1).ToList();
                }
            }
            else
            {
                throw new RuntimeException("Operand must be a list.");
            }
        }
    }
}
