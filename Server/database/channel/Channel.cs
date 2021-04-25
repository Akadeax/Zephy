using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.Database.Message;
using Server.Database.User;
using System.Collections.Generic;

namespace Server.Database.Channel
{
    public class BaseChannelData
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string name;
    }

    public class Channel : BaseChannelData
    {
        public List<string> messages;
        public List<string> members;

        public BaseChannelData AsBaseChannelData
        {
            get
            {
                return new BaseChannelData
                {
                    _id = _id,
                    name = name,
                };
            }
        }
    }

    public class PopulatedChannel : BaseChannelData
    {
        public List<Message.Message> messages;
        public List<User.User> members;
    }
}
