using server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace packets.channel
{
    public class FetchChannelsRequestPacketData : PacketData
    {
        public string forUser;

        public FetchChannelsRequestPacketData(string forUser)
        {
            this.forUser = forUser;
        }
    }

    class FetchChannelsRequestPacketHandler : PacketHandler<FetchChannelsRequestPacket>
    {
        protected override void Handle(FetchChannelsRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;


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
