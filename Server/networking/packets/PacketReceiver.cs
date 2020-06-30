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
using MongoDB.Bson;

namespace Packets
{
    public static class PacketReceiver
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

            ushort packetType = BitConverter.ToUInt16(packet, 0);
            Console.WriteLine($"Received packet, Length: {packet.Length} | Type: {packetType}");

            switch (packetType)
            {
                case LoginPacket.TYPE:
                    LoginPacket loginPacket = new LoginPacket(packet);
                    Console.WriteLine($"Received login attempt with {loginPacket.Username} and password {loginPacket.Password}.");
                    break;

                case DeleteUserPacket.TYPE:
                    DeleteUserPacket deleteUserPacket = new DeleteUserPacket(packet);

                    Server.UserCrud userCrud = new Server.UserCrud("Zephy");
                    Server.User user = userCrud.ReadRecordById(deleteUserPacket.ToDeleteId);

                    Console.WriteLine($"Received Delete user packet, deleting {deleteUserPacket.ToDeleteId} ({user.name}).");
                    userCrud.DeleteRecord(deleteUserPacket.ToDeleteId);
                    break;
            }


            return 0;
        }
    }
}
