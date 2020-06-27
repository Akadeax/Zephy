using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.Roles;
using System;
using System.Collections.Generic;

namespace Server
{
    public class UserCrud : MongoCrud<User>
    {
        public const string COLLECTION_NAME = "Users";

        public UserCrud(string database) : base(database, COLLECTION_NAME)
        {

        }

        public PopulatedUser ReadOnePopulated(ObjectId id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .Match(filter).As<PopulatedUser>()
                .First();
        }

        public List<PopulatedUser> ReadManyPopulated()
        {
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .As<PopulatedUser>()
                .ToList();
        }
    }
}
