namespace YispSharp.Data
{
    /// <summary>
    /// Stores all possible <see cref="Token"/> types.
    /// </summary>
    public enum TokenType
    {
        // Delimiters
        LeftParentheses,
        RightParentheses,
        SingleQuote,

        // Operations
        Plus,
        Minus,
        Star,
        Slash,
        Equal,
        LessThan,
        GreaterThan,

        // Literals/atoms
        Symbol,
        String,
        Number,

        Eof,
    }
}
