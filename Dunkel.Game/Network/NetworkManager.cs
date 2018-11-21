using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dunkel.Game.Commands;
using Dunkel.Game.Commands.Game;
using Dunkel.Game.Network.Packets;
using Dunkel.Game.Network.Packets.Game;
using Microsoft.Xna.Framework;

namespace Dunkel.Game.Network
{
    public class NetworkManager
    {
        public PacketManager Packets { get; private set; }

        private readonly ConcurrentQueue<IPacket> _packets;
        private readonly Random _random;

        public NetworkManager(PacketManager packetManager)
        {
            Packets = packetManager ?? throw new ArgumentNullException(nameof(packetManager));

            _random = new Random();
            _packets = new ConcurrentQueue<IPacket>();
        }

        public void Update()
        {
            while (_packets.TryDequeue(out var packet))
            {
                Packets.Update(packet);
            }
        }

        public void SendPacket(IPacket packet)
        {
            // simulate networking with a random 50-160ms delay
            Task.Run(async () => 
            {
                await Task.Delay(TimeSpan.FromMilliseconds(_random.Next(50, 160)));
                
                switch (packet.Type)
                {
                    case PacketType.GameUpdate:
                    {
                        var p = (UpdatePacket)packet;
                        _packets.Enqueue(new UpdatePacket(1337, p.Tick + 3, p.Commands));
                        break;
                    }
                }

                switch (packet.Type)
                {
                    case PacketType.GameUpdate:
                    {
                        var p = (UpdatePacket)packet;
                        ICommand[] commands = new ICommand[p.Commands.Length];
                        for (var i = 0; i < commands.Length; i++)
                        {
                            var command = p.Commands[i];
                            switch (command.Type)
                            {
                                case CommandType.GameSpawn:
                                {
                                    var c = (SpawnCommand)command;
                                    commands[i] = new SpawnCommand(new Point(c.Position.X / 2, c.Position.Y / 2), c.ClassificationType);
                                    break;
                                }
                            }
                        }
                        _packets.Enqueue(new UpdatePacket(1338, p.Tick + 3, commands));
                        break;
                    }
                }
            });
        }
    }
}