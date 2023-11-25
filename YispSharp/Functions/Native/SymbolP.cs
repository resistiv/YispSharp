using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Checks if an object is a symbol.
    /// </summary>
    public class SymbolP : ICallable
    {
        public Range Arity()
        {
            return new Range(1, 1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            object obj = interpreter.Evaluate(arguments[0]);
            return (obj is SExpr.Atom a && a.Value is Token t && (t.Type == TokenType.Symbol || Parser.Operations.Contains(t.Type))) ? true : null;
        }
    }
}
