using MongoDB.Bson;
using Server.database.message;
using Server.database.role;
using Server.database.user;
using Server.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Server.database.channel
{
    class ChannelMessageSeeder : MongoSeeder
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly RoleCrud roleCrud = new RoleCrud();
        readonly ChannelCrud channelCrud = new ChannelCrud();
        readonly MessageCrud messageCrud = new MessageCrud();

        readonly UserUtil userUtil;

        readonly Random rnd = new Random();

        public ChannelMessageSeeder()
        {
            userUtil = new UserUtil(userCrud, channelCrud);
        }


        public override void Seed(SeederEntriesAmount amount)
        {
            if ((amount.messageSeederAmount < 1 || amount.userSeederAmount > MAX_SEED) ||
                (amount.channelSeederAmount < 1 || amount.channelSeederAmount > MAX_SEED))
                throw new ArgumentException("out of bounds", "amount");

            if (messageCrud.DocumentCount > 0 || channelCrud.DocumentCount > 0) return;


            int messagesPerChannel = (int)((float)amount.messageSeederAmount / amount.channelSeederAmount);
            for (int c = 0; c < amount.channelSeederAmount; c++)
            {
                List<string> channelRoles = RoleSeeder.GetRandomRoles(roleCrud, 2)
                    .Select(x => x._id)
                    .ToList();

                channelRoles.Add(RoleCrud.COLLECTION_NAME);

                Channel newChannel = new Channel
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    name = Faker.Lorem.Sentence(1),
                    description = Faker.Lorem.Sentence(1),
                    roles = channelRoles,
                    messages = new List<string>(),
                };

                List<User> canSeeChannel = userUtil.GetUsersThatCanView(newChannel);

                for(int m = 0; m < messagesPerChannel; m++)
                {
                    if (canSeeChannel.Count == 0 || canSeeChannel == null) break;

                    string author = canSeeChannel[rnd.Next(canSeeChannel.Count)]._id;
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
