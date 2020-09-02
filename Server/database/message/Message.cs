using MongoDB.Bson;
using Server.database.user;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.message
{
    public class MessageBase
    {
        public ObjectId _id;
        public string content;
        public int sentAt;
    }

    public class Message : MessageBase
    {
        public ObjectId author;
    }

    public class PopulatedMessage : MessageBase
    {
        public User author;
    }
}
