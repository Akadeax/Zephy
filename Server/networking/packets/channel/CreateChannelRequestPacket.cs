using Server;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Channel
{
    public class CreateChannelRequestPacketData : PacketData
    {
        public string name;
        public List<string> withMembers;

        public CreateChannelRequestPacketData(string name, List<string> withMembers)
        {
            this.name = name;
            this.withMembers = withMembers;
        }
    }

    class CreateChannelRequestPacketHandler : PacketHandler<CreateChannelRequestPacket>
    {
        protected override void Handle(CreateChannelRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;


        }
    }

    class CreateChannelRequestPacket : Packet
    {
        public const int TYPE = 3003;

        public CreateChannelRequestPacket(CreateChannelRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public CreateChannelRequestPacket(byte[] packet)
            : base(packet) { }

        public CreateChannelRequestPacketData Data
        {
            get { return ReadJsonObject<CreateChannelRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
