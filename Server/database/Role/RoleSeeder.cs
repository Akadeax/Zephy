using System;

namespace Server.database.Roles
{
    class RoleSeeder
    {
        private readonly RoleCrud roleCrud;

        public RoleSeeder()
        {
            roleCrud = new RoleCrud("Zephy");
        }

        public void Seed(int amountOfRoles)
        {
            if (roleCrud.DocumentCount > 0) return;

            for (var i = 0; i < amountOfRoles; i++)
            {
                roleCrud.InsertRole(new Role
                {
                    name = Faker.Company.Name(),
                    description = Faker.Lorem.Paragraph(5),
                });
            }
        }
    }
}
