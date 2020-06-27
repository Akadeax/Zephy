using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.Messages;
using System.Collections.Generic;

namespace Server.database.Channels
{
    class ChannelCrud : MongoCrud<Channel>
    {
        public const string COLLECTION_NAME = "channels";

        public ChannelCrud(string database) : base(database, COLLECTION_NAME)
        {

        }

        public PopulatedChannel ReadOnePopulated(ObjectId id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            return collection
                .Aggregate()
                .Lookup(MessageCrud.COLLECTION_NAME, "channel", "_id", "channel")
                .Match(filter).As<PopulatedChannel>()
                .First();
        }

        public List<PopulatedChannel> ReadManyPopulated()
        {
            return collection
                .Aggregate()
                .Lookup(MessageCrud.COLLECTION_NAME, "channel", "_id", "channel")
                .As<PopulatedChannel>()
                .ToList();
        }
    }
}
