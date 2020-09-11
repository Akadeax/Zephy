using MongoDB.Bson;
using Server;
using Server.database.channel;
using Server.database.message;
using Server.database.user;
using Server.utilities;
using Server.utilityData;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Packets.message
{
    public class MessageSendPacketData : PacketData
    {
        public string message;
        public string channel;
        public string author;

        public PopulatedMessage returnMessage;

        public MessageSendPacketData(string message, string channel, string author, PopulatedMessage returnMessage)
        {
            this.message = message;
            this.channel = channel;
            this.author = author;
            this.returnMessage = returnMessage;
        }
    }

    public class MessageSendPacketHandler : PacketHandler<MessageSendPacket>
    {
        readonly MessageCrud messageCrud = new MessageCrud();

        protected override void Handle(MessageSendPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            List<ActiveUser> users = UserUtilData.GetActiveInChannel(data.channel);

            string msgId = ObjectId.GenerateNewId().ToString();
            Message messageSent = new Message
            {
                _id = msgId,
                author = data.author,
                channel = data.channel,
                content = data.message,
                sentAt = Util.ToUnixTimestamp(DateTime.UtcNow),
            };

            messageCrud.CreateOne(messageSent);

            MessageSendPacket returnPacket = new MessageSendPacket(new MessageSendPacketData(
                data.message,
                data.channel,
                data.author,
                messageCrud.ReadOnePopulated(msgId)
            ));

            foreach(ActiveUser user in users)
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
