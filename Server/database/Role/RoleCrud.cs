using System;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Server
{
    public class RoleCrud : MongoCRUD<Role>
    {
        public const string COLLECTION_NAME = "Roles";


        public RoleCrud(string database)
            : base(database, COLLECTION_NAME) { }

        public void InsertRole(Role record)
        {
            InsertRecord(record);
        }

        public List<Role> LoadRoles()
        {
            return LoadRecords();
        }

        public Role LoadRoleById(ObjectId id)
        {
            return LoadRecordById(id);
        }
    }
}
