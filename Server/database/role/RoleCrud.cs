using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.role
{
    class RoleCrud : MongoCrud<Role>
    {
        public const string COLLECTION_NAME = "roles";

        public RoleCrud() : base(COLLECTION_NAME) { }
    }
}
