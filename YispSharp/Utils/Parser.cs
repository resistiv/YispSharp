using YispSharp.Data;
using YispSharp.Exceptions;

namespace YispSharp.Utils
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public static readonly TokenType[] Operations =
        {
            TokenType.Plus, TokenType.Minus,
            TokenType.Star, TokenType.Slash,
            TokenType.Equal, TokenType.SingleQuote,
            TokenType.GreaterThan, TokenType.LessThan,
        };


        /// <summary>
        /// Accepts a list of <see cref="Token"/>s to parse.
        /// </summary>
        /// <param name="tokens">A list of <see cref="Token"/>s to parse.</param>
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        /// <summary>
        /// Parses the previously provided <see cref="Token"/>s.
        /// </summary>
        /// <returns>A list of <see cref="SExpr"/>s, representing the parsed code.</returns>
        public List<SExpr> Parse()
        {
            List<SExpr> expressions = new();
            
            while (!AtEnd())
            {
                try
                {
                    expressions.Add(SExpression());
                }
                catch (ParsingException)
                {
                    SynchronizeState();
                }
            }

            return expressions;
        }

        /// <summary>
        /// Processes an s-expression.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Processes an atom.
        /// </summary>
        /// <returns></returns>
        private SExpr Atom()
        {
            if (MatchToken(TokenType.Number, TokenType.String))
            {
                return new SExpr.Atom(PreviousToken().Literal);
            }
            else if (MatchToken(TokenType.Symbol) || MatchToken(Operations))
            {
                return new SExpr.Atom(PreviousToken());
            }
            // FIXME: Remove once quote is implemented
            else if (MatchToken(TokenType.True))
            {
                return new SExpr.Atom(true);
            }

            throw Error(Peek(), "Expected atom.");
        }

        /// <summary>
        /// Processes a list.
        /// </summary>
        /// <returns></returns>
        private SExpr List()
        {
            // Read all values of list
            List<SExpr> values = new();
            while (!MatchToken(TokenType.RightParentheses))
            {
                values.Add(SExpression());
            }

            return new SExpr.List(values);
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


        /// <summary>
        /// Attempts to synchronize the state of the parser after encountering a <see cref="ParsingException"/>.
        /// </summary>
        private void SynchronizeState()
        {
            NextToken();

            while (!AtEnd())
            {
                if (PreviousToken().Type == TokenType.RightParentheses)
                {
                    return;
                }

                switch (Peek().Type)
                {
                    case TokenType.LeftParentheses:
                        return;
                }

                NextToken();
            }
        }
    }
}
