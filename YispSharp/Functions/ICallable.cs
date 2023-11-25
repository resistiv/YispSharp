using YispSharp.Data;
using YispSharp.Utils;

namespace YispSharp.Functions
{
    /// <summary>
    /// Represents a callable object.
    /// </summary>
    public interface ICallable
    {
        /// <summary>
        /// Returns the <see cref="Range"/> of number of arguments this <see cref="ICallable"/> accepts.
        /// </summary>
        /// <returns></returns>
        public Range Arity();
        /// <summary>
        /// Calls this <see cref="ICallable"/>.
        /// </summary>
        /// <param name="interpreter">An <see cref="Interpreter"/> instance for evaluation and environment manipulation.</param>
        /// <param name="arguments">Arguments to pass into the <see cref="ICallable"/>.</param>
        /// <returns>The result of calling this <see cref="ICallable"/>.</returns>
        public object Call(Interpreter interpreter, List<SExpr> arguments);
    }
}
