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
        True, // FIXME: This is here temporarily, until quote is properly implemented for 't

        Eof,
    }
}
