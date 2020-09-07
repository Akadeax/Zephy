using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.user
{
    public class UserBase
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id;
        public string name;
        public string email;
        public string password;
    }

    public class User : UserBase
    {
        public List<string> roles;
    }
    public class PopulatedUser : UserBase
    {
        public List<Role> roles;
    }
}
