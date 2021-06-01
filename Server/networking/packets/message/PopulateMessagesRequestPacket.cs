using Server;
using Server.Database.Channel;
using Server.Database.Message;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class PopulateMessagesRequestPacketData : PacketData
    {
        public string forChannel;
        public int page;

        public PopulateMessagesRequestPacketData(string forChannel, int page)
        {
            this.forChannel = forChannel;
            this.page = page;
        }
    }

    class PopulateMessagesRequestPacketHandler : PacketHandler<PopulateMessagesRequestPacket>
    {
        const int PAGE_SIZE = 25;

        readonly MessageCrud messageCrud = new MessageCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(PopulateMessagesRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            var channel = channelCrud.ReadOneById(data.forChannel);
            if(channel == null)
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }

            List<PopulatedMessage> paginatedMessages = messageCrud.ReadManyPaginated(channel._id, data.page, PAGE_SIZE);
            var response = new PopulateMessagesResponsePacket(new PopulateMessagesResponsePacketData(
                (int)HttpStatusCode.OK, data.page, paginatedMessages
            ));
            Zephy.serverSocket.SendPacket(response, sender);
        }

        void SendError(HttpStatusCode status, Socket sender)
        {
            var response = new PopulateMessagesResponsePacket(new PopulateMessagesResponsePacketData(
                (int)status, 0, null
            ));
            Zephy.serverSocket.SendPacket(response, sender);
        }
    }

    class PopulateMessagesRequestPacket : Packet
    {
        public const int TYPE = 4001;

        public PopulateMessagesRequestPacket(PopulateMessagesRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public PopulateMessagesRequestPacket(byte[] packet)
            : base(packet) { }

        public PopulateMessagesRequestPacketData Data
        {
            get { return ReadJsonObject<PopulateMessagesRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
