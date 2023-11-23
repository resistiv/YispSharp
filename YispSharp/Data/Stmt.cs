namespace YispSharp.Data
{
    public abstract class Stmt
    {
        public abstract T Accept<T>(IVisitor<T> visitor);

        public interface IVisitor<T>
        {
            public T VisitSExpressionStmt(SExpression stmt);
            public T VisitDefineStmt(Define stmt);
            public T VisitSetStmt(Set stmt);
        }

        /// <summary>
        /// Represents an s-expression.
        /// </summary>
        public class SExpression : Stmt
        {
            public readonly SExpr Expression;

            public SExpression(SExpr expression)
            {
                Expression = expression;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitSExpressionStmt(this);
            }
        }

        /// <summary>
        /// Represents a define statement.
        /// </summary>
        public class Define : Stmt
        {
            public readonly Token Name;
            public readonly List<Token> Arguments;
            public readonly SExpr Body;

            public Define(Token name, List<Token> arguments, SExpr body)
            {
                Name = name;
                Arguments = arguments;
                Body = body;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitDefineStmt(this);
            }
        }

        /// <summary>
        /// Represents a set statement.
        /// </summary>
        public class Set : Stmt
        {
            public readonly Token Name;
            public readonly SExpr Value;

            public Set(Token name, SExpr value)
            {
                Name = name;
                Value = value;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitSetStmt(this);
            }
        }
    }
}
