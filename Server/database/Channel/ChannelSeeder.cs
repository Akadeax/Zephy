using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.Channels
{
    class ChannelSeeder
    {
        private readonly ChannelCrud channelCrud;

        public ChannelSeeder()
        {
            channelCrud = new ChannelCrud("Zephy");
        }

        public void Seed(int amountOfRoles)
        {
            if (channelCrud.DocumentCount > 0) return;

            for (var i = 0; i < amountOfRoles; i++)
            {
                channelCrud.Create(new Channel
                {
                    name = Faker.Company.Name(),
                    description = Faker.Lorem.Paragraph(5),
                });
            }
        }
    }
}
