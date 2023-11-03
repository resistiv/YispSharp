using YispSharp.Data;
using YispSharp.Exceptions;

namespace YispSharp.Utils
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        private readonly TokenType[] _binaryTypes = { TokenType.Plus, TokenType.Minus, TokenType.Star, TokenType.Slash, TokenType.Equal, TokenType.LessThan, TokenType.GreaterThan };
        private readonly TokenType[] _unaryTypes = { TokenType.Car, TokenType.Cdr, TokenType.NumberP, TokenType.SymbolP, TokenType.NotP, TokenType.ListP, TokenType.NilP };

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private SExpr SExpression()
        {
            if (MatchToken(TokenType.LeftParentheses))
            {
                return List();
            }
            else
            {
                return Atom();
            }
        }

        private SExpr Atom()
        {
            if (MatchToken(TokenType.Number, TokenType.String, TokenType.Symbol))
            {
                return new SExpr.Atom(PreviousToken().Literal);
            }

            Error(Peek(), "Expected atom.");
            return null;
        }

        private SExpr List()
        {
            // Binary operations
            if (MatchToken(_binaryTypes))
            {
                return Binary();
            }
            // Unary operations
            else if (MatchToken(_unaryTypes))
            {
                return Unary();
            }
            else if (MatchToken(TokenType.Define))
            {
                return Define();
            }
            // Raw list
            else
            {
                return PlainList();
            }
        }

        private SExpr PlainList()
        {
            // Read all values of list
            List<SExpr> values = new();
            while (!MatchToken(TokenType.RightParentheses))
            {
                values.Add(SExpression());
            }

            // No content, nil!
            if (values.Count == 0)
            {
                return new SExpr.Atom(null);
            }
            else
            {
                return new SExpr.List(values);
            }
        }

        private SExpr Binary()
        {
            Token @operator = PreviousToken();
            SExpr left = SExpression();
            SExpr right = SExpression();
            return new SExpr.Binary(@operator, left, right);
        }

        private SExpr Unary()
        {
            Token @operator = PreviousToken();
            SExpr right = SExpression();
            return new SExpr.Unary(@operator, right);
        }

        private SExpr Define()
        {
            Token name = ConsumeToken(TokenType.Symbol, "Expected name in function definition.");

            // Read argument names
            List<Token> args = new();
            ConsumeToken(TokenType.LeftParentheses, "Expected argument list in function definition.");
            while (!MatchToken(TokenType.RightParentheses))
            {
                args.Add(ConsumeToken(TokenType.Symbol, "Expected symbol in function definition argument list."));
            }

            SExpr body = SExpression();

            return new SExpr.Define(name, args, body);
        }

        /// <summary>
        /// Checks if the next <see cref="Token"/> matches a list of <see cref="TokenType"/>s.
        /// </summary>
        /// <param name="types">A list of <see cref="TokenType"/>s.</param>
        /// <returns>Whether or not the next <see cref="Token"/> matches any given <see cref="TokenType"/> provided.</returns>
        private bool MatchToken(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (CheckToken(type))
                {
                    NextToken();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to consume a <see cref="Token"/>.
        /// </summary>
        /// <param name="type">The expected <see cref="TokenType"/> of the <see cref="Token"/>.</param>
        /// <param name="message">A message detailing the expected <see cref="Token"/>.</param>
        /// <exception cref="ParsingException"></exception>
        /// <returns>A <see cref="Token"/> that was consumed.</returns>
        private Token ConsumeToken(TokenType type, string message)
        {
            if (CheckToken(type))
            {
                return NextToken();
            }

            throw Error(Peek(), message);
        }

        /// <summary>
        /// Returns true if the given <see cref="TokenType"/> matches that of the next <see cref="Token"/>.
        /// </summary>
        /// <param name="type">A <see cref="TokenType"/> to match.</param>
        /// <returns>Whether or not the given <see cref="TokenType"/> matches that of the next <see cref="Token"/>.</returns>
        private bool CheckToken(TokenType type)
        {
            if (AtEnd())
            {
                return false;
            }
            return Peek().Type == type;
        }

        /// <summary>
        /// Consumes the next <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/>.</returns>
        private Token NextToken()
        {
            if (!AtEnd())
            {
                _current++;
            }
            return PreviousToken();
        }

        /// <summary>
        /// Returns whether or not the end of the <see cref="Token"/> list has been reached.
        /// </summary>
        /// <returns>Whether or not the end of the <see cref="Token"/> list has been reached.</returns>
        private bool AtEnd()
        {
            return Peek().Type == TokenType.Eof;
        }

        /// <summary>
        /// Peeks at the next <see cref="Token"/> without advancing.
        /// </summary>
        /// <returns>The next <see cref="Token"/>.</returns>
        private Token Peek()
        {
            return _tokens[_current];
        }

        /// <summary>
        /// Returns the previous <see cref="Token"/>.
        /// </summary>
        /// <returns>The previous <see cref="Token"/>.</returns>
        private Token PreviousToken()
        {
            return _tokens[_current - 1];
        }

        /// <summary>
        /// Reports and returns a <see cref="ParsingException"/>.
        /// </summary>
        /// <param name="token">A <see cref="Token"/> that caused the error.</param>
        /// <param name="message">A message detailing the error.</param>
        /// <returns>A <see cref="ParsingException"/>.</returns>
        private ParsingException Error(Token token, string message)
        {
            Yisp.Error(token, message);
            return new ParsingException();
        }
    }
}
