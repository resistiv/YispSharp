﻿using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;

namespace YispSharp.Functions.Native
{
    /// <summary>
    /// Handles numeric multiplication.
    /// </summary>
    public class Multiplication : ICallable
    {
        public Range Arity()
        {
            return new Range(2, -1);
        }

        public object Call(Interpreter interpreter, List<SExpr> arguments)
        {
            double result = 1;
            foreach (SExpr s in arguments)
            {
                object arg = interpreter.Evaluate(s);
                if (arg is not double d)
                {
                    throw new RuntimeException($"Operand '{interpreter.Stringify(arg)}' is not a number.");
                }

                result *= d;
            }
            return result;
        }
    }
}
