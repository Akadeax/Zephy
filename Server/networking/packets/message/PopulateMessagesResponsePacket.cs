using Server;
using Server.Database.Message;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class PopulateMessagesResponsePacketData : PacketData
    {
        public int httpStatus;
        public int page;
        public List<PopulatedMessage> fetchedMessages;

        public PopulateMessagesResponsePacketData(int httpStatus, int page, List<PopulatedMessage> fetchedMessages)
        {
            this.httpStatus = httpStatus;
            this.page = page;
            this.fetchedMessages = fetchedMessages;
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
