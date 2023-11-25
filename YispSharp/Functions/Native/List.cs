using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Handles list creation
    /// </summary>
    public class List : ICallable
    {
        public Range Arity()
        {
            return new Range(0, -1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            List<object> result = new();
            foreach (SExpr s in arguments)
            {
                result.Add(interpreter.Evaluate(s));
            }

            // Nil terminator for non-nil lists
            if (result.Count != 0)
            {
                result.Add(null);
            }

            return result.Count == 0 ? null : result;
        }
    }
}
