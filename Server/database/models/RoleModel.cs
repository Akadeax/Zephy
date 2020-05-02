using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.models
{
    public class RoleModel
    {
        public ObjectId _id;
        public string name;
        public string description;
    }
}
