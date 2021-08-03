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
    public static class ChannelAction
    {
        public const int MODIFY_NAME = 0;
    }

    public class ModifyChannelRequestPacketData : PacketData
    {
        public string channel;
        public int action;
        public string data;

        public ModifyChannelRequestPacketData(string channel, int action, string data)
        {
            this.channel = channel;
            this.action = action;
            this.data = data;
        }
    }

    class ModifyChannelRequestPacketHandler : PacketHandler<ModifyChannelRequestPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(ModifyChannelRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            Channel channel = channelCrud.ReadOneById(data.channel);
            if(channel == null)
            {
                SendError(HttpStatusCode.NotFound, sender);
                return;
            }

            if(data.data.Length < 2 && data.data.Length > 32)
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }

            channel.name = data.data;
            channelCrud.UpdateOne(channel._id, channel);

            var response = new ModifyChannelResponsePacket(new ModifyChannelResponsePacketData(
                (int)HttpStatusCode.OK, channel._id, data.action, data.data
            ));
            foreach (string memberId in channel.members)
            {
                if (ActiveUsers.IsLoggedIn(memberId))
                {
                    Zephy.serverSocket.SendPacket(response, ActiveUsers.GetUser(memberId).clientSocket);
                }
            }
        }

        private void SendError(HttpStatusCode code, Socket sender)
        {
            var error = new ModifyChannelResponsePacket(new ModifyChannelResponsePacketData(
                (int)code, "", 0, ""
            ));
            Zephy.serverSocket.SendPacket(error, sender);
        }
    }

    class ModifyChannelRequestPacket : Packet
    {
        public const int TYPE = 3007;

        public ModifyChannelRequestPacket(ModifyChannelRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyChannelRequestPacket(byte[] packet)
            : base(packet) { }

        public ModifyChannelRequestPacketData Data
        {
            get { return ReadJsonObject<ModifyChannelRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
