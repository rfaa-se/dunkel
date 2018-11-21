using Dunkel.Game.Components.Attributes;
using Microsoft.Xna.Framework;

namespace Dunkel.Game.Commands.Game
{
    public class SpawnCommand : ICommand
    {
        public CommandType Type => CommandType.GameSpawn;
        public Point Position { get; private set; }
        public ClassificationType ClassificationType { get; private set; }

        public SpawnCommand(Point position, ClassificationType type)
        {
            Position = position;
            ClassificationType = type;
        }
    }
}