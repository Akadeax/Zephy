using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Packets.General;
using Packets.Auth;
using Packets.Message;
using Packets.User;
using Packets.Channel;

namespace Packets
{
    public static class PacketHandler
    {
        public const int SHUTDOWN = 1;

        public static int Handle(byte[] packet, Socket clientSocket)
        {
            // 0 Length packet indicates close from the other side
            if (packet.Length == 0)
            {
                IPAddress disconnectedAddress = (clientSocket.LocalEndPoint as IPEndPoint).Address;
                Console.WriteLine($"{disconnectedAddress.ToString()} has disconnected, closing & disposing socket.");
                clientSocket.Close();
                return SHUTDOWN;
            }

            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            Console.WriteLine($"Received packet, Length: {packetLength} | Type: {packetType}");

            switch (packetType)
            {
                case LoginPacket.TYPE:
                    LoginPacket loginPacket = new LoginPacket(packet);
                    Console.WriteLine($"Received login attempt with {loginPacket.Username} and password {loginPacket.Password}.");
                    break;
            }

            return 0;
        }
    }
}
