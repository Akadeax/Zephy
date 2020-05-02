using MongoDB.Bson;
using MongoDB.Driver;
using Server.exceptions;
using Server.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// This class is derived from the basic MongoCRUD and will Create Read Update Delete only Employee records
    /// </summary>
    public class EmployeeCrud : MongoCRUD
    {
        // Hold the amount of documents in this collection
        public long docAmount;
        private string collectionName = "Employees";

        public EmployeeCrud(string database) : base(database) 
        {
            docAmount = GetDocumentCount<long>(collectionName);
            Console.WriteLine(collectionName);
        }

        public void InsertEmployee<EmployeeModel>(EmployeeModel record) 
        {
            InsertRecord(collectionName, record);
        }

        public List<T> LoadEmployees<T>()
        {
            return LoadRecord<T>(collectionName);
        }

        public T LoadEmployeeById<T>(ObjectId id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);

            return collection.Find(filter).First();
        }

        public T LoadPopulatedEmployeeById<T>(ObjectId id)
        {
            try
            {
                var collection = db.GetCollection<T>(collectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

                var result = collection.Aggregate().Lookup("Roles", "roles", "_id", "roles").Match(filter).As<T>().First();

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidIDException("The provided Id returned no results, ya probably didn't change the default ID YA CUNT");
            }
        }
    }
}
