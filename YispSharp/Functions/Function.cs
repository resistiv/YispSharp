using YispSharp.Data;
using YispSharp.Utils;

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
            throw new NotImplementedException();
        }
    }
}
