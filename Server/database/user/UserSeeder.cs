using MongoDB.Bson;
using Server.database.role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.database.user
{
    public class UserSeeder : MongoSeeder
    {
        readonly UserCrud userCrud = new UserCrud();
        readonly RoleCrud roleCrud = new RoleCrud();

        public override void Seed(SeederEntriesAmount amount)
        {
            if (amount.userSeederAmount < 1 || amount.userSeederAmount > MAX_SEED)
                throw new ArgumentException("out of bounds", "amount");

            if (userCrud.DocumentCount > 0) return;

            userCrud.CreateOne(new User
            {
                roles = roleCrud.ReadMany().Select(x => x._id).ToList(),
                name = "admin",
                email = "admin@admin.com",
                password = "admin",
            });

            for (int i = 0; i < amount.userSeederAmount; i++)
            {
                List<ObjectId> roles = RoleSeeder.GetRandomRoles(roleCrud, 3)
                .Select(x => x._id)
                .ToList();

                string name = Faker.Name.Last();
                userCrud.CreateOne(new User
                {
                    roles = roles,
                    name = name,
                    email = $"{name}@gmail.com".ToLower(),
                    password = "test",
                });
            }
        }
    }
}
