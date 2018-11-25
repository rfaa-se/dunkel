using Microsoft.Xna.Framework;

namespace Dunkel.Game.Commands.Game
{
    public class MoveCommand : ICommand
    {
        public CommandType Type => CommandType.GameMove;
        public Point Destination { get; private set; }
        public int[] EntityIds { get; private set; }

        public MoveCommand(Point destination, int[] entityIds)
        {
            Destination = destination;
            EntityIds = entityIds;
        }
    }
}