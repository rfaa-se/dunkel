namespace Dunkel.Game.Utilities
{
    public static class FlintMath
    {
        public static Flint Sqrt(Flint number)
        {
            // modified and taken from https://stackoverflow.com/a/16366529
            long s = 1;
            long t = number.Raw * Flint.Precision;
            long x = t;

            while (s < t)
            {
                s <<= 1;
                t >>= 1;
            } //decide the value of the first tentative

            do
            {
                t = s;
                s = (x / s + s) >> 1; //x1=(N / x0 + x0)/2 : recurrence formula
            } while (s < t);

            return new Flint((int)(t / Flint.Precision), (int)(t % Flint.Precision));
        }
    }
}