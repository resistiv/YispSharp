namespace YispSharp.Data
{
    public abstract class SExpr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);

        public interface IVisitor<T>
        {
            public T VisitAtomSExpr(Atom expr);
            public T VisitListSExpr(List expr);
            public T VisitBinarySExpr(Binary expr);
            public T VisitUnarySExpr(Unary expr);
            public T VisitDefineSExpr(Define expr);
        }

        /// <summary>
        /// Represents a single unit of data.
        /// </summary>
        public class Atom : SExpr
        {
            public readonly object Value;

            public Atom(object value)
            {
                Value = value;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitAtomSExpr(this);
            }
        }

        /// <summary>
        /// Represents a variable list of data.
        /// </summary>
        public class List : SExpr
        {
            public readonly List<SExpr> Values;

            public List(List<SExpr> values)
            {
                Values = values;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitListSExpr(this);
            }
        }

        /// <summary>
        /// Represents an expression with two arguments.
        /// </summary>
        public class Binary : SExpr
        {
            public readonly Token Operator;
            public readonly SExpr Left;
            public readonly SExpr Right;

            public Binary(Token @operator, SExpr left, SExpr right)
            {
                Operator = @operator;
                Left = left;
                Right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitBinarySExpr(this);
            }
        }

        /// <summary>
        /// Represents an expression with one argument.
        /// </summary>
        public class Unary : SExpr
        {
            public readonly Token Operator;
            public readonly SExpr Right;

            public Unary(Token @operator, SExpr right)
            {
                Operator = @operator;
                Right = right;
            }

            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.VisitUnarySExpr(this);
            }
        }

        /// <summary>
        /// Represents a define statement.
        /// </summary>
        public class Define : SExpr
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
                return visitor.VisitDefineSExpr(this);
            }
        }
    }
}
