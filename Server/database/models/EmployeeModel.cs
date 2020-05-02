using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Server.models
{
    public class EmployeeModel
    {
        public ObjectId _id;
        public List<ObjectId> roles = new List<ObjectId>();
        public string name;
    }

    public class EmployeePopulated
    {
        public ObjectId _id;
        public List<RoleModel> roles = new List<RoleModel>();
        public string name;
    }
}
