using System.Diagnostics;
using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Assigns a value to an environment variable.
    /// </summary>
    public class Set : ICallable
    {
        public Range Arity()
        {
            return new Range(2, 2);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            // Stop sets in nested code
            // FIXME: This feels hacky, is there a better solution?
            if (!new StackFrame(4).GetMethod().Name.Equals("Interpret"))
            {
                throw new RuntimeException("Cannot set an object in nested code.");
            }

            if (arguments[0] is SExpr.Atom a && a.Value is Token t && t.Type == TokenType.Symbol)
            {
                // Ensure we don't overwrite built-ins
                if (Interpreter.NativeFunctions.ContainsKey(t.Lexeme))
                {
                    throw new RuntimeException("Cannot overwrite built-in functions.");
                }

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
