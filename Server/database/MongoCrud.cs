using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Server
{
    public abstract class MongoCRUD<T>
    {
        protected IMongoDatabase db;
        protected IMongoCollection<T> collection;
        private string database;

        public MongoCRUD(string database, string table)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
            collection = db.GetCollection<T>(table);
        }

        public void InsertRecord(T record)
        {
            collection.InsertOne(record);
        }

        public List<T> LoadRecords()
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filter).First();
        }

        public void UpsertRecord(ObjectId id, T record)
        {
            collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true }
            );
        }

        public void DeleteRecord(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }

        public long DocumentCount
        {
            get
            {
                return collection.EstimatedDocumentCount();
            }
        }

        public void CloseConnection()
        {
            db = null;
        }
    }
}
