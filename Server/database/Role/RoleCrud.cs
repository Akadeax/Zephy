using System;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Server.database.Roles
{
    public class RoleCrud : MongoCrud<Role>
    {
        public const string COLLECTION_NAME = "Roles";


        public RoleCrud(string database)
            : base(database, COLLECTION_NAME) { }

        public void InsertRole(Role record)
        {
            CreateRecord(record);
        }

        public List<Role> LoadRoles()
        {
            return ReadRecords();
        }

        public Role LoadRoleById(ObjectId id)
        {
            return ReadRecordById(id);
        }
    }
}
