namespace YispSharp.Data
{
    public enum TokenType
    {
        // Delimiters
        LeftParentheses,
        RightParentheses,
        SingleQuote,
        //DoubleQuote,
        //Semicolon,

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
