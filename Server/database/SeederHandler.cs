using Server.database.Roles;
using System;

namespace Server.database.seeders
{
    class SeederHandler
    {
        public UserSeeder es;
        public RoleSeeder rs;

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
