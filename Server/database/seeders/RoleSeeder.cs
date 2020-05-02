using Server.database.controllers;
using Server.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.seeders
{
    class RoleSeeder
    {
        private RoleCrud em;

        public RoleSeeder()
        {
            em = new RoleCrud("Zephy");
        }

        public void Seed(int amountOfRoles)
        {
            if (em.docAmount > 0) return;

            for (var i = 0; i < amountOfRoles; i++)
            {
                em.InsertRole(new RoleModel
                {
                    name = Faker.Company.Name(),
                    description = Faker.Lorem.Paragraph(5),
                }
                );
            }
        }
    }
}
