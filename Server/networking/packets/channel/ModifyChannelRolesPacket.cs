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
    public static class ModifyChannelRolesAction
    {
        public const int REMOVE = 0;
        public const int ADD = 1;
    }

    public class ModifyChannelRolesPacketData : PacketData
    {
        public int action;
        public string channel;
        public string role;

        public ModifyChannelRolesPacketData(string channel, string role, int action)
        {
            this.action = action;
            this.channel = channel;
            this.role = role;
        }
    }

    public class ModifyChannelRolesPacketHandler : PacketHandler<ModifyChannelRolesPacket>
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        readonly UserUtil userUtil;

        public ModifyChannelRolesPacketHandler()
        {
            userUtil = new UserUtil(userCrud, channelCrud);
        }

        protected override void Handle(ModifyChannelRolesPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            Channel channel = channelCrud.ReadOneById(data.channel);
            if (channel == null) return;

            ActiveUser userSender = UserUtilData.GetUser(sender);
            if (!userUtil.UserCanViewChannel(userSender.userId, channel._id)) return;

            if (!channel.roles.Contains(data.role)) return;

            List<ActiveUser> canViewChannel = userUtil.GetActiveUsersThatCanView(channel);

            switch(data.action)
            {
                case ModifyChannelRolesAction.ADD:
                    if (channel.roles.Contains(data.role)) return;

                    channel.roles.Add(data.role);
                    channelCrud.UpdateOne(data.channel, channel);
                    // TODO: Tell all users affected by the role add to reload accessible channels?
                    break;
                case ModifyChannelRolesAction.REMOVE:
                    if (!channel.roles.Contains(data.role)) return;
                    channel.roles.Remove(data.role);
                    channelCrud.UpdateOne(data.channel, channel);
                    // TODO: Deny all users affected by the role change permission to the respective channels?
                    break;
                default:
                    return;
            }

            foreach(ActiveUser user in canViewChannel)
            {
                Zephy.serverSocket.SendPacket(packet, user.clientSocket);
            }
        }
    }

    public class ModifyChannelRolesPacket : Packet
    {
        public const int TYPE = 3003;

        public ModifyChannelRolesPacket(ModifyChannelRolesPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyChannelRolesPacket(byte[] packet)
            : base(packet) { }

        public ModifyChannelRolesPacketData Data
        {
            get { return ReadJsonObject<ModifyChannelRolesPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
