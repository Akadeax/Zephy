using MongoDB.Bson;
using Server.database.message;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.channel
{
    public class BaseChannelData
    {
        public ObjectId _id;
        public string name;
        public string description;
    }

    public class Channel : BaseChannelData
    {
        public List<ObjectId> roles;
        public List<ObjectId> messages;

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
