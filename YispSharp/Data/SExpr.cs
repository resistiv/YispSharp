namespace YispSharp.Data
{
    public abstract class SExpr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);

        public interface IVisitor<T>
        {
            public T VisitAtomSExpr(Atom expr);
            public T VisitListSExpr(List expr);
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
    }
}
