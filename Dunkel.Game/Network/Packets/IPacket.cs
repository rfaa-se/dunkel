namespace Dunkel.Game.Network.Packets
{
    public interface IPacket
    {
         PacketType Type { get; }
    }
}