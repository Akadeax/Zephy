﻿using MongoDB.Bson;
using MongoDB.Driver;
using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Server.Database.Message
{
    class MessageCrud : MongoCrud<Message>
    {
        public const string COLLECTION_NAME = "messages";

        public MessageCrud() : base(COLLECTION_NAME) 
        {
            InitIndexes();
        }

        async void InitIndexes()
        {
            var notificationLogBuilder = Builders<Message>.IndexKeys;
            var indexModel = new CreateIndexModel<Message>(notificationLogBuilder.Descending(x => x.sentAt));
            await collection.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false);
        }

        public PopulatedMessage ReadOnePopulated(string id)
        {
            return ReadOnePopulated(x => x._id == id);
        }

        public List<PopulatedMessage> ReadManyPaginated(string channel, int page, int pageSize)
        {
            return collection
                .Aggregate()
                .Match(x => x.channel == channel)
                .SortByDescending(x => x.sentAt)
                .Lookup(UserCrud.COLLECTION_NAME, "author", "_id", "author")
                .Unwind("author")
                .As<PopulatedMessage>()
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToList();

        }

        public PopulatedMessage ReadOnePopulated(Expression<Func<Message, bool>> filter = null)
        {
            return ReadManyPopulated(filter).FirstOrDefault();
        }

        public List<PopulatedMessage> ReadManyPopulated(Expression<Func<Message, bool>> filter = null)
        {
            if (filter == null) filter = x => true;

            return collection
                .Aggregate()
                .Match(filter)
                .Lookup(UserCrud.COLLECTION_NAME, "author", "_id", "author")
                .Unwind("author")
                .As<PopulatedMessage>()
                .ToList();
        }
    }
}
