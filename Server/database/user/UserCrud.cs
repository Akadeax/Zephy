using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace server.database.user
{
    public class UserCrud : MongoCrud<User>
    {
        public const string COLLECTION_NAME = "users";

        public UserCrud() : base(COLLECTION_NAME) { }
    }
}
