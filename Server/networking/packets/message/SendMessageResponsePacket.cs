using Server;
using Server.Database.Message;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class SendMessageResponsePacketData : PacketData
    {
        public int httpStatus;
        public PopulatedMessage message;
        public string channel;

        public SendMessageResponsePacketData(int httpStatus, PopulatedMessage message, string channel)
        {
            this.httpStatus = httpStatus;
            this.message = message;
            this.channel = channel;
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
