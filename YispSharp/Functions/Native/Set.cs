using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    public class Set : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            if (arguments[0] is SExpr.Atom a && a.Value is Token t && t.Type == TokenType.Symbol)
            {
                object value = interpreter.Evaluate(arguments[1]);
                interpreter.Environment.Define(t.Lexeme, value);
                throw new StatementException();
            }
            else
            {
                throw new RuntimeException("Name of set object must be a symbol.");
            }
        }
    }
}
