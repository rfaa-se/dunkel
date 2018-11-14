using Dunkel.Game.Components.Attributes;
using Microsoft.Xna.Framework;

namespace Dunkel.Game.Commands.Game
{
    public class SpawnCommand : ICommand
    {
        public CommandType Type => CommandType.Spawn;
        public Point Point { get; private set; }
        public ClassificationType ClassificationType { get; private set; }

        public SpawnCommand(Point point, ClassificationType type)
        {
            Point = point;
            ClassificationType = type;
        }
    }
}