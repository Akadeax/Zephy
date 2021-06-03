using Server;
using Server.Database.Channel;
using Server.Database.User;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.channel
{
    public class FetchMembersRequestPacketData : PacketData
    {
        public string channel;

        public FetchMembersRequestPacketData(string channel)
        {
            this.channel = channel;
        }
    }

    class FetchMembersRequestPacketHandler : PacketHandler<FetchMembersRequestPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(FetchMembersRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            PopulatedChannel channel = channelCrud.ReadOnePopulated(data.channel);
            if(channel == null)
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }
            
            var response = new FetchMembersResponsePacket(new FetchMembersResponsePacketData(
                (int)HttpStatusCode.OK, data.channel, channel.members
            ));
            Zephy.serverSocket.SendPacket(response, sender);
        }

        private void SendError(HttpStatusCode code, Socket socket)
        {
            var errResponse = new FetchMembersResponsePacket(new FetchMembersResponsePacketData(
                (int)code,
                null, null
            ));
            Zephy.serverSocket.SendPacket(errResponse, socket);
        }
    }


    class FetchMembersRequestPacket : Packet
    {
        public const int TYPE = 5003;

        public FetchMembersRequestPacket(FetchMembersRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchMembersRequestPacket(byte[] packet)
            : base(packet) { }

        public FetchMembersRequestPacketData Data
        {
            get { return ReadJsonObject<FetchMembersRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
