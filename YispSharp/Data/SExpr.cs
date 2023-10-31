using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YispSharp.Data
{
    public abstract class SExpr
    {
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
        }
    }
}
