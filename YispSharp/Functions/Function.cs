using YispSharp.Data;
using YispSharp.Utils;
using Environment = YispSharp.Data.Environment;

namespace YispSharp.Functions
{
    public class Function : ICallable
    {
        private readonly Stmt.Define _definition;
        
        public Function(Stmt.Define definition)
        {
            _definition = definition;
        }

        public int Arity()
        {
            return _definition.Arguments.Count;
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            Environment env = new(interpreter.Globals);
            for (int i = 0; i < Arity(); i++)
            {
                env.Define(_definition.Arguments[i].Lexeme, arguments[i]);
            }
            interpreter.ExecuteFunction(_definition.Body, env);
            return null;
        }
    }
}
