using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.controllers
{
    public class RoleCrud : MongoCRUD
    {
        public long docAmount;

        public RoleCrud(string database) : base(database) 
        {
            docAmount = GetDocumentCount<long>("Roles");
        }

        public void InsertRole<RoleModel>(RoleModel record)
        {
            InsertRecord("Roles", record);
        }

        public List<T> LoadRoles<T>()
        {
            return LoadRecord<T>("Roles");
        }

        public T LoadRoleById<T>(ObjectId id)
        {
            return LoadRecordById<T>("Roles", id);
        }
    }
}
