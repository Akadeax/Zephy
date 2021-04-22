using Server.Database.Channel;
using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Database
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
