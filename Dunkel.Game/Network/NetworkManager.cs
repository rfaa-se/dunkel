using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dunkel.Game.Commands;
using Dunkel.Game.Network.Packets;
using Dunkel.Game.Network.Packets.Game;

namespace Dunkel.Game.Network
{
    public class NetworkManager
    {
        public PacketManager Packets { get; private set; }

        private readonly ConcurrentQueue<IPacket> _packets;
        private readonly Random _random;
        private bool _hasEnqueuedStartingPackets;

        public NetworkManager(PacketManager packetManager)
        {
            Packets = packetManager ?? throw new ArgumentNullException(nameof(packetManager));

            _random = new Random();
            _packets = new ConcurrentQueue<IPacket>();
        }

        public void Update()
        {
            if (!_hasEnqueuedStartingPackets)
            {
                _packets.Enqueue(new UpdatePacket(0, new ICommand[0]));
                _packets.Enqueue(new UpdatePacket(1, new ICommand[0]));
                _packets.Enqueue(new UpdatePacket(2, new ICommand[0]));
                _hasEnqueuedStartingPackets = true;
            }

            while (_packets.TryDequeue(out var packet))
            {
                Packets.Update(packet);
            }
        }

        public void SendPacket(IPacket packet, long tick)
        {
            // simulate networking with a random 50-200ms delay
            Task.Run(async () => 
            {
                await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(50, 200)));
                
                switch (packet.Type)
                {
                    case PacketType.GameUpdate:
                    {
                        var p = (UpdatePacket)packet;
                        _packets.Enqueue(new UpdatePacket(1337, p.Tick + 3, p.Commands));
                        break;
                    }
                }
            });
        }
    }
}