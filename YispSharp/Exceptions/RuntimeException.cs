using YispSharp.Data;

namespace YispSharp.Exceptions
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message) { }
    }
}
