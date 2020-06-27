using System;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Server.database.Roles
{
    public class RoleCrud : MongoCrud<Role>
    {
        public const string COLLECTION_NAME = "Roles";


        public RoleCrud(string database) : base(database, COLLECTION_NAME)
        { 

        }

        public void Create(Role role)
        {
            CreateRecord(role);
        }

        public void Delete(ObjectId id)
        {
            DeleteRecord(id);
        }

        public void Update(ObjectId id, Role role)
        {
            UpdateRecord(id, role);
        }

        public Role ReadOne(ObjectId id)
        {
            return ReadRecordById(id);
        }

        public Role ReadOne(string name)
        {
            var filter = Builders<Role>.Filter.Eq("name", name);
            return collection.Find(filter).First();
        }

        public List<Role> ReadMany()
        {
            return ReadRecords();
        }
    }
}
