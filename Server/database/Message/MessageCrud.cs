using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.Messages
{
    class MessageCrud : MongoCrud<Message>
    {
        public const string COLLECTION_NAME = "messages";

        public MessageCrud(string database) : base (database, COLLECTION_NAME)
        {

        }

        public void Create(Message message)
        {
            CreateRecord(message);
        }

        public void Update(ObjectId id, Message newMessage)
        {
            UpdateRecord(id, newMessage);
        }

        public void Delete(ObjectId id)
        {
            DeleteRecord(id);
        }

        public Message ReadOne(ObjectId id)
        {
            return ReadRecordById(id);
        }

        public PopulatedMessage ReadOnePopulated(ObjectId id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            return collection
                .Aggregate()
                .Lookup(ChannelCrud.COLLECTION_NAME, "channels", "_id", "channels")
                .Match(filter).As<PopulatedMessage>()
                .First();
        }

        public List<Message> ReadMany()
        {
            return ReadRecords();
        }

        public List<PopulatedMessage> ReadManyPopulated()
        {
            return collection
                .Aggregate()
                .Lookup(ChannelCrud.COLLECTION_NAME, "channels", "_id", "channels")
                .As<PopulatedMessage>()
                .ToList();
        }
    }
}
