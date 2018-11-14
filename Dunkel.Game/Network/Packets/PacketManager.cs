using System;

namespace Dunkel.Game.Network.Packets
{
    public class PacketManager
    {
        public event Action<Game.UpdatePacket> OnGameUpdate;

        public void Update(IPacket packet)
        {
            switch (packet.Type)
            {
                case PacketType.GameUpdate:
                    OnGameUpdate?.Invoke((Game.UpdatePacket)packet);
                    break;
            }
        }
    }
}