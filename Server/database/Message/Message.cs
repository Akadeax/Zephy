using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Server.database.Channels;

namespace Server.database.Messages
{
    public class MessageBase
    {
        [BsonId]
        public string _id;

        public string message;
    }

    public class Message : MessageBase
    {
        public ObjectId channel;
    }

    public class PopulatedMessage: MessageBase
    {
        public Channel channel;
    }
}
