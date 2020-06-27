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

    }
}
