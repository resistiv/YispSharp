using YispSharp.Data;

namespace YispSharp.Utils
{
    /// <summary>
    /// Provides utilities for scanning raw Yisp code into <see cref="Token"/>s.
    /// </summary>
    public class Scanner
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new();
        private int _startIndex = 0;
        private int _currentIndex = 0;
        private int _line = 1;

        /// <summary>
        /// Constructs a <see cref="Scanner"/>.
        /// </summary>
        /// <param name="source">A piece of raw Yisp code to scan and parse.</param>
        public Scanner(string source)
        {
            _source = source;
        }

        /// <summary>
        /// Scans all <see cref="Token"/>s from the <see cref="Scanner"/>'s source <see cref="string"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="Token"/>s parsed from the source <see cref="string"/>.</returns>
        public List<Token> ScanTokens()
        {
            while (!AtEnd())
            {
                _startIndex = _currentIndex;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.Eof, "", null, _line));
            return _tokens;
        }

        /// <summary>
        /// Scans an individual <see cref="Token"/> and adds it to the <see cref="Token"/> <see cref="List{T}"/>.
        /// </summary>
        private void ScanToken()
        {
            char c = NextCharacter();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LeftParentheses);
                    break;
                case ')':
                    AddToken(TokenType.RightParentheses);
                    break;
                case '+':
                    AddToken(TokenType.Plus);
                    break;
                case '-':
                    AddToken(TokenType.Minus);
                    break;
                case '*':
                    AddToken(TokenType.Star);
                    break;
                case '/':
                    AddToken(TokenType.Slash);
                    break;
                case '=':
                    AddToken(TokenType.Equal);
                    break;
                case '<':
                    AddToken(TokenType.LessThan);
                    break;
                case '>':
                    AddToken(TokenType.GreaterThan);
                    break;
                case '\'':
                    AddToken(TokenType.SingleQuote);
                    break;
                case ';':
                    while (Peek() != '\n' && !AtEnd())
                    {
                        NextCharacter();
                    }
                    break;

                // Whitespace
                case ' ':
                case '\r':
                case '\t':
                    break;

                case '\n':
                    _line++;
                    break;

                case '"':
                    ReadString();
                    break;

                default:
                    if (IsDigit(c))
                    {
                        ReadNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ReadSymbol();
                    }
                    else
                    {
                        Yisp.Error(_line, "Unexpected character.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns whether or not the current index is at the end of the source <see cref="string"/>.
        /// </summary>
        /// <returns>Whether or not the current index is at the end of the source <see cref="string"/>.</returns>
        private bool AtEnd()
        {
            return _currentIndex >= _source.Length;
        }

        /// <summary>
        /// Returns the next character in the source <see cref="string"/> and increments the current index.
        /// </summary>
        /// <returns>The next character in the source <see cref="string"/>.</returns>
        private char NextCharacter()
        {
            return _source[_currentIndex++];
        }

        /// <summary>
        /// Adds a <see cref="Token"/> to the <see cref="Token"/> <see cref="List{T}"/> given only a <see cref="TokenType"/>.
        /// </summary>
        /// <param name="type">A <see cref="TokenType"/> from which to generate and add a <see cref="Token"/>.</param>
        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        /// <summary>
        /// Adds a <see cref="Token"/> to the <see cref="Token"/> <see cref="List{T}"/>.
        /// </summary>
        /// <param name="type">A <see cref="TokenType"/> from which to generate and add a <see cref="Token"/>.</param>
        /// <param name="literal">The literal <see cref="object"/> of the <see cref="Token"/>.</param>
        private void AddToken(TokenType type, object literal)
        {
            string lexeme = _source[_startIndex.._currentIndex];
            _tokens.Add(new Token(type, lexeme, literal, _line));
        }

        /// <summary>
        /// Checks if a character is an ASCII digit.
        /// </summary>
        /// <param name="c">A character to check.</param>
        /// <returns>Whether or not the character is an ASCII digit.</returns>
        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Checks if a character is an ASCII alphabetical character.
        /// </summary>
        /// <param name="c">A character to check.</param>
        /// <returns>Whether or not the character is an ASCII alphabetical character.</returns>
        private static bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '-' || c == '?';
        }

        /// <summary>
        /// Checks if a character is an ASCII alpha-numeric character.
        /// </summary>
        /// <param name="c">A character to check.</param>
        /// <returns>Whether or not the character is an ASCII alpha-numeric character.</returns>
        private static bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        /// <summary>
        /// Peeks ahead at the next <see cref="char"/> in the source <see cref="string"/>, without incrementing the current index.
        /// </summary>
        /// <returns>The next <see cref="char"/> in the source <see cref="string"/>.</returns>
        private char Peek()
        {
            if (AtEnd())
            {
                return '\0';
            }
            return _source[_currentIndex];
        }

        /// <summary>
        /// Peeks ahead at the second <see cref="char"/> from the current one.
        /// </summary>
        /// <returns>The second <see cref="char"/> from the current one.</returns>
        private char PeekNext()
        {
            if (_currentIndex + 1 >= _source.Length)
            {
                return '\0';
            }
            return _source[_currentIndex + 1];
        }

        /// <summary>
        /// Reads a number from the source <see cref="string"/>.
        /// </summary>
        private void ReadNumber()
        {
            // Read digits
            while (IsDigit(Peek()))
            {
                NextCharacter();
            }

            // If there is a decimal point, read everything to right of it
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                NextCharacter();
                while (IsDigit(Peek()))
                {
                    NextCharacter();
                }
            }

            // Parse out
            double value = double.Parse(_source[_startIndex.._currentIndex]);
            AddToken(TokenType.Number, value);
        }

        /// <summary>
        /// Reads a <see cref="string"/> from the source <see cref="string"/>.
        /// </summary>
        private void ReadString()
        {
            // Read either until we reach closing quote or EOF
            while (Peek() != '"' && !AtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                }
                NextCharacter();
            }

            // We ran out of string to read!
            if (AtEnd())
            {
                Yisp.Error(_line, "Unterminated string.");
                return;
            }

            // Consume closing double quote
            NextCharacter();

            // Parse out string
            string value = _source[(_startIndex + 1)..(_currentIndex - 1)];
            AddToken(TokenType.String, value);
        }

        /// <summary>
        /// Parses keywords and symbols.
        /// </summary>
        private void ReadSymbol()
        {
            while (IsAlphaNumeric(Peek()))
            {
                NextCharacter();
            }

            string text = _source[_startIndex.._currentIndex];
            AddToken(TokenType.Symbol, text);
        }
    }
}
