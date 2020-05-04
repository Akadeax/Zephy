using System;

namespace Server.database.seeders
{
    class SeederHandler
    {
        public EmployeeSeeder es;
        public RoleSeeder rs;

        public SeederHandler()
        {
            rs = new RoleSeeder();
            es = new EmployeeSeeder();
        }

        public void Start(int entrees)
        {
            rs.Seed(entrees);
            es.Seed(entrees);
        }
    }
}
