using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.message;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.database.channel
{
    public class ChannelCrud : MongoCrud<Channel>
    {
        public const string COLLECTION_NAME = "channels";

        public ChannelCrud() : base(COLLECTION_NAME) { }

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
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .Lookup(MessageCrud.COLLECTION_NAME, "messages", "_id", "messages")
                .As<PopulatedChannel>()
                .ToList();
        }

        public List<BaseChannelData> ReadManyBase(Expression<Func<Channel, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            List<Channel> fetchedChannels = ReadMany(filter);

            var baseChannels = new List<BaseChannelData>();
            foreach(Channel channel in fetchedChannels)
            {
                baseChannels.Add(channel.AsBaseChannelData);
            }

            return baseChannels;
        }
    }
}
