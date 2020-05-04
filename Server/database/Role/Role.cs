using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Role
    {
        public ObjectId _id;
        public string name;
        public string description;
    }
}
