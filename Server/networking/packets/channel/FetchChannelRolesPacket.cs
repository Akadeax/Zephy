using MongoDB.Bson;
using Server;
using Server.database.channel;
using Server.database.role;
using Server.database.user;
using Server.utilityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Packets.channel
{
    public class FetchChannelRolesPacketData : PacketData
    {
        public string forChannel;
        public List<Role> roles;

        public FetchChannelRolesPacketData(string forChannel, List<Role> roles)
        {
            this.forChannel = forChannel;
            this.roles = roles;
        }
    }

    public class FetchChannelRolesPacketHandler : PacketHandler<FetchChannelRolesPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(FetchChannelRolesPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            PopulatedChannel channel = channelCrud.ReadOnePopulated(data.forChannel);
            if (channel == null) return;

            var retPacket = new FetchChannelRolesPacket(new FetchChannelRolesPacketData(
                data.forChannel,
                channel.roles
            ));
            Zephy.serverSocket.SendPacket(retPacket, sender);
        }
    }

    public class FetchChannelRolesPacket : Packet
    {
        public const int TYPE = 3002;

        public FetchChannelRolesPacket(FetchChannelRolesPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchChannelRolesPacket(byte[] packet)
            : base(packet) { }

        public FetchChannelRolesPacketData Data
        {
            get { return ReadJsonObject<FetchChannelRolesPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
