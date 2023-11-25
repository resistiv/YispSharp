using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Fetches the tail of a list.
    /// </summary>
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
                // If there are only two elements left, take the last element
                if (cdrList.Count == 2)
                {
                    return cdrList[1];
                }
                else
                {
                    return cdrList.Skip(1).ToList();
                }
            }
            else if (obj is SExpr.List sl && sl.Values.Count != 0)
            {
                List<SExpr> newList = sl.Values.Skip(1).ToList();
                // Self-evaluating nil
                return newList.Count == 0 ? null : new SExpr.List(newList);
            }
            else
            {
                throw new RuntimeException("Operand must be a list.");
            }
        }
    }
}
