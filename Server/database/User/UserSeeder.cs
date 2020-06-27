using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Server.database.Roles;

namespace Server
{
    class UserSeeder
    {
        private UserCrud userCrud;
        private RoleCrud roleCrud;

        public UserSeeder()
        {
            userCrud = new UserCrud("Zephy");
            roleCrud = new RoleCrud("Zephy");
        }

        public void Seed(int amountOfEmployees)
        {
            if (userCrud.DocumentCount > 0) return;

            for (var i = 0; i < amountOfEmployees; i++)
            {
                userCrud.Create(new User
                {
                    roles = GetRandomRoles(),
                    name = Faker.Name.FullName(),
                    password = "test",
                });
            }

            List<ObjectId> roleList = new List<ObjectId>();
            Role AdminRole = roleCrud.ReadOne("administrator");
            roleList.Add(AdminRole._id);

            userCrud.Create(new User
            {
                roles = roleList,
                name = "admin",
                password = "root"
            });
        }

        public List<ObjectId> GetRandomRoles()
        {
            List<ObjectId> randomRoles = new List<ObjectId>();

            List<Role> roles = roleCrud.ReadMany();

            Random r = new Random();

            for (var i = 0; i < 5; i++)
            {
                int randomCount = r.Next(1, roles.Count);
                Console.WriteLine(roles[randomCount]._id);
                randomRoles.Add(roles[randomCount]._id);
            }
            return randomRoles;
        }
    }
}
