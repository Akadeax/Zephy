using Server;
using Server.Database.Channel;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Channel
{
    public class FetchChannelsRequestPacketData : PacketData
    {
        public string search;
        public FetchChannelsRequestPacketData(string search)
        {
            this.search = search;
        }
    }

    class FetchChannelsRequestPacketHandler : PacketHandler<FetchChannelsRequestPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(FetchChannelsRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            var response = new FetchChannelsResponsePacket(new FetchChannelsResponsePacketData(
                (int)HttpStatusCode.OK,
                channelCrud.ReadManyBase(x => x.name.ToLower().Contains(data.search.ToLower()))
            ));

            Zephy.serverSocket.SendPacket(response, sender);
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
