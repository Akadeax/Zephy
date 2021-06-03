using MongoDB.Bson.Serialization.IdGenerators;
using Server;
using Server.Database.Channel;
using Server.Database.Message;
using Server.Util;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.message
{
    public class SendMessageRequestPacketData : PacketData
    {
        public string forChannel;
        public string content;

        public SendMessageRequestPacketData(string forChannel, string content)
        {
            this.forChannel = forChannel;
            this.content = content;
        }
    }

    class SendMessageRequestPacketHandler : PacketHandler<SendMessageRequestPacket>
    {
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        protected override void Handle(SendMessageRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            // check if request is valid
            Channel channel = channelCrud.ReadOneById(data.forChannel);
            if(
                string.IsNullOrWhiteSpace(data.forChannel) || 
                string.IsNullOrWhiteSpace(data.content) || 
                channel == null)
            {
                SendError(HttpStatusCode.BadRequest, sender);
                return;
            }

            ActiveUser senderUser = ActiveUsers.GetUser(sender);
            if(senderUser == null || !channel.members.Contains(senderUser.userId))
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }


            // add new message to database
            var newMessage = new Message
            {
                content = data.content,
                sentAt = Util.ToUnixTimestamp(DateTime.Now),
                author = senderUser.userId,
                channel = channel._id,
            };
            messageCrud.CreateOne(newMessage);

            channelCrud.UpdateOne(channel._id, channel);

            // send response to members of channel (only if success!)
            var responsePacket = new SendMessageResponsePacket(new SendMessageResponsePacketData(
                (int)HttpStatusCode.OK, messageCrud.ReadOnePopulated(newMessage._id), channel._id
            ));

            foreach(string memberId in channel.members)
            {
                if(ActiveUsers.IsLoggedIn(memberId))
                {
                    Zephy.serverSocket.SendPacket(responsePacket, ActiveUsers.GetUser(memberId).clientSocket);
                }
            }
        }

        void SendError(HttpStatusCode code, Socket sender)
        {
            var err = new SendMessageResponsePacket(new SendMessageResponsePacketData(
                (int)code, null, null
            ));
            Zephy.serverSocket.SendPacket(err, sender);
        }
    }

    class SendMessageRequestPacket : Packet
    {
        public const int TYPE = 4003;

        public SendMessageRequestPacket(SendMessageRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public SendMessageRequestPacket(byte[] packet)
            : base(packet) { }

        public SendMessageRequestPacketData Data
        {
            get { return ReadJsonObject<SendMessageRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
