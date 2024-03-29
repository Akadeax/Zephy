﻿using MongoDB.Bson;
using Server.Database.Message;
using Server.Database.User;
using Server.Util;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Server.Database.Channel
{
    class ChannelMessageSeeder : MongoSeeder
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        readonly Random rnd = new Random();

        public override void Seed(SeederEntriesAmount amount)
        {
            if ((amount.messageSeederAmount < 0 || amount.userSeederAmount > MAX_SEED) ||
                (amount.channelSeederAmount < 0 || amount.channelSeederAmount > MAX_SEED))
                throw new ArgumentException("out of bounds", "amount");

            if (messageCrud.DocumentCount > 0 || channelCrud.DocumentCount > 0) return;


            int messagesPerChannel = (int)((float)amount.messageSeederAmount / amount.channelSeederAmount);

            List<User.User> users = userCrud.ReadMany();

            for(int i = 0; i < amount.channelSeederAmount; i++)
            {
                Channel newChannel = new Channel
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    name = Faker.Internet.DomainWord(),
                    members = new List<string>(),
                };

                const int CHANNEL_MEMBER_COUNT = 2;

                for (int j = 0; j < CHANNEL_MEMBER_COUNT; j++)
                {
                    string rndUserId;
                    do
                    {
                        rndUserId = users[rnd.Next(users.Count)]._id;
                    } while (newChannel.members.Contains(rndUserId));

                    newChannel.members.Add(rndUserId);
                }


                for (int m = 0; m < messagesPerChannel; m++)
                {
                    if (newChannel.members.Count == 0) break;

                    string author = newChannel.members[rnd.Next(newChannel.members.Count)];
                    int rndTimestamp = Util.Util.RandTimestamp();
                    Message.Message msg = new Message.Message
                    {
                        _id = ObjectId.GenerateNewId().ToString(),
                        content = rnd.Next(1, 10) < 8 ? Faker.Lorem.Sentence(20) : "ok",
                        author = author,
                        sentAt = rndTimestamp,
                        channel = newChannel._id
                    };

                    messageCrud.CreateOne(msg);
                }

                channelCrud.CreateOne(newChannel);
            }
        }
    }
}