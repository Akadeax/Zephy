using Server;

using System;
using System.Collections.Generic;
using System.Net.Sockets;

using Packets.general;
using Packets.auth;
using Packets.channel;
using Packets.user;
using Packets.message;

namespace Packets
{
    public static class PacketReceiver
    {
        // Associate received packet type with given handler
        private static readonly Dictionary<int, PacketHandler> handlers = new Dictionary<int, PacketHandler>()
        {
            { IdentifyPacket.TYPE, new IdentifyPacketHandler() },
            { LoginAttemptPacket.TYPE, new LoginAttemptPacketHandler() },
            { FetchChannelsRequestPacket.TYPE, new FetchChannelsRequestPacketHandler() },
            { ConfirmSessionRequestPacket.TYPE, new ConfirmSessionRequestPacketHandler() },
            { FetchUserListRequestPacket.TYPE, new FetchUserListRequestPacketHandler() },
            { CreateChannelRequestPacket.TYPE, new CreateChannelRequestPacketHandler() },
            { PopulateMessagesRequestPacket.TYPE, new PopulateMessagesRequestPacketHandler() },
            { SendMessageRequestPacket.TYPE, new SendMessageRequestPacketHandler() },
            { FetchMembersRequestPacket.TYPE, new FetchMembersRequestPacketHandler() },
        };

        /// <summary>
        /// Hand the received buffer to the appropriate handler based on PacketType
        /// </summary>
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
