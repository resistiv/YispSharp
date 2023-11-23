using YispSharp.Data;
using YispSharp.Exceptions;

namespace YispSharp.Utils
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        private readonly TokenType[] _binaryTypes = { TokenType.Plus, TokenType.Minus, TokenType.Star, TokenType.Slash, TokenType.Equal, TokenType.LessThan, TokenType.GreaterThan, TokenType.Cons, TokenType.AndP, TokenType.OrP, TokenType.EqP };
        private readonly TokenType[] _unaryTypes = { TokenType.Car, TokenType.Cdr, TokenType.NumberP, TokenType.SymbolP, TokenType.NotP, TokenType.ListP, TokenType.NilP };

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new();
            
            while (!AtEnd())
            {
                try
                {
                    statements.Add(Statement());
                }
                catch (ParsingException)
                {
                    SyncState();
                }
            }

            return statements;
        }

        private Stmt Statement()
        {
            if (MatchToken(TokenType.LeftParentheses))
            {
                // Define statement
                if (MatchToken(TokenType.Define))
                {
                    return Define();
                }
                // Set statement
                else if (MatchToken(TokenType.Set))
                {
                    return Set();
                }
                // Else, must be some sort of list expression
                else
                {
                    return new Stmt.SExpression(List());
                }
            }
            // Must be an atom
            else
            {
                return new Stmt.SExpression(Atom());
            }
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
            if (MatchToken(TokenType.Number, TokenType.String))
            {
                return new SExpr.Atom(PreviousToken().Literal);
            }
            else if (MatchToken(TokenType.Symbol))
            {
                return new SExpr.Atom(PreviousToken());
            }
            else if (MatchToken(TokenType.True))
            {
                return new SExpr.Atom(true);
            }

            throw Error(Peek(), "Expected atom.");
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
            // Cond control flow statement
            else if (MatchToken(TokenType.Cond))
            {
                return Cond();
            }
            // Reject any set or definition occuring in nested code
            else if (MatchToken(TokenType.Set, TokenType.Define))
            {
                throw Error(PreviousToken(), "Define & set are not allowed outside of top-level statements.");
            }
            // Raw list
            else if (MatchToken(TokenType.List))
            {
                return PlainList();
            }
            // Nil
            else if (MatchToken(TokenType.RightParentheses))
            {
                return new SExpr.List(new List<SExpr>());
            }
            // Function call
            else
            {
                return Call();
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

            // Add nil terminator
            if (values.Count != 0)
            {
                values.Add(new SExpr.List(new List<SExpr>()));
            }
            
            return new SExpr.List(values);
        }

        private SExpr Binary()
        {
            Token @operator = PreviousToken();
            SExpr left = SExpression();
            SExpr right = SExpression();
            ConsumeToken(TokenType.RightParentheses, "Expected closing parentheses in binary expression.");
            return new SExpr.Binary(@operator, left, right);
        }

        private SExpr Unary()
        {
            Token @operator = PreviousToken();
            SExpr right = SExpression();
            ConsumeToken(TokenType.RightParentheses, "Expected closing parentheses in unary expression.");
            return new SExpr.Unary(@operator, right);
        }

        private Stmt Define()
        {
            Token name = ConsumeToken(TokenType.Symbol, "Expected name in function definition.");

            // Read argument names
            ConsumeToken(TokenType.LeftParentheses, "Expected list of arguments in function definition.");
            List<Token> args = new();
            while (!MatchToken(TokenType.RightParentheses))
            {
                args.Add(ConsumeToken(TokenType.Symbol, "Expected symbol in argument list in function definition."));
            }

            SExpr body = SExpression();

            ConsumeToken(TokenType.RightParentheses, "Expected closing parentheses in function definition.");

            return new Stmt.Define(name, args, body);
        }

        private Stmt Set()
        {
            Token name = ConsumeToken(TokenType.Symbol, "Expected name in variable definition.");
            SExpr value = SExpression();
            ConsumeToken(TokenType.RightParentheses, "Expected closing parentheses in variable definition.");
            return new Stmt.Set(name, value);
        }

        private SExpr Cond()
        {
            Token op = PreviousToken();

            // Read conditions and results
            List<Tuple<SExpr, SExpr>> condPairs = new();
            while (!MatchToken(TokenType.RightParentheses))
            {
                Tuple<SExpr, SExpr> pair = new(SExpression(), SExpression());
                condPairs.Add(pair);
            }

            return new SExpr.Cond(op, condPairs);
        }

        private SExpr Call()
        {
            Token funcName = ConsumeToken(TokenType.Symbol, "Expected symbol in function call.");

            List<SExpr> args = new();
            while (!MatchToken(TokenType.RightParentheses))
            {
                args.Add(SExpression());
            }

            return new SExpr.Call(funcName, args);
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

        private void SyncState()
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
