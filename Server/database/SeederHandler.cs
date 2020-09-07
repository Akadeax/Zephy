using Server.database.channel;
using Server.database.role;
using Server.database.user;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database
{
    public class SeederHandler
    {
        public static void Seed(SeederEntriesAmount amounts)
        {
            new RoleSeeder().Seed(amounts);
            new UserSeeder().Seed(amounts);
            new ChannelMessageSeeder().Seed(amounts);
        }
    }
    
    public class SeederEntriesAmount
    {
        public int roleSeederAmount = 5;
        public int userSeederAmount = 5;
        public int channelSeederAmount = 3;
        public int messageSeederAmount = 100;
    }
}
