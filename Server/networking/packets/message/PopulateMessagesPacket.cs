using MongoDB.Bson;
using Newtonsoft.Json;
using Server;
using Server.database.channel;
using Server.database.message;
using Server.utilityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Packets.message
{
    class PopulateMessagesPacketData : PacketData
    {
        public string forChannel;
        public int page;
        public string user;
        public List<PopulatedMessage> populatedMessages;

        public PopulateMessagesPacketData(string forChannel, string user, int page, List<PopulatedMessage> populatedMessages)
        {
            this.forChannel = forChannel;
            this.page = page;
            this.user = user;
            this.populatedMessages = populatedMessages;
        }
    }

    class PopulateMessagesPacketHandler : PacketHandler<PopulateMessagesPacket>
    {
        const int PAGE_SIZE = 25;

        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        protected override void Handle(PopulateMessagesPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            Channel channel = channelCrud.ReadOneById(data.forChannel);

            List<PopulatedMessage> populatedChannelMessages = messageCrud.ReadManyPopulatedSorted(x => x.channel == channel._id);

            int lowerBound = Math.Min(populatedChannelMessages.Count, data.page * PAGE_SIZE);
            int upperBound = Math.Min(populatedChannelMessages.Count, data.page * PAGE_SIZE + PAGE_SIZE);

            List<PopulatedMessage> paginatedMessages = new List<PopulatedMessage>();

            // TODO: DOESN'T INCLUDE LAST MESSAGE
            for (int i = lowerBound; i < upperBound; i++)
            {
                paginatedMessages.Add(populatedChannelMessages[i]);
            }

            var returnPacket = new PopulateMessagesPacket(new PopulateMessagesPacketData(
                data.forChannel, data.user, data.page,
                paginatedMessages
            ));

            Zephy.Logger.Information($"Sending message range {lowerBound} - {upperBound} of channel '{channel.name}' back.");

            if(data.page == 0 && UserUtilData.IsLoggedIn(data.user))
            {
                UserUtilData.GetUser(data.user).activeChannelId = data.forChannel;
            }

            sender.Send(returnPacket.Buffer);
        }
    }

    class PopulateMessagesPacket : Packet
    {
        public const int TYPE = 4000;

        public PopulateMessagesPacket(PopulateMessagesPacketData data) : base(TYPE, data)
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
