using System;
using Microsoft.Extensions.DependencyInjection;

namespace Dunkel.Game
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Dunkel())
            {
                var services = new ServiceCollection();
                game.Configure(services);
                game.ConfigureServices(services.BuildServiceProvider());
                game.Run();
            }
        }
    }
}
