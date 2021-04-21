﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using server;

using packets.general;
using System.Text.Unicode;
using System.Text;
using packets.auth;

namespace packets
{
    public static class PacketReceiver
    {
        public const int SHUTDOWN = 1;

        private static readonly Dictionary<int, PacketHandler> handlers = new Dictionary<int, PacketHandler>()
        {
            { IdentifyPacket.TYPE, new IdentifyPacketHandler() },
            { LoginAttemptPacket.TYPE, new LoginAttemptPacketHandler() },
        };

        public static int Handle(byte[] packet, Socket clientSocket)
        {
            // 0 Length packet indicates close from the other side
            if (packet.Length == 0)
            {
                IPAddress disconnectedAddress = (clientSocket.LocalEndPoint as IPEndPoint).Address;
                Zephy.Logger.Information($"{disconnectedAddress} has disconnected, closing & disposing socket.");
                clientSocket.Close();
                return SHUTDOWN;
            }

            ushort packetType = BitConverter.ToUInt16(packet, 0);
            Zephy.Logger.Information($"Received packet, Length: {packet.Length} | Type: {packetType}");

            if(handlers.ContainsKey(packetType))
            {
                handlers[packetType].Handle(packet, clientSocket);
            }
            else
            {
                Zephy.Logger.Information($"Packet Type '{packetType}' could not be identified/handled.");
            }


            return 0;
        }
    }
}
