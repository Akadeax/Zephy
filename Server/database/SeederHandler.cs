using server.database.channel;
using server.database.user;
using System;
using System.Collections.Generic;
using System.Text;

namespace server.database
{
    public class SeederHandler
    {
        public static void Seed(SeederEntriesAmount amounts)
        {
            new UserSeeder().Seed(amounts);
            new ChannelMessageSeeder().Seed(amounts);
        }
    }
    
    public class SeederEntriesAmount
    {
        public int userSeederAmount;
        public int channelSeederAmount;
        public int messageSeederAmount;
    }
}
