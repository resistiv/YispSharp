using YispSharp.Data;
using YispSharp.Exceptions;
using Environment = YispSharp.Data.Environment;

namespace YispSharp.Utils
{
    public class Interpreter : SExpr.IVisitor<object>, Stmt.IVisitor<object>
    {
        private Environment _environment = new();

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
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

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
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
            // Specified in Instructions.md: "Returns () when either expression is a larger list."
            else if (a is List<object> || b is List<object>)
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }

        private bool IsSymbol(object obj)
        {
            if (obj is Token t)
            {
                if (t.Type == TokenType.Symbol || Scanner.Keywords.ContainsValue(t.Type))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null)
            { 
                return false;
            }
            else if (obj is bool b)
            {
                return b;
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

                // Traverse list elements
                for (int i = 0; i < l.Count; i++)
                {
                    // If the we're at the last element, we have to be non-nil, so output a dot to indicate
                    bool isLast = (i == l.Count - 1);
                    if (isLast)
                    {
                        output += ". ";
                    }

                    // Figure out the output type
                    if (l[i] == null)
                    {
                        output += "()";
                    }
                    else if (l[i] is List<object>)
                    {
                        // Recurse on lists
                        output += Stringify(l[i]);
                    }
                    else
                    {
                        output += l[i].ToString();
                    }

                    // If we're at the second-to-last element and the last element is nil, we're done
                    if (lastIsNil && i == l.Count - 2)
                    {
                        break;
                    }
                    // Otherwise, print separator
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
            else if (obj is bool b)
            {
                if (b)
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
                // No clue what we've encountered, leave it up to god
                return obj.ToString();
            }
        }

        public object VisitAtomSExpr(SExpr.Atom expr)
        {
            if (expr.Value is Token t && t.Type == TokenType.Symbol)
            {
                return _environment.Get(t);
            }
            else
            {
                return expr.Value;
            }
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
                    return IsEqual(left, right) ? true : null; ;
                case TokenType.LessThan:
                    CheckNumberOperands(expr.Operator, left, right);
                    return ((double)left < (double)right) ? true : null;
                case TokenType.GreaterThan:
                    CheckNumberOperands(expr.Operator, left, right);
                    return ((double)left > (double)right) ? true : null;
                case TokenType.Cons:
                    if (right is List<object> consList)
                    {
                        return consList.Prepend(left).ToList();
                    }
                    else
                    {
                        return new List<object>() { left, right };
                    }
                case TokenType.AndP:
                    return (IsTruthy(left) && IsTruthy(right)) ? true : null;
                case TokenType.OrP:
                    return (IsTruthy(left) || IsTruthy(right)) ? true : null;
                // FIXME: How is this implemented in other interpreters?
                // From what I can tell, the only meaningful check this can make is for symbols in Yisp...
                case TokenType.EqP:
                    /*if (left is Token l && right is Token r && l.Type == TokenType.Symbol && l.Type == r.Type && l.Lexeme.Equals(r.Lexeme))
                    {
                        return true;
                    }
                    else
                    {
                        return null;
                    }*/
                    return ReferenceEquals(left, right) ? true : null;
                default:
                    return null;
            }
        }

        public object VisitCondSExpr(SExpr.Cond expr)
        {
            foreach (Tuple<SExpr, SExpr> pair in expr.Conditions)
            {
                object cond = Evaluate(pair.Item1);
                if (IsTruthy(cond))
                {
                    return Evaluate(pair.Item2);
                }
            }
            throw new RuntimeException(expr.Operator, "No condition in cond evaluated to true.");
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

        public object VisitUnarySExpr(SExpr.Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Car:
                    if (right is List<object> carList)
                    {
                        return carList[0];
                    }
                    else
                    {
                        throw new RuntimeException(expr.Operator, "Operand must be a list.");
                    }
                case TokenType.Cdr:
                    if (right is List<object> cdrList)
                    {
                        if (cdrList.Count == 2)
                        {
                            return cdrList[1];
                        }
                        else
                        {
                            return cdrList.Skip(1).ToList();
                        }
                    }
                    else
                    {
                        throw new RuntimeException(expr.Operator, "Operand must be a list.");
                    }
                case TokenType.NumberP:
                    return right is double;
                case TokenType.SymbolP:
                    return IsSymbol(right) ? true : null;
                case TokenType.NotP:
                    return !IsTruthy(right) ? true : null;
                case TokenType.ListP:
                    return right is List<object> || right == null ? true : null;
                case TokenType.NilP:
                    return right == null ? true : null;
                default:
                    return null;
            }
        }

        public object VisitSExpressionStmt(Stmt.SExpression stmt)
        {
            object result = Evaluate(stmt.Expression);
            Console.WriteLine(Stringify(result));
            return null;
        }

        public object VisitDefineStmt(Stmt.Define stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitSetStmt(Stmt.Set stmt)
        {
            object value = Evaluate(stmt.Value);
            _environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object VisitCallSExpr(SExpr.Call expr)
        {
            throw new NotImplementedException();
        }
    }
}
