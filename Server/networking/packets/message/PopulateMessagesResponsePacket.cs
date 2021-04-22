using Server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Message
{
    public class PopulateMessagesResponsePacketData : PacketData
    {
        public int httpStatus;
        public string user;

        public PopulateMessagesResponsePacketData(int httpStatus, string user)
        {
            this.httpStatus = httpStatus;
            this.user = user;
        }
    }

    class PopulateMessagesResponsePacket : Packet
    {
        public const int TYPE = 4002;

        public PopulateMessagesResponsePacket(PopulateMessagesResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public PopulateMessagesResponsePacket(byte[] packet)
            : base(packet) { }

        public PopulateMessagesResponsePacketData Data
        {
            get { return ReadJsonObject<PopulateMessagesResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
