using Server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class SendMessageRequestPacketData : PacketData
    {
        public string forChannel;
        public string content;

        public SendMessageRequestPacketData(string forChannel, string content)
        {
            this.forChannel = forChannel;
            this.content = content;
        }
    }

    class SendMessageRequestPacketHandler : PacketHandler<SendMessageRequestPacket>
    {
        protected override void Handle(SendMessageRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;


        }
    }

    class SendMessageRequestPacket : Packet
    {
        public const int TYPE = 4003;

        public SendMessageRequestPacket(SendMessageRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public SendMessageRequestPacket(byte[] packet)
            : base(packet) { }

        public SendMessageRequestPacketData Data
        {
            get { return ReadJsonObject<SendMessageRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
