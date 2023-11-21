/*using System.Text;
using YispSharp.Data;

namespace YispSharp.Utils
{
    public class AstPrinter : SExpr.IVisitor<string>
    {
        public void Print(List<SExpr> exprs)
        {
            foreach (SExpr expr in exprs)
            {
                Console.WriteLine(expr.Accept(this));
            }
        }

        public string Parenthesize(string name, params SExpr[] sexprs)
        {
            StringBuilder sb = new();
            sb.Append('(').Append(name);
            foreach (SExpr sexpr in sexprs)
            {
                sb.Append(' ');
                sb.Append(sexpr.Accept(this));
            }
            sb.Append(')');

            return sb.ToString();
        }

        public string VisitAtomSExpr(SExpr.Atom expr)
        {
            if (expr.Value == null)
            {
                return "nil";
            }
            else
            {
                return expr.Value.ToString();
            }
        }

        public string VisitBinarySExpr(SExpr.Binary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
        }

        public string VisitCondSExpr(SExpr.Cond expr)
        {
            return Parenthesize("cond", expr.Conditions.ToArray());
        }

        public string VisitDefineSExpr(SExpr.Define expr)
        {
            return Parenthesize("define", new SExpr.Atom(expr.Name.Lexeme), expr.Arguments, expr.Body);
        }

        public string VisitListSExpr(SExpr.List expr)
        {
            return Parenthesize("list", expr.Values.ToArray());
        }

        public string VisitSetSExpr(SExpr.Set expr)
        {
            return Parenthesize("set", new SExpr.Atom(expr.Name.Lexeme), expr.Value);
        }

        public string VisitUnarySExpr(SExpr.Unary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Right);
        }
    }
}*/
