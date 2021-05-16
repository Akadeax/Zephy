using Server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class SendMessageResponsePacketData : PacketData
    {
        public int httpStatus;
        public string forChannel;
        public string content;
        public string author;

        public SendMessageResponsePacketData(int httpStatus, string forChannel, string content, string author)
        {
            this.httpStatus = httpStatus;
            this.forChannel = forChannel;
            this.content = content;
            this.author = author;
        }
    }

    class SendMessageResponsePacket : Packet
    {
        public const int TYPE = 4004;

        public SendMessageResponsePacket(SendMessageResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public SendMessageResponsePacket(byte[] packet)
            : base(packet) { }

        public SendMessageResponsePacketData Data
        {
            get { return ReadJsonObject<SendMessageResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
