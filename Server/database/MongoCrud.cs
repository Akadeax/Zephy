using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Server.Database
{
    /// <summary>
    /// base class for all MongoDB interaction in the app.
    /// </summary>
    public abstract class MongoCrud<T>
    {
        protected const string DATABASE = "Zephy";

        protected IMongoDatabase db;
        protected IMongoCollection<T> collection;

        public MongoCrud(string table)
        {
            var client = new MongoClient("mongodb://mongo");
            db = client.GetDatabase(DATABASE);
            collection = db.GetCollection<T>(table);
        }

        public void CreateOne(T record)
        {
            collection.InsertOne(record);
        }

        public List<T> ReadMany()
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        public List<T> ReadMany(Expression<Func<T, bool>> filter)
        {
            if (filter == null) return ReadMany();
            return collection.Find(filter).ToList();
        }

        public T ReadOneById(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return collection.Find(filter).FirstOrDefault();
        }

        public T ReadOne(Expression<Func<T, bool>> filter = null)
        {
            if (DocumentCount == 0) return default;
            // if filter isn't specified, use filter that allows everything
            if (filter == null) filter = x => true;

            var foundList = collection.Find(filter);
            if (foundList.CountDocuments() == 0) return default;
            return foundList.FirstOrDefault();
        }

        public void UpdateOne(string id, T record)
        {
            collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true }
            );
        }

        public void DeleteOne(string id)
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
