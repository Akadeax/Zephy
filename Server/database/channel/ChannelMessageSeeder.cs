using MongoDB.Bson;
using server.database.message;
using server.database.user;
using server.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace server.database.channel
{
    class ChannelMessageSeeder : MongoSeeder
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        readonly Random rnd = new Random();

        public override void Seed(SeederEntriesAmount amount)
        {
            if ((amount.messageSeederAmount < 1 || amount.userSeederAmount > MAX_SEED) ||
                (amount.channelSeederAmount < 1 || amount.channelSeederAmount > MAX_SEED))
                throw new ArgumentException("out of bounds", "amount");

            if (messageCrud.DocumentCount > 0 || channelCrud.DocumentCount > 0) return;


            int messagesPerChannel = (int)((float)amount.messageSeederAmount / amount.channelSeederAmount);

            List<User> users = userCrud.ReadMany();

            for(int i = 0; i < amount.channelSeederAmount; i++)
            {
                Channel newChannel = new Channel
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    name = Faker.Lorem.Sentence(1),
                    messages = new List<string>(),
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
                    int rndTimestamp = Util.RandTimestamp();
                    Message msg = new Message
                    {
                        _id = ObjectId.GenerateNewId().ToString(),
                        content = Faker.Lorem.Sentence(20),
                        author = author,
                        channel = newChannel._id,
                        sentAt = rndTimestamp,
                    };

                    newChannel.messages.Add(msg._id);
                    messageCrud.CreateOne(msg);
                }

                channelCrud.CreateOne(newChannel);
            }
        }
    }
}