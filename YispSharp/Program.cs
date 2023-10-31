namespace YispSharp
{
    /// <summary>
    /// Main entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">Optionally, a script for Y# to run.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("YispSharp v0.1.0 - Kai NeSmith 2023");

            if (args.Length > 1)
            {
                Console.WriteLine("Usage: YispSharp [Yisp script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                Yisp.RunFile(args[0]);
            }
            else
            {
                Yisp.RunPrompt();
            }
        }
    }
}