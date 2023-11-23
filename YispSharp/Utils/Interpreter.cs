using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Functions;
using YispSharp.Functions.Native;
using Environment = YispSharp.Data.Environment;

namespace YispSharp.Utils
{
    public class Interpreter : SExpr.IVisitor<object>
    {
        public readonly Environment Globals = new();
        private Environment _environment;

        private static readonly Dictionary<string, ICallable> _nativeFunctions = new()
        {
            { "+", new Addition() },
            { "-", new Subtraction() },
            { "*", new Multiplication() },
            { "/", new Division() },
            { "list", new Functions.Native.List() },
        };

        public Interpreter()
        {
            _environment = Globals;
            foreach (KeyValuePair<string, ICallable> kvp in _nativeFunctions)
            {
                Globals.Define(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Interprets a list of <see cref="SExpr"/>s.
        /// </summary>
        /// <param name="expressions">A list of <see cref="SExpr"/>s to interpret.</param>
        public void Interpret(List<SExpr> expressions)
        {
            try
            {
                foreach (SExpr expr in expressions)
                {
                    try
                    {
                        Console.WriteLine(Stringify(Evaluate(expr)));
                    }
                    catch (StatementException) { }
                }
            }
            catch (RuntimeException e)
            {
                Yisp.RuntimeError(e);
            }
        }

        /// <summary>
        /// Converts interpreter output to human-readable output.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Stringify(object obj)
        {
            if (obj == null)
            {
                return "()";
            }
            else if (obj is List<object> l)
            {
                string output = "(";
                bool lastIsNil = (l.Last() == null);

                // Traverse list elements
                for (int i = 0; i < l.Count; i++)
                {
                    // If the we're at the last element, we have to be non-nil, so output a dot to indicate
                    bool isLast = (i == l.Count - 1);
                    if (isLast)
                    {
                        output += ". ";
                    }

                    // Figure out the output type
                    if (l[i] == null)
                    {
                        output += "()";
                    }
                    else if (l[i] is List<object>)
                    {
                        // Recurse on lists
                        output += Stringify(l[i]);
                    }
                    else
                    {
                        output += l[i].ToString();
                    }

                    // If we're at the second-to-last element and the last element is nil, we're done
                    if (lastIsNil && i == l.Count - 2)
                    {
                        break;
                    }
                    // Otherwise, print separator
                    else if (!isLast)
                    {
                        output += " ";
                    }
                }
                output += ")";
                return output;
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// Evaluates an expression.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object Evaluate(SExpr expr) 
        {
            return expr.Accept(this);
        }

        public object VisitAtomSExpr(SExpr.Atom expr)
        {
            if (expr.Value is Token t && (t.Type == TokenType.Symbol || Parser.Operations.Contains(t.Type)))
            {
                return _environment.Get(t);
            }
            else
            {
                return expr.Value;
            }
        }

        public object VisitListSExpr(SExpr.List expr)
        {
            // Nil
            if (expr.Values.Count == 0)
            {
                return null;
            }
            // Normal list
            else
            {
                object firstElement = Evaluate(expr.Values[0]);

                // Properly formed
                if (firstElement is ICallable f)
                {
                    // Do we have the correct amount of args?
                    if (f.Arity().IsInRange(expr.Values.Count - 1))
                    {
                        // If so, continue with the call.
                        return f.Call(this, expr.Values.Skip(1).ToList());
                    }
                    else
                    {
                        throw new RuntimeException($"Incorrect number of arguments for function. Expected {f.Arity()} argument(s), got {expr.Values.Count - 1}.");
                    }
                }
                // Improperly formed
                else
                {
                    throw new RuntimeException($"Cannot call non-symbol value '{Stringify(firstElement)}'.");
                }
            }
        }
    }
}
