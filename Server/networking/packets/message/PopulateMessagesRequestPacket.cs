using server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace packets.message
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
        protected override void Handle(PopulateMessagesRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;


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
