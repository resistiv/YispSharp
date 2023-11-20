using YispSharp.Data;
using YispSharp.Exceptions;

namespace YispSharp.Utils
{
    public class Interpreter : SExpr.IVisitor<object>
    {
        public void Interpret(List<SExpr> expr)
        {
            foreach (SExpr sexpr in expr)
            {
                Interpret(sexpr);
            }
        }

        private void Interpret(SExpr expr)
        {
            try
            {
                object value = Evaluate(expr);
                Console.WriteLine(Stringify(value));
            }
            catch (RuntimeException e)
            {
                Yisp.RuntimeError(e);
            }
        }

        private object Evaluate(SExpr expr)
        {
            return expr.Accept(this);
        }

        private void CheckNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double)
            {
                return;
            }
            else
            {
                throw new RuntimeException(op, "Operands must be numbers.");
            }
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null)
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null)
            { 
                return false;
            }
            else if (obj is bool)
            {
                return (bool)obj;
            }
            else
            {
                return true;
            }
        }

        private string Stringify(object obj)
        {
            if (obj == null)
            {
                return "()";
            }
            else if (obj is List<object> l)
            {
                string output = "(";
                bool lastIsNil = (l.Last() == null);

                for (int i = 0; i < l.Count; i++)
                {
                    bool isLast = (i == l.Count - 1);
                    if (isLast)
                    {
                        output += ". ";
                    }

                    output += l[i] == null ? "()" : l[i].ToString();

                    if (lastIsNil && i == l.Count - 2)
                    {
                        break;
                    }
                    else if (!isLast)
                    {
                        output += " ";
                    }
                }
                output += ")";
                return output;
            }
            else if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text[..^2];
                }
                return text;
            }
            else if (obj is bool)
            {
                if ((bool)obj)
                {
                    return "t";
                }
                else
                {
                    return "()";
                }
            }
            else
            {
                return obj.ToString();
            }
        }

        public object VisitAtomSExpr(SExpr.Atom expr)
        {
            return expr.Value;
        }

        public object VisitBinarySExpr(SExpr.Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Plus:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left + (double)right;
                case TokenType.Minus:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.Star:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.Slash:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.Equal:
                    return IsEqual(left, right);
                case TokenType.LessThan:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.GreaterThan:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.Cons:
                    if (right is not List<object>)
                    {
                        return new List<object>() { left, right };
                    }
                    else
                    {
                        return (right as List<object>).Prepend(left).ToList();
                    }
                case TokenType.AndP:
                    return IsTruthy(left) && IsTruthy(right);
                case TokenType.OrP:
                    return IsTruthy(left) || IsTruthy(right);
                case TokenType.EqP:
                    return ReferenceEquals(left, right);
                default:
                    return null;
            }
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
            if (expr.Values.Count == 0)
            {
                return null;
            }
            else
            {
                List<object> outVals = new();
                foreach (SExpr sexpr in expr.Values)
                {
                    outVals.Add(Evaluate(sexpr));
                }
                return outVals;
            }
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
                    return (right as List<object>)[0];
                case TokenType.Cdr:
                    return ((List<object>)right).Skip(1);
                case TokenType.NumberP:
                    return right is double;
                case TokenType.SymbolP:
                    return right is Token t && t.Type == TokenType.Symbol;
                case TokenType.NotP:
                    return !IsTruthy(right);
                case TokenType.ListP:
                    return right is List<object>;
                case TokenType.NilP:
                    return right == null ? null : true;
                default:
                    return null;
            }
        }
    }
}
