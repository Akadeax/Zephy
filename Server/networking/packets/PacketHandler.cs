using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public static class PacketHandler
    {
        public const int SHUTDOWN = 1;

        public static int Handle(byte[] packet, Socket clientSocket)
        {
            if (packet.Length == 0)
            {
                clientSocket.Close();
                return SHUTDOWN;
            }

            ushort packetLength = BitConverter.ToUInt16(packet, 0);
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            Console.WriteLine($"Received packet, Length: {packetLength} | Type: {packetType}");

            switch (packetType)
            {
                case MessagePacket.TYPE:
                    MessagePacket msgPacket = new MessagePacket(packet);
                    Console.WriteLine($"Received Message Packet: {msgPacket.Message}");
                    break;
            }

            return 0;
        }
    }
}
