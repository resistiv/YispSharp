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
            // Self-evaluating nil
            if (arguments[0] is SExpr.List sl && sl.Values.Count == 0)
            {
                return null;
            }
            // Basic forms are self-evaluating
            else if (arguments[0] is SExpr.Atom a)
            {
                if (a.Value is double d)
                {
                    return d;
                }
                else if (a.Value is string s)
                {
                    return s;
                }
                else
                {
                    return arguments[0];
                }
            }
            else
            {
                return arguments[0];
            }
        }
    }
}
