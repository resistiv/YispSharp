using YispSharp.Exceptions;

namespace YispSharp.Data
{
    public class Environment
    {
        public readonly Environment Enclosing;
        private readonly Dictionary<string, object> Values = new();

        public Environment()
        {
            Enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            Enclosing = enclosing;
        }

        /// <summary>
        /// Defines a variable within this <see cref="Environment"/>.
        /// </summary>
        /// <param name="name">The name, or identifier, of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void Define(string name, object value)
        {
            Values[name] = value;
        }

        /// <summary>
        /// Gets a variable from this <see cref="Environment"/>.
        /// </summary>
        /// <param name="name">The name, or identifier, of the variable.</param>
        /// <returns>The value of the variable.</returns>
        /// <exception cref="RuntimeException">Thrown when the variable is undefined.</exception>
        public object Get(Token name)
        {
            if (Values.ContainsKey(name.Lexeme))
            {
                return Values[name.Lexeme];
            }

            if (Enclosing != null)
            {
                return Enclosing.Get(name);
            }

            throw new RuntimeException($"Unknown symbol '{name.Lexeme}'.");
        }
    }
}
