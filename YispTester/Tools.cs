using System;
using System.IO;
using YispSharp;

namespace YispTester
{
    /// <summary>
    /// Contains utility methods to make code execution and output testing easier.
    /// </summary>
    public static class Tools
    {
        private static TextWriter _originalOutTw = null;
        private static StringWriter _outputCaptureSw = null;

        /// <summary>
        /// Runs a piece of Yisp within Y#, reads the output into an array, and prints the output.
        /// </summary>
        /// <param name="code">The name of the file to run.</param>
        /// <returns>Output of the script, split at newlines.</returns>
        public static string RunCode(string code)
        {
            // Record output
            StartOutputCapture();

            // Run the script
            Yisp.Run(code);

            // Reset console output and write out output
            string output = FinishOutputCapture();
            Console.WriteLine(output);

            // Reset the system to clear out state (errors, interpreter gunk)
            Yisp.DebugReset();

            // Return captured output
            return output.Trim().ReplaceLineEndings("\n");
        }

        /// <summary>
        /// Swaps the current <see cref="Console"/> output with a <see cref="StringWriter"/> to capture standard output.
        /// </summary>
        private static void StartOutputCapture()
        {
            // Swap output with custom StringWriter
            _outputCaptureSw = new();
            _originalOutTw = Console.Out;
            Console.SetOut(_outputCaptureSw);
        }

        /// <summary>
        /// Swaps the current <see cref="Console"/> output back to the previous output <see cref="TextWriter"/> and returns what the <see cref="StringWriter"/> captured.
        /// </summary>
        /// <returns></returns>
        private static string FinishOutputCapture()
        {
            // Record captured output
            string output = _outputCaptureSw.ToString();

            // Swap out custom capture TextWriter with original
            Console.SetOut(_originalOutTw);
            _outputCaptureSw.Dispose();
            _outputCaptureSw = null;
            _originalOutTw = null;

            return output;
        }
    }
}
