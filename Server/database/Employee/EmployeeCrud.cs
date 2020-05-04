using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// This class is derived from the basic MongoCRUD and will Create Read Update Delete only Employee records
    /// </summary>
    public class EmployeeCrud : MongoCRUD<Employee>
    {
        public const string COLLECTION_NAME = "Employees";
        private RoleCache roleCache;

        public EmployeeCrud(string database)
            : base(database, COLLECTION_NAME)
        {
            roleCache = new RoleCache(database);
        }

        public void InsertEmployee(Employee record) 
        {
            InsertRecord(record);
        }

        public List<Employee> LoadEmployees()
        {
            return LoadRecords();
        }

        public PopulatedEmployee Populate(Employee employee)
        {
            List<Role> roles = new List<Role>();
            foreach(ObjectId id in employee.roles)
            {
                roles.Add(roleCache.GetRole(id));
            }

            return new PopulatedEmployee
            {
                name = employee.name,
                roles = roles,
                _id = employee._id
            };
        }

        /// <summary>
        /// Loads an employee by id
        /// </summary>
        /// <param name="id">the id to load the employee by</param>
        /// <returns>the Employee that was loaded by ID by this method</returns>
        public Employee LoadEmployee(ObjectId id)
        {
            var filter = Builders<Employee>.Filter.Eq("_id", id);
            return collection.Find(filter).First();
        }



        [Obsolete("Not optimized, use methods that adopt the caching approach")]
        public PopulatedEmployee LoadPopulatedEmployee(ObjectId id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .Match(filter).As<PopulatedEmployee>()
                .First();
        }

        [Obsolete("Not optimized, use methods that adopt the caching approach")]
        public List<PopulatedEmployee> LoadPopulatedEmployees()
        {
            return collection
                .Aggregate()
                .Lookup(RoleCrud.COLLECTION_NAME, "roles", "_id", "roles")
                .As<PopulatedEmployee>()
                .ToList();
        }
    }
}
