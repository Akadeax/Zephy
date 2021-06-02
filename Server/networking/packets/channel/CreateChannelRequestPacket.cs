using Server;
using Server.Database.Channel;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using Server.Util;

namespace Packets.channel
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
        readonly ChannelCrud channelCrud = new ChannelCrud();

        protected override void Handle(CreateChannelRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            // check whether request is valid
            if(data.withMembers.Count <= 1 || Util.HasDuplicates(data.withMembers))
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }

            ActiveUser user = ActiveUsers.GetUser(sender);
            if(user == null)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            if(channelCrud.Exists(data.withMembers))
            {
                SendError(HttpStatusCode.Conflict, sender);
                return;
            }

            // create actual channel
            var newChannel = new Channel()
            {
                name = data.name,
                members = data.withMembers,
                messages = new List<string>(),
            };
            channelCrud.CreateOne(newChannel);

            var successResponse = new CreateChannelResponsePacket(new CreateChannelResponsePacketData(
                (int)HttpStatusCode.OK,
                newChannel
            ));
            Zephy.serverSocket.SendPacket(successResponse, sender);
        }

        private void SendError(HttpStatusCode code, Socket sender)
        {
            var errResponse = new CreateChannelResponsePacket(new CreateChannelResponsePacketData(
                (int)code,
                null
            ));
            Zephy.serverSocket.SendPacket(errResponse, sender);
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
