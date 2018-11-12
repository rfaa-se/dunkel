using Dunkel.Game.Utilities;
using Xunit;

namespace Dunkel.Tests.Utilities
{
    public class flintTests
    {
        [Fact]
        public void TestAssignment()
        {
            flint x = 10;
            Assert.Equal(10, (int)x);
            Assert.Equal(10, (float)x);

            var y = new flint(10);
            Assert.Equal(10, (int)y);
            Assert.Equal(10, (float)y);

            var z = new flint(10, 50);
            Assert.Equal(10, (int)z);
            Assert.Equal(10.5f, (float)z);

            var a = new flint(10, 5);
            Assert.Equal(10, (int)a);
            Assert.Equal(10.05f, (float)a);

            var b = new flint(10, 50);
            Assert.Equal(10, b.ToInt());
            Assert.Equal(10.50f, b.ToFloat());
        }

        [Fact]
        public void TestAddition()
        {
            var x = new flint(20);
            var y = x + new flint(10);
            Assert.Equal(30, (int)y); // 20 + 10 -> 30

            x += 10;
            Assert.Equal(30, (int)x); // 20 + 10 -> 30

            x += new flint(0, 50);
            Assert.Equal(30, (int)x); // 30 + 0.5 -> 30 int
            Assert.Equal(30.5f, (float)x); // 30 + 0.5 -> 30.5 float

            x += new flint(0, 150);
            Assert.Equal(32, (int)x); // 30.5 + 1.5 -> 32
            Assert.Equal(32f, (float)x); // 30.5 + 1.5 -> 32
        }

        [Fact]
        public void TestSubtraction()
        {
            var x = new flint(20);
            var y = x - new flint(10);
            Assert.Equal(10, (int)y); // 20 - 10 -> 10

            x -= 10;
            Assert.Equal(10, (int)x); // 20 - 10 -> 10

            x -= new flint(0, 50);
            Assert.Equal(9, (int)x); // 10 - 0.5 -> 9 int
            Assert.Equal(9.5f, (float)x); // 10 - 0.5 -> 9.5 float

            x -= new flint(0, 150);
            Assert.Equal(8, (int)x); // 9.5 - 1.5 -> 8
            Assert.Equal(8f, (float)x); // 9.5 - 1.5 -> 8
        }

        [Fact]
        public void TesMultiplication()
        {
            var x = new flint(20);
            var y = x * new flint(10);
            Assert.Equal(200, (int)y); // 20 * 10 -> 200

            x *= 10;
            Assert.Equal(200, (int)x); // 20 * 10 -> 200

            x *= new flint(0, 50);
            Assert.Equal(100, (int)x); // 200 * 0.5 -> 100
            Assert.Equal(100f, (float)x); // 200 * 0.5 -> 100

            x *= new flint(0, 150);
            Assert.Equal(150, (int)x); // 100 * 1.5 -> 150
            Assert.Equal(150f, (float)x); // 100 * 1.5 -> 150
        }

        [Fact]
        public void TestDivision()
        {
            var x = new flint(200);
            var y = x / new flint(10);
            Assert.Equal(20, (int)y); // 200 / 10 -> 20

            x /= 10;
            Assert.Equal(20, (int)x); // 200 / 10 -> 20

            x /= new flint(0, 50);
            Assert.Equal(40, (int)x); // 20 / 0.5 -> 40
            Assert.Equal(40f, (float)x); // 20 / 0.5 -> 40

            x /= new flint(0, 150);
            Assert.Equal(26, (int)x); // 40 / 1.5 -> 26 int
            Assert.Equal(26.66f, (float)x); // 40 / 1.5 -> 26.666666.. -> 26.66 float
        }
    }
}