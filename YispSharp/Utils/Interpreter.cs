using YispSharp.Data;

namespace YispSharp.Utils
{
    public class Interpreter : SExpr.IVisitor<object>
    {
        private object Evaluate(SExpr expr)
        {
            return expr.Accept(this);
        }

        public object VisitAtomSExpr(SExpr.Atom expr)
        {
            return expr.Value;
        }

        public object VisitBinarySExpr(SExpr.Binary expr)
        {
            throw new NotImplementedException();
        }

        public object VisitCondSExpr(SExpr.Cond expr)
        {
            throw new NotImplementedException();
        }

        public object VisitDefineSExpr(SExpr.Define expr)
        {
            throw new NotImplementedException();
        }

        public object VisitListSExpr(SExpr.List expr)
        {
            return expr.Values.Count == 0 ? null : expr.Values;
        }

        public object VisitSetSExpr(SExpr.Set expr)
        {
            throw new NotImplementedException();
        }

        public object VisitUnarySExpr(SExpr.Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Car:
                    return ((List<SExpr>)right)[0];
                case TokenType.Cdr:
                    return ((List<SExpr>)right).Skip(1);
                case TokenType.NumberP:
                case TokenType.SymbolP:
                case TokenType.NotP:
                case TokenType.ListP:
                case TokenType.NilP:
                    return right == null ? null : true;
            }
        }
    }
}
