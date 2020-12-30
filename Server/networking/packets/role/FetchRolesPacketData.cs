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
    public class FetchRolesPacketData : PacketData
    {
        public string forChannel;
        public List<Role> roles;

        public FetchRolesPacketData(string forChannel, List<Role> roles)
        {
            this.forChannel = forChannel;
            this.roles = roles;
        }
    }

    public class FetchRolesPacketHandler : PacketHandler<FetchRolesPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly RoleCrud roleCrud = new RoleCrud();

        protected override void Handle(FetchRolesPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            List<Role> roles;

            if (data.forChannel == "")
            {
                roles = roleCrud.ReadMany();
            }
            else
            {
                PopulatedChannel channel = channelCrud.ReadOnePopulated(data.forChannel);
                if (channel == null) return;
                roles = channel.roles;
            }

            var retPacket = new FetchRolesPacket(new FetchRolesPacketData(
                data.forChannel,
                roles
            ));

            Zephy.serverSocket.SendPacket(retPacket, sender);
        }
    }

    public class FetchRolesPacket : Packet
    {
        public const int TYPE = 5000;

        public FetchRolesPacket(FetchRolesPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchRolesPacket(byte[] packet)
            : base(packet) { }

        public FetchRolesPacketData Data
        {
            get { return ReadJsonObject<FetchRolesPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
