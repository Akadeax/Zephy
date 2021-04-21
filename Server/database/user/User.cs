using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace server.database.user
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string fullName;

        public string identifier;
        public string password;
    }
}
