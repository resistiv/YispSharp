namespace YispSharp.Data
{
    /// <summary>
    /// Represents a lexical token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The type of this <see cref="Token"/>.
        /// </summary>
        public readonly TokenType Type;
        /// <summary>
        /// The source <see cref="string"/> of this <see cref="Token"/>.
        /// </summary>
        public readonly string Lexeme;
        /// <summary>
        /// The literal <see cref="object"/> of this <see cref="Token"/>.
        /// </summary>
        public readonly object Literal;
        /// <summary>
        /// The line number that this <see cref="Token"/> originates from.
        /// </summary>
        public readonly int Line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public override string ToString()
        {
            return $"{Type} {Lexeme ?? "null"} {Literal ?? "null"}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Token t)
            {
                return (Type == t.Type) && Lexeme.Equals(t.Lexeme) && Literal.Equals(t.Literal) && (Line == t.Line);
            }
            else
            {
                return false;
            }
        }
    }
}
