using Server;
using Server.Database.Channel;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.channel
{
    public class CreateChannelResponsePacketData : PacketData
    {
        public int httpStatus;
        public Channel newChannel;

        public CreateChannelResponsePacketData(int httpStatus, Channel newChannel)
        {
            this.httpStatus = httpStatus;
            this.newChannel = newChannel;
        }
    }

    class CreateChannelResponsePacket : Packet
    {
        public const int TYPE = 3004;

        public CreateChannelResponsePacket(CreateChannelResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public CreateChannelResponsePacket(byte[] packet)
            : base(packet) { }

        public CreateChannelResponsePacketData Data
        {
            get { return ReadJsonObject<CreateChannelResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
