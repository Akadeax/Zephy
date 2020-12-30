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
    public static class ModifyChannelAction
    {
        public const int REMOVE_ROLE = 0;
        public const int ADD_ROLE = 1;
        public const int UPDATE_NAME = 2;
        public const int UPDATE_DESC = 3;
        public const int DELETE = 4;
    }

    public class ModifyChannelPacketData : PacketData
    {
        public int action;
        public string channel;
        public string data;

        public ModifyChannelPacketData(string channel, int action, string data)
        {
            this.action = action;
            this.channel = channel;
            this.data = data;
        }
    }

    public class ModifyChannelPacketHandler : PacketHandler<ModifyChannelPacket>
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        readonly UserUtil userUtil;

        public ModifyChannelPacketHandler()
        {
            userUtil = new UserUtil(userCrud, channelCrud);
        }

        protected override void Handle(ModifyChannelPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            Channel channel = channelCrud.ReadOneById(data.channel);
            if (channel == null) return;

            ActiveUser userSender = UserUtilData.GetUser(sender);
            if (!userUtil.UserCanViewChannel(userSender.userId, channel._id)) return;


            List<ActiveUser> canViewChannel = userUtil.GetActiveUsersThatCanView(channel);

            switch (data.action)
            {
                case ModifyChannelAction.ADD_ROLE:
                    if (channel.roles.Contains(data.data)) return;

                    channel.roles.Add(data.data);
                    channelCrud.UpdateOne(data.channel, channel);
                    break;
                case ModifyChannelAction.REMOVE_ROLE:
                    if (!channel.roles.Contains(data.data)) return;
                    channel.roles.Remove(data.data);
                    channelCrud.UpdateOne(data.channel, channel);
                    break;

                case ModifyChannelAction.UPDATE_NAME:
                    // TODO
                    if (channel.name == data.data) return;
                    channel.name = data.data;
                    channelCrud.UpdateOne(data.channel, channel);
                    channelCrud.ReadOneById(data.channel);
                    break;
                case ModifyChannelAction.UPDATE_DESC:
                    if (channel.description == data.data) return;
                    channel.description = data.data;
                    channelCrud.UpdateOne(data.channel, channel);
                    break;

                case ModifyChannelAction.DELETE:
                    channelCrud.DeleteOne(data.channel);
                    break;
                default:
                    return;
            }

            foreach (ActiveUser user in canViewChannel)
            {
                Zephy.serverSocket.SendPacket(packet, user.clientSocket);
            }
        }
    }

    public class ModifyChannelPacket : Packet
    {
        public const int TYPE = 3003;

        public ModifyChannelPacket(ModifyChannelPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyChannelPacket(byte[] packet)
            : base(packet) { }

        public ModifyChannelPacketData Data
        {
            get { return ReadJsonObject<ModifyChannelPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
