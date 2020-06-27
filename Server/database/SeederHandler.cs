using Server.database.Roles;
using System;

namespace Server.database.seeders
{
    class SeederHandler
    {
        public RoleSeeder rs;
        public UserSeeder es;

        public SeederHandler()
        {
            rs = new RoleSeeder();
            es = new UserSeeder();
        }

        public void Start(int entrees)
        {
            rs.Seed(entrees);
            es.Seed(entrees);
        }
    }
}
