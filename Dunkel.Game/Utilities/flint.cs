namespace Dunkel.Game.Utilities
{
    /// <summary>
    /// This struct uses integers in the background to simulate floating point and can have up to two decimals.
    /// </summary>
    public struct flint
    {
        private const int Precision = 100;

        private readonly int _raw;

        /// <summary>
        /// The decimals can be maximum 99, anything above that will be added to the integers part.
        /// new flint(50, 100) => 51
        /// new flint(50, 5) => 50.05
        /// new flint(50, 50) => 50.50
        /// </summary>
        public flint(int integers, int decimals)
        {
            _raw = integers * Precision + decimals;
        }

        public flint(int integers)
        {
            _raw = integers * Precision;
        }

        public int ToInt() => (int)this;
        public float ToFloat() => (float)this;

        public static implicit operator flint(int number) => new flint(number);
        public static implicit operator int(flint number) => number._raw / Precision;
        public static implicit operator float(flint number) => number._raw / (float)Precision;

        public static flint operator +(flint one, flint two) => new flint(0, one._raw + two._raw);
        public static flint operator +(flint one, int two) => one + new flint(two);

        public static flint operator -(flint one, flint two) => new flint(0, one._raw - two._raw);
        public static flint operator -(flint one, int two) => one - new flint(two);

        public static flint operator *(flint one, flint two) => new flint(0, (one._raw * two._raw) / Precision);
        public static flint operator *(flint one, int two) => one * new flint(two);

        public static flint operator /(flint one, flint two) => new flint(0, (one._raw * Precision / two._raw));
        public static flint operator /(flint one, int two) => one / new flint(two);
    }
}