using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using Server;

using Packets.General;
using System.Text.Unicode;
using System.Text;
using Packets.Auth;

namespace Packets
{
    public static class PacketReceiver
    {
        public const int SHUTDOWN = 1;

        private static readonly Dictionary<int, PacketHandler> handlers = new Dictionary<int, PacketHandler>()
        {
            { IdentifyPacket.TYPE, new IdentifyPacketHandler() },
            { LoginAttemptPacket.TYPE, new LoginAttemptPacketHandler() },
        };

        public static void Handle(byte[] packet, Socket clientSocket)
        {
            ushort packetType = BitConverter.ToUInt16(packet, 0);
            Zephy.Logger.Information($"Received packet, Length: {packet.Length} | Type: {packetType}");

            if(handlers.ContainsKey(packetType))
            {
                handlers[packetType].Handle(packet, clientSocket);
            }
            else
            {
                Zephy.Logger.Warning($"Packet Type '{packetType}' could not be identified/handled.");
            }
        }
    }
}
