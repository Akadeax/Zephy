using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.seeders
{
    class Seeder
    {
        public EmployeeSeeder es;
        public RoleSeeder rs;

        public Seeder()
        {
            es = new EmployeeSeeder();
            rs = new RoleSeeder();
        }

        public void Start(int entrees)
        {
            rs.Seed(entrees);
            es.Seed(entrees);
        }
    }
}
