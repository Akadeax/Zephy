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
    public class AccessibleChannelsInfoPacketData
    {
        public List<BaseChannelData> accessibleChannelsData;
        public ObjectId forUser;

        public AccessibleChannelsInfoPacketData(List<BaseChannelData> accessibleChannelsData, ObjectId forUser)
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
            Zephy.Logger.Information("Received Request for AccessibleChannelsData");

            var data = packet.Data;

            PopulatedUser forUser = userCrud.ReadOnePopulated(data.forUser);
            var accessibleChannels = new List<BaseChannelData>();
            if (forUser != null)
            {
                foreach (Channel currChannel in channelCrud.ReadMany())
                {
                    foreach (ObjectId currUserRole in forUser.roles.Select(x => x._id))
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

        public AccessibleChannelsInfoPacket(AccessibleChannelsInfoPacketData data) : base(TYPE)
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
