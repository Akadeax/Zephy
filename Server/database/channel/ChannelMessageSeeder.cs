using MongoDB.Bson;
using Server.database.message;
using Server.database.role;
using Server.database.user;

using System;
using System.Collections.Generic;
using System.Linq;
namespace Server.database.channel
{
    class ChannelMessageSeeder : MongoSeeder
    {
        UserCrud userCrud = new UserCrud();
        RoleCrud roleCrud = new RoleCrud();
        ChannelCrud channelCrud = new ChannelCrud();
        MessageCrud messageCrud = new MessageCrud();

        Random rnd = new Random();

        public override void Seed(SeederEntriesAmount amount)
        {
            if ((amount.messageSeederAmount < 1 || amount.userSeederAmount > MAX_SEED) ||
                (amount.channelSeederAmount < 1 || amount.channelSeederAmount > MAX_SEED))
                throw new ArgumentException("out of bounds", "amount");

            if (messageCrud.DocumentCount > 0 || channelCrud.DocumentCount > 0) return;


            int messagesPerChannel = (int)((float)amount.messageSeederAmount / amount.channelSeederAmount);
            for (int c = 0; c < amount.channelSeederAmount; c++)
            {
                List<ObjectId> channelRoles = RoleSeeder.GetRandomRoles(roleCrud, 2)
                    .Select(x => x._id)
                    .ToList();

                Channel newChannel = new Channel
                {
                    name = Faker.Lorem.Sentence(1),
                    description = Faker.Lorem.Sentence(1),
                    roles = channelRoles,
                    messages = new List<ObjectId>(),
                };

                List<User> canSeeChannel = UserUtil.GetUsersWithPermission(userCrud, channelRoles);

                for(int m = 0; m < messagesPerChannel; m++)
                {
                    if (canSeeChannel.Count == 0 || canSeeChannel == null) break;

                    ObjectId author = canSeeChannel[rnd.Next(canSeeChannel.Count)]._id;
                    Message msg = new Message
                    {
                        _id = ObjectId.GenerateNewId(),
                        content = Faker.Lorem.Sentence(20),
                        author = author,
                    };

                    newChannel.messages.Add(msg._id);
                    messageCrud.CreateOne(msg);
                }

                channelCrud.CreateOne(newChannel);
            }
        }
    }
}
