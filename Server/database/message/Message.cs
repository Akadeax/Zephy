using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.database.channel;
using Server.database.user;

namespace Server.database.message
{
    public class MessageBase
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string content;
        public int sentAt;
        public string channel;
    }

    public class Message : MessageBase
    {
        public string author;
    }

    public class PopulatedMessage : MessageBase
    {
        public User author;
    }
}
