namespace Dunkel.Game.Utilities
{
    /// <summary>
    /// This struct uses longs in the background to simulate floating point and can have up to two decimals.
    /// </summary>
    public struct Flint
    {
        public const int Precision = 100;
        public readonly long Raw;

        /// <summary>
        /// The decimals can be maximum 99, anything above that will be added to the integers part.
        /// new flint(50, 100) => 51
        /// new flint(50, 5) => 50.05
        /// new flint(50, 50) => 50.50
        /// </summary>
        public Flint(int integers, int decimals)
        {
            Raw = integers * Precision + decimals;
        }

        public Flint(int integers)
        {
            Raw = integers * Precision;
        }

        public int ToInt() => (int)this;
        public float ToFloat() => (float)this;

        public static implicit operator Flint(int number) => new Flint(number);
        public static implicit operator int(Flint number) => (int)(number.Raw / Precision);
        public static implicit operator float(Flint number) => number.Raw / (float)Precision;

        public static Flint operator +(Flint one, Flint two) => new Flint(0, (int)(one.Raw + two.Raw));
        public static Flint operator +(Flint one, int two) => one + new Flint(two);

        public static Flint operator -(Flint one, Flint two) => new Flint(0, (int)(one.Raw - two.Raw));
        public static Flint operator -(Flint one, int two) => one - new Flint(two);

        public static Flint operator *(Flint one, Flint two) => new Flint(0, (int)((one.Raw * two.Raw) / Precision));
        public static Flint operator *(Flint one, int two) => one * new Flint(two);

        public static Flint operator /(Flint one, Flint two) => new Flint(0, (int)((one.Raw * Precision / two.Raw)));
        public static Flint operator /(Flint one, int two) => one / new Flint(two);
    }
}