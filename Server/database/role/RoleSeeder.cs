using Server.utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Server.database.role
{
    class RoleSeeder : MongoSeeder
    {
        readonly RoleCrud roleCrud = new RoleCrud();

        public override void Seed(SeederEntriesAmount amount)
        {
            if (amount.roleSeederAmount < 1 || amount.roleSeederAmount > MAX_SEED)
                throw new ArgumentException("out of bounds", "amount");

            if (roleCrud.DocumentCount > 0) return;

            for (int i = 0; i < amount.roleSeederAmount; i++)
            {
                roleCrud.CreateOne(new Role
                {
                    name = Faker.Company.Name(),
                    description = Faker.Lorem.Sentence(1),
                });
            }
        }


        public static List<Role> GetRandomRoles(RoleCrud crud, int roleAmount, Expression<Func<Role, bool>> filter = null)
        {
            List<Role> records = crud.ReadMany(filter);
            if(records.Count < roleAmount)
            {
                Zephy.Logger.Error($"Couldn't fetch {roleAmount} roles, only {records.Count} exist");
            }

            records.Shuffle();

            return records.GetRange(0, roleAmount);
        }
    }
}
