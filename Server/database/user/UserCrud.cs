using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.database.user
{
    public class UserCrud : MongoCrud<User>
    {
        public const string COLLECTION_NAME = "users";

        public UserCrud() : base(COLLECTION_NAME) { }

        public PopulatedUser ReadOnePopulated(string id)
        {
            return ReadOnePopulated(x => x._id == id);
        }
        public PopulatedUser ReadOnePopulated(Expression<Func<User, bool>> filter = null)
        {
            return ReadManyPopulated(filter).FirstOrDefault();
        }

        public List<PopulatedUser> ReadManyPopulated(Expression<Func<User, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            return collection
                .Aggregate()
                .Match(filter)
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .As<PopulatedUser>()
                .ToList();
        }
    }
}
