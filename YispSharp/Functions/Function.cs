using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions
{
    public class Function : ICallable
    {
        private readonly Stmt.Define _definition;
        
        public Function(Stmt.Define definition)
        {
            _definition = definition;
        }

        public Range Arity()
        {
            return new Range(_definition.Arguments.Count, _definition.Arguments.Count);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            throw new NotImplementedException();
        }
    }
}
