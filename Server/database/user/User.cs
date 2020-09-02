using MongoDB.Bson;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.user
{
    public class UserBase
    {
        public ObjectId _id;
        public string name;
        public string email;
        public string password;
    }

    public class User : UserBase
    {
        public List<ObjectId> roles;
    }
    public class PopulatedUser : UserBase
    {
        public List<Role> roles;
    }
}
