﻿using YispSharp.Data;
using YispSharp.Exceptions;
using YispSharp.Utils;
using Environment = System.Environment;

namespace YispSharp
{
    /// <summary>
    /// Main utility class.
    /// </summary>
    public static class Yisp
    {
        private static Interpreter interpreter = new();
        private static bool _hadError = false;
        private static bool _hadRuntimeError = false;

        /// <summary>
        /// Runs a Yisp file.
        /// </summary>
        /// <param name="path">A path to a Yisp file.</param>
        public static void RunFile(string path)
        {
            string fileContents = string.Empty;
            try
            {
                fileContents = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: Could not access file '{path}', aborting. Reason: {e.Message}");
                Environment.Exit(65);
            }
            Run(fileContents);

            if (_hadError)
            {
                Environment.Exit(65);
            }
            if (_hadRuntimeError)
            {
                Environment.Exit(70);
            }
        }

        /// <summary>
        /// Runs Y# as a REPL prompt.
        /// </summary>
        public static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }
                Run(line);
                _hadError = false;
            }
        }

        /// <summary>
        /// Runs some quantity of Yisp code.
        /// </summary>
        /// <param name="source">A piece of Yisp code.</param>
        public static void Run(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();

            Parser parser = new(tokens);
            List<SExpr> expressions = parser.Parse();

            if (_hadError)
            {
                return;
            }

            interpreter.Interpret(expressions);
        }

        /// <summary>
        /// Prints an error.
        /// </summary>
        /// <param name="line">The line number of where the error occurred.</param>
        /// <param name="message">A message addressing the error.</param>
        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        /// <summary>
        /// Prints an error.
        /// </summary>
        /// <param name="token">The <see cref="Token"/> that caused the error.</param>
        /// <param name="message">A message addressing the error.</param>
        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.Eof)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, $" at '{token.Lexeme}'", message);
            }
        }

        /// <summary>
        /// Prints a runtime error.
        /// </summary>
        /// <param name="error">The <see cref="RuntimeException"/> representing the error.</param>
        public static void RuntimeError(RuntimeException error)
        {
            Console.WriteLine($"RuntimeException: {error.Message}");
            _hadRuntimeError = true;
        }

        /// <summary>
        /// Reports a message.
        /// </summary>
        /// <param name="line">The line number of where the message originates.</param>
        /// <param name="where">Where the message originates from.</param>
        /// <param name="message">A message.</param>
        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            _hadError = true;
        }

        /// <summary>
        /// Used for unit testing to reset static variables.
        /// </summary>
        public static void DebugReset()
        {
            interpreter = new();
            _hadError = false;
            _hadRuntimeError = false;
        }
    }
}
