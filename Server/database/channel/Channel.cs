using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.database.message;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.channel
{
    public class BaseChannelData
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string name;
        public string description;
    }

    public class Channel : BaseChannelData
    {
        public List<string> roles;
        public List<string> messages;

        public BaseChannelData AsBaseChannelData
        {
            get
            {
                return new BaseChannelData
                {
                    _id = _id,
                    name = name,
                    description = description,
                };
            }
        }
    }

    public class PopulatedChannel : BaseChannelData
    {
        public List<Role> roles;
        public List<Message> messages;
    }
}
