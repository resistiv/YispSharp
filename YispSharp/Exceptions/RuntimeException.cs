using YispSharp.Utils;

namespace YispSharp.Exceptions
{
    /// <summary>
    /// Represents an <see cref="Exception"/> encountered by an <see cref="Interpreter"/> during runtime.
    /// </summary>
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message) { }
    }
}
