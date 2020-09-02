using MongoDB.Bson;
using Server.database.channel;
using Server.database.message;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Packets.message
{
    class PopulateMessagesPacketData
    {
        public ObjectId forChannel;
        public List<PopulatedMessage> populatedMessages;
        public int page;

        public PopulateMessagesPacketData(ObjectId forChannel, List<PopulatedMessage> populatedMessages, int page)
        {
            this.forChannel = forChannel;
            this.populatedMessages = populatedMessages;
            this.page = page;
        }
    }

    class PopulateMessagesPacketHandler : PacketHandler<PopulateMessagesPacket>
    {
        const int PAGE_SIZE = 50;

        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        protected override void Handle(PopulateMessagesPacket packet, Socket sender) 
        {
            PopulateMessagesPacketData data = packet.Data;

            Channel channel = channelCrud.ReadOneById(data.forChannel);

            List<PopulatedMessage> populatedMessages = new List<PopulatedMessage>();

            int lastIndex = channel.messages.Count - 1;
            int lowerBound = Math.Min(lastIndex, data.page * PAGE_SIZE);
            int upperBound = Math.Min(lastIndex, data.page * PAGE_SIZE + (PAGE_SIZE - 1));

            for(int i = lowerBound; i < upperBound; i++)
            {
                populatedMessages.Add(messageCrud.ReadOnePopulated(channel.messages[i]));
            }

            var returnPacket = new PopulateMessagesPacket(new PopulateMessagesPacketData(
                data.forChannel, populatedMessages, data.page
            ));

            Console.WriteLine(returnPacket.Data.populatedMessages.Count);

            sender.Send(returnPacket.Buffer);
        }
    }

    class PopulateMessagesPacket : Packet
    {
        public const int TYPE = 4000;

        public PopulateMessagesPacket(PopulateMessagesPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public PopulateMessagesPacket(byte[] packet)
            : base(packet) { }


        public PopulateMessagesPacketData Data
        {
            get { return ReadJsonObject<PopulateMessagesPacketData>(); }
            set { WriteJsonObject(value); }
        }

    }
}
