using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Car : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);
            if (obj is List<object> carList)
            {
                return carList[0];
            }
            else if (obj is SExpr.List sl && sl.Values.Count != 0)
            {
                return sl.Values[0];
            }
            else
            {
                throw new RuntimeException($"Operand must be a list.");
            }
        }
    }
}
