using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text;
using Server.database.Roles;

namespace Server
{
    public abstract class UserBase
    {
        public ObjectId _id;
        public string name;
        public string password;
    }

    public class User : UserBase
    {
        public List<ObjectId> roles = new List<ObjectId>();
    }
    public class PopulatedUser : UserBase
    {
        public List<Role> roles = new List<Role>();
    }
}
