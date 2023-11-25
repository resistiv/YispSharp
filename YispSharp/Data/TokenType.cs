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

        // Functions
        Define,
        Set,
        List,
        Cons,
        Cond,
        Car,
        Cdr,
        AndP,
        OrP,
        NotP,
        NumberP,
        SymbolP,
        ListP,
        NilP,
        EqP,

        // Literals/atoms
        Symbol,
        String,
        Number,
        Nil,

        Eof,
    }
}
