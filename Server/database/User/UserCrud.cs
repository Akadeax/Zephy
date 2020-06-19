using MongoDB.Bson;
using MongoDB.Driver;
using Server.database.Roles;
using System;
using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// This class is derived from the basic MongoCRUD and will Create Read Update Delete only Employee records
    /// </summary>
    public class UserCrud : MongoCrud<User>
    {
        public const string COLLECTION_NAME = "Users";
        private RoleCache roleCache;

        public UserCrud(string database)
            : base(database, COLLECTION_NAME)
        {
            roleCache = new RoleCache(database);
        }

        public void InsertEmployee(User record) 
        {
            CreateRecord(record);
        }

        public List<User> LoadEmployees()
        {
            return ReadRecords();
        }

        public PopulatedUser Populate(User user)
        {
            List<Role> roles = new List<Role>();
            foreach(ObjectId id in user.roles)
            {
                roles.Add(roleCache.GetRole(id));
            }

            return new PopulatedUser
            {
                name = user.name,
                roles = roles,
                _id = user._id
            };
        }

        /// <summary>
        /// Loads an employee by id
        /// </summary>
        /// <param name="id">the id to load the employee by</param>
        /// <returns>the Employee that was loaded by ID by this method</returns>
        public User LoadUser(ObjectId id)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            return collection.Find(filter).First();
        }



        [Obsolete("Not optimized, use methods that adopt the caching approach")]
        public PopulatedUser LoadPopulatedUser(ObjectId id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .Match(filter).As<PopulatedUser>()
                .First();
        }

        [Obsolete("Not optimized, use methods that adopt the caching approach")]
        public List<PopulatedUser> LoadPopulatedEmployees()
        {
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .As<PopulatedUser>()
                .ToList();
        }
    }
}
