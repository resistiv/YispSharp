using YispSharp.Data;
using YispSharp.Utils;
using Environment = YispSharp.Data.Environment;

namespace YispSharp.Functions
{
    /// <summary>
    /// Represents a user-defined function.
    /// </summary>
    public class Function : ICallable
    {
        private readonly List<Token> Arguments;
        private readonly SExpr Body;
        
        public Function(List<Token> args, SExpr body)
        {
            Arguments = args;
            Body = body;
        }

        public Range Arity()
        {
            return new Range(Arguments.Count, Arguments.Count);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            // First, define arguments as local variables in a new environment
            Environment env = new(interpreter.Globals);
            for (int i = 0; i < Arguments.Count; i++)
            {
                env.Define(Arguments[i].Lexeme, interpreter.Evaluate(arguments[i]));
            }

            // Swap environments within interpreter and evaluate
            Environment prev = interpreter.Environment;
            object result = null;
            try
            {
                interpreter.Environment = env;
                result = interpreter.Evaluate(Body);
            }
            finally
            {
                interpreter.Environment = prev;
            }

            return result;
        }
    }
}
