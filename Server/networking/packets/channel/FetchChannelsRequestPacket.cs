using Server;
using Server.Database.Channel;
using Server.Database.User;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Channel
{
    public class FetchChannelsRequestPacketData : PacketData
    {
        public string search;
        public FetchChannelsRequestPacketData(string search)
        {
            this.search = search;
        }
    }

    class FetchChannelsRequestPacketHandler : PacketHandler<FetchChannelsRequestPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly UserCrud userCrud = new UserCrud();

        protected override void Handle(FetchChannelsRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            ActiveUser activeUser = ActiveUsers.GetUser(sender);
            if (activeUser == null)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            User user = userCrud.ReadOneById(activeUser.userId);

            List<BaseChannelData> channels = channelCrud.ReadManyBase(x =>
                x.members.Contains(user._id) &&
                x.name.ToLower().Contains(data.search.ToLower())
            );

            var response = new FetchChannelsResponsePacket(new FetchChannelsResponsePacketData(
                (int)HttpStatusCode.OK,
                channels
            ));

            Zephy.serverSocket.SendPacket(response, sender);
        }

        private void SendError(HttpStatusCode code, Socket socket)
        {
            var errResponse = new FetchChannelsResponsePacket(new FetchChannelsResponsePacketData(
                (int)code,
                null
            ));
            Zephy.serverSocket.SendPacket(errResponse, socket);
        }
    }


    class FetchChannelsRequestPacket : Packet
    {
        public const int TYPE = 3001;

        public FetchChannelsRequestPacket(FetchChannelsRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchChannelsRequestPacket(byte[] packet)
            : base(packet) { }

        public FetchChannelsRequestPacketData Data
        {
            get { return ReadJsonObject<FetchChannelsRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
