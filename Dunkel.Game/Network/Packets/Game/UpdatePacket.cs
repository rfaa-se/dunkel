using Dunkel.Game.Commands;

namespace Dunkel.Game.Network.Packets.Game
{
    public class UpdatePacket : IPacket
    {
        public PacketType Type => PacketType.GameUpdate;
        public long ClientId { get; private set; }
        public long Tick { get; private set; }
        public ICommand[] Commands { get; private set; }

        public UpdatePacket(long tick, ICommand[] commands) : this(0, tick, commands) {}

        public UpdatePacket(long clientId, long tick, ICommand[] commands)
        {
            ClientId = clientId;
            Tick = tick;
            Commands = commands;
        }
    }
}