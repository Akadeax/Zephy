using MongoDB.Bson;
using MongoDB.Driver;
using Server.Database.Message;
using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.Database.Channel
{
    public class ChannelCrud : MongoCrud<Channel>
    {
        public const string COLLECTION_NAME = "channels";

        public ChannelCrud() : base(COLLECTION_NAME) { }

        /// <summary>
        /// Returns whether any channel that has the same elements as 'members' exists.
        /// </summary>
        public bool Exists(List<string> members)
        {
            List<Channel> channels = ReadMany();
            foreach(var channel in channels)
            {
                if(new HashSet<string>(channel.members).SetEquals(members)) return true;
            }
            return false;
        }

        public PopulatedChannel ReadOnePopulated(string id)
        {
            return ReadOnePopulated(x => x._id == id);
        }
        public PopulatedChannel ReadOnePopulated(Expression<Func<Channel, bool>> filter = null)
        {
            return ReadManyPopulated(filter).FirstOrDefault();
        }

        public List<PopulatedChannel> ReadManyPopulated(Expression<Func<Channel, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            return collection
                .Aggregate()
                .Match(filter)
                .Lookup(MessageCrud.COLLECTION_NAME, "messages", "_id", "messages")
                .Lookup(UserCrud.COLLECTION_NAME, "members", "_id", "members")
                .As<PopulatedChannel>()
                .ToList();
        }

        public List<BaseChannelData> ReadManyBase(Expression<Func<Channel, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            List<Channel> fetchedChannels = ReadMany(filter);

            ChannelCrud channelCrud = new ChannelCrud();

            var baseChannels = new List<BaseChannelData>();
            foreach (Channel channel in fetchedChannels)
            {
                PopulatedChannel popChannel = channelCrud.ReadOnePopulated(channel._id);

                baseChannels.Add(channel.ToBaseChannelData(
                    popChannel?.messages.Count > 0
                        ? popChannel?.messages[0]
                        : null
                ));
            }

            return baseChannels;
        }
    }
}