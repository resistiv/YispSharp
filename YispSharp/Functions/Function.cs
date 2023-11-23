using YispSharp.Data;
using YispSharp.Utils;
using Environment = YispSharp.Data.Environment;

namespace YispSharp.Functions
{
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
            Environment env = new(interpreter.Globals);
            for (int i = 0; i < Arguments.Count; i++)
            {
                env.Define(Arguments[i].Lexeme, interpreter.Evaluate(arguments[i]));
            }

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
