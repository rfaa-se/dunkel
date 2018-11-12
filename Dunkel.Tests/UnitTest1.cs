using System;
using Dunkel.Game.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Dunkel.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        [Fact]
        public void Test1()
        {
            var x = Microsoft.Xna.Framework.MathHelper.Lerp(5, 4, 0.14f);
            //MathHelper.Lerp(0, 0, 0);
            _output.WriteLine(x.ToString());
        }
    }
}
