using Server;
using Server.Database.Channel;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using Server.Util;
using Server.Database.User;

namespace Packets.channel
{
    public static class MemberAction
    {
        public const int ADD_MEMBER = 0;
        public const int REMOVE_MEMBER = 1;
    }

    public class ModifyMembersRequestPacketData : PacketData
    {
        public int action;
        public string channel;
        public string user;

        public ModifyMembersRequestPacketData(int action, string channel, string user)
        {
            this.action = action;
            this.channel = channel;
            this.user = user;
        }
    }

    class ModifyMembersRequestPacketHandler : PacketHandler<ModifyMembersRequestPacket>
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(ModifyMembersRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            ListedUser user = userCrud.ReadOneListedById(data.user);
            Channel channel = channelCrud.ReadOneById(data.channel);
            if(user == null || channel == null)
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }

            if(data.action == MemberAction.ADD_MEMBER)
            {
                if(channel.members.Contains(user._id))
                {
                    SendError(HttpStatusCode.Conflict, sender);
                    return;
                }
                channel.members.Add(user._id);
                channelCrud.UpdateOne(channel._id, channel);
            }
            else if(data.action == MemberAction.REMOVE_MEMBER)
            {
                if (!channel.members.Contains(user._id))
                {
                    SendError(HttpStatusCode.NotFound, sender);
                    return;
                }
                channel.members.Remove(user._id);
                channelCrud.UpdateOne(channel._id, channel);
            }


            var response = new ModifyMembersResponsePacket(new ModifyMembersResponsePacketData(
                (int)HttpStatusCode.OK, user, channel._id, data.action
            ));

            if (ActiveUsers.IsLoggedIn(data.user))
            {
                Zephy.serverSocket.SendPacket(response, ActiveUsers.GetUser(data.user).clientSocket);
            }

            foreach (string memberId in channel.members)
            {
                if(ActiveUsers.IsLoggedIn(memberId))
                {
                    Zephy.serverSocket.SendPacket(response, ActiveUsers.GetUser(memberId).clientSocket);
                }
            }
        }

        private void SendError(HttpStatusCode code, Socket sender)
        {
            var errResponse = new ModifyMembersResponsePacket(new ModifyMembersResponsePacketData(
                (int)code,
                null,
                "",
                -1
            ));
            Zephy.serverSocket.SendPacket(errResponse, sender);
        }
    }

    class ModifyMembersRequestPacket : Packet
    {
        public const int TYPE = 3005;

        public ModifyMembersRequestPacket(ModifyMembersRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyMembersRequestPacket(byte[] packet)
            : base(packet) { }

        public ModifyMembersRequestPacketData Data
        {
            get { return ReadJsonObject<ModifyMembersRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
