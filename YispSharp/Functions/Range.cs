namespace YispSharp.Functions
{
    /// <summary>
    /// Represents a bounded range.
    /// </summary>
    public readonly struct Range
    {
        /// <summary>
        /// The inclusive minimum of this <see cref="Range"/>.
        /// </summary>
        public readonly int Min;
        /// <summary>
        /// The inclusive maximum of this <see cref="Range"/>.
        /// </summary>
        public readonly int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Checks if a number is within this <see cref="Range"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsInRange(int n)
        {
            return n >= Min && (n <= Max || Max == -1);
        }

        public override string ToString()
        {
            if (Max == -1)
            {
                return $"at least {Min}";
            }
            else if (Max == Min)
            {
                return $"{Max}";
            }
            else
            {
                return $"between {Min} and {Max}";
            }
        }
    }
}
