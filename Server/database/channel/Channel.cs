using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.Database.Message;
using Server.Database.User;
using System.Collections.Generic;

namespace Server.Database.Channel
{
    public class BaseChannel
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string name;
    }

    public class BaseChannelData : BaseChannel
    {
        public Message.Message lastMessage;
    }

    public class Channel : BaseChannel
    {
        public List<string> members;

        public BaseChannelData ToBaseChannelData(Message.Message lastMessage)
        {
            return new BaseChannelData
            {
                _id = _id,
                name = name,
                lastMessage = lastMessage
            };
        }
    }

    public class PopulatedChannel : BaseChannelData
    {
        public List<Message.Message> messages;
        public List<User.User> members;
    }
}
