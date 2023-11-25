using System.Diagnostics;
using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Defines a function within the environment.
    /// </summary>
    public class Define : ICallable
    {
        public Range Arity()
        {
            return new Range(3, 3);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            // Stop definitions in nested code
            // FIXME: This feels hacky, is there a better solution?
            if (!new StackFrame(4).GetMethod().Name.Equals("Interpret"))
            {
                throw new RuntimeException("Cannot define a function in nested code.");
            }

            // Ensure name is symbol
            if (arguments[0] is SExpr.Atom a && a.Value is Token t && t.Type == TokenType.Symbol)
            {
                // Ensure we don't overwrite built-ins
                if (Interpreter.NativeFunctions.ContainsKey(t.Lexeme))
                {
                    throw new RuntimeException("Cannot overwrite built-in functions.");
                }

                // No arguments
                if (arguments[1] == null)
                {
                    interpreter.Environment.Define(t.Lexeme, new Function(new List<Token>(), arguments[2]));

                    // Return to eval loop
                    throw new StatementException();
                }
                // Some arguments
                else if (arguments[1] is SExpr.List l)
                {
                    List<Token> args = new();
                    foreach (SExpr s in l.Values)
                    {
                        // Ensure each argument is a symbol
                        if (s is SExpr.Atom aa && aa.Value is Token tt && tt.Type == TokenType.Symbol)
                        {
                            args.Add(tt);
                        }
                        else
                        {
                            throw new RuntimeException("Name of function argument must be a symbol.");
                        }
                    }
                    interpreter.Environment.Define(t.Lexeme, new Function(args, arguments[2]));

                    // Return to eval loop
                    throw new StatementException();
                }
                else
                {
                    throw new RuntimeException("Expected list of function arguments.");
                }
            }
            else
            {
                throw new RuntimeException("Name of function must be a symbol.");
            }
        }
    }
}
