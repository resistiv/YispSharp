﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace YispSharp.Data
{
    public abstract class SExpr
    {
        public abstract T Accept<T>(IVisitor<T> visitor);

        public interface IVisitor<T>
        {
            public T VisitAtomSExpr(Atom expr);
            public T VisitBinarySExpr(Binary expr);
            public T VisitUnarySExpr(Unary expr);
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
    }
}
