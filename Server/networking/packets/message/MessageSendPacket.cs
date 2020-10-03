using MongoDB.Bson;
using Server;
using Server.database.channel;
using Server.database.message;
using Server.database.user;
using Server.utilities;
using Server.utilityData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;

namespace Packets.message
{
    public class MessageSendPacketData : PacketData
    {
        public string message;
        public string channel;

        public PopulatedMessage returnMessage;

        public MessageSendPacketData(string message, string channel, PopulatedMessage returnMessage)
        {
            this.message = message;
            this.channel = channel;
            this.returnMessage = returnMessage;
        }
    }

    public class MessageSendPacketHandler : PacketHandler<MessageSendPacket>
    {
        readonly MessageCrud messageCrud = new MessageCrud();
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();

        readonly UserUtil userUtil;

        public MessageSendPacketHandler()
        {
            userUtil = new UserUtil(userCrud, channelCrud);
        }

        protected override void Handle(MessageSendPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            ActiveUser author = UserUtilData.GetUser(sender);
            if (!userUtil.UserCanViewChannel(author.userId, data.channel)) return;

            List<ActiveUser> activeChannelUsers = UserUtilData.GetActiveInChannel(data.channel);
            Console.WriteLine(activeChannelUsers.Count);
            string msgId = ObjectId.GenerateNewId().ToString();
            Message messageSent = new Message
            {
                _id = msgId,
                author = author.userId,
                channel = data.channel,
                content = data.message,
                sentAt = Util.ToUnixTimestamp(DateTime.UtcNow),
            };

            messageCrud.CreateOne(messageSent);

            MessageSendPacket returnPacket = new MessageSendPacket(new MessageSendPacketData(
                data.message,
                data.channel,
                messageCrud.ReadOnePopulated(msgId)
            ));

            foreach(ActiveUser user in activeChannelUsers)
            {
                if(user.clientSocket.Connected)
                {
                    user.clientSocket.Send(returnPacket.Buffer);
                }
            }
        }
    }

    public class MessageSendPacket : Packet
    {
        public const int TYPE = 4001;

        public MessageSendPacket(MessageSendPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public MessageSendPacket(byte[] packet)
            : base(packet) { }

        public MessageSendPacketData Data
        {
            get { return ReadJsonObject<MessageSendPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
