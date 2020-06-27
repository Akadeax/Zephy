using MongoDB.Bson.Serialization.Attributes;
using Server.database.Messages;
using Server.database.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.Channels
{
    public class ChannelBase
    {
        [BsonId]
        public string _id;

        public string name;

        public string description;

        public List<Role> roles = new List<Role>();
    }

    public class Channel : ChannelBase
    {
        public List<Message> messages = new List<Message>();
    }

    public class PopulatedChannel : ChannelBase
    {
        public List<PopulatedMessage> popMessages = new List<PopulatedMessage>();
    }
}
