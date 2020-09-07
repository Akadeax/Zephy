using MongoDB.Bson;
using Server;
using Server.database.channel;
using Server.database.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Packets.channel
{
    public class AccessibleChannelsInfoPacketData : PacketData
    {
        public List<BaseChannelData> accessibleChannelsData;
        public string forUser;

        public AccessibleChannelsInfoPacketData(List<BaseChannelData> accessibleChannelsData, string forUser)
        {
            this.accessibleChannelsData = accessibleChannelsData;
            this.forUser = forUser;
        }
    }

    public class AccessibleChannelsInfoPacketHandler : PacketHandler<AccessibleChannelsInfoPacket>
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(AccessibleChannelsInfoPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            Zephy.Logger.Information("Received Request for AccessibleChannelsData");

            PopulatedUser forUser = userCrud.ReadOnePopulated(data.forUser);
            var accessibleChannels = new List<BaseChannelData>();
            if (forUser != null)
            {
                foreach (Channel currChannel in channelCrud.ReadMany())
                {
                    foreach (string currUserRole in forUser.roles.Select(x => x._id))
                    {
                        if (currChannel.roles.Contains(currUserRole))
                        {
                            accessibleChannels.Add(currChannel.AsBaseChannelData);
                            break;
                        }
                    }
                }
            }


            AccessibleChannelsInfoPacket ret = new AccessibleChannelsInfoPacket(new AccessibleChannelsInfoPacketData(
                accessibleChannels, data.forUser
            ));

            Zephy.Logger.Information("sending info back.");
            Zephy.serverSocket.SendPacket(ret, sender);
        }
    }

    public class AccessibleChannelsInfoPacket : Packet
    {
        public const int TYPE = 3001;

        public AccessibleChannelsInfoPacket(AccessibleChannelsInfoPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public AccessibleChannelsInfoPacket(byte[] packet)
            : base(packet) { }

        public AccessibleChannelsInfoPacketData Data
        {
            get { return ReadJsonObject<AccessibleChannelsInfoPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
