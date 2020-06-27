using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Server
{
    public abstract class MongoCrud<T>
    {
        protected IMongoDatabase db;
        protected IMongoCollection<T> collection;

        public MongoCrud(string database, string table)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
            collection = db.GetCollection<T>(table);
        }

        public void CreateRecord(T record)
        {
            collection.InsertOne(record);
        }

        public List<T> ReadRecords()
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        public T ReadRecordById(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filter).First();
        }

        public T ReadRecord(Expression<Func<T, bool>> filter = null)
        {
            if (DocumentCount == 0) return default;

            var foundList = collection.Find(filter);
            if (foundList.CountDocuments() == 0) return default;
            return foundList.First();
        }

        public void UpdateRecord(ObjectId id, T record)
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
