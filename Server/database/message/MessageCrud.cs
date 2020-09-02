using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.database.message
{
    class MessageCrud : MongoCrud<Message>
    {
        public const string COLLECTION_NAME = "messages";

        public MessageCrud() : base(COLLECTION_NAME) { }

        public PopulatedMessage ReadOnePopulated(ObjectId id)
        {
            return ReadOnePopulated(x => x._id == id);
        }
        public PopulatedMessage ReadOnePopulated(Expression<Func<Message, bool>> filter = null)
        {
            return ReadManyPopulated(filter).FirstOrDefault();
        }

        public List<PopulatedMessage> ReadManyPopulated(Expression<Func<Message, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            return collection
                .Aggregate()
                .Match(filter)
                .Lookup(UserCrud.COLLECTION_NAME, "author", "_id", "author")
                .Unwind("author")
                .As<PopulatedMessage>()
                .ToList();
        }
    }
}
