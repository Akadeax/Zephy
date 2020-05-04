using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Server
{
    class EmployeeSeeder
    {
        private EmployeeCrud employeeCrud;
        private RoleCache roleCache;

        public EmployeeSeeder()
        {
            employeeCrud = new EmployeeCrud("Zephy");
            roleCache = new RoleCache("Zephy");
        }

        public void Seed(int amountOfEmployees)
        {
            if (employeeCrud.DocumentCount > 0) return;

            for (var i = 0; i < amountOfEmployees; i++)
            {
                employeeCrud.InsertEmployee(new Employee
                {
                    roles = GetRandomRoles(),
                    name = Faker.Name.FullName()
                });
            }
        }

        public List<ObjectId> GetRandomRoles()
        {
            // init roles (if roleCache is empty load it)
            List<Role> roles;
            if(roleCache.Empty)
            {
                roleCache.ReloadCache();
            }
            roles = roleCache.GetRoles();

            List<ObjectId> newRoles = new List<ObjectId>();

            Random rnd = new Random();
            int randRolesAmount = rnd.Next(1, 5);

            for(int i = 0; i < randRolesAmount; i++)
            {
                int rndIndex = rnd.Next(roles.Count);
                Role rndRole = roles[rndIndex];
                newRoles.Add(rndRole._id);
                roles.Remove(rndRole);
            }

            return newRoles;
        }
    }
}
