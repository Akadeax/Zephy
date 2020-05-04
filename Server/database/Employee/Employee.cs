using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public abstract class EmployeeBase
    {
        public ObjectId _id;
        public string name;
    }

    public class Employee : EmployeeBase
    {
        public List<ObjectId> roles = new List<ObjectId>();
    }
    public class PopulatedEmployee : EmployeeBase
    {
        public List<Role> roles = new List<Role>();
    }
}
