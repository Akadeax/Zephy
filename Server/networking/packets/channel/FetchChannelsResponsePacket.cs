using Server;
using Server.Database.Channel;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Channel
{
    public class FetchChannelsResponsePacketData : PacketData
    {
        public int httpStatus;
        public List<BaseChannelData> channels;

        public FetchChannelsResponsePacketData(int httpStatus, List<BaseChannelData> channels)
        {
            this.httpStatus = httpStatus;
            this.channels = channels;
        }
    }

    class FetchChannelsResponsePacket : Packet
    {
        public const int TYPE = 3002;

        public FetchChannelsResponsePacket(FetchChannelsResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchChannelsResponsePacket(byte[] packet)
            : base(packet) { }

        public FetchChannelsResponsePacketData Data
        {
            get { return ReadJsonObject<FetchChannelsResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
