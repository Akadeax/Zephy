using MongoDB.Bson;
using Server.database.controllers;
using Server.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.database.seeders
{
    class EmployeeSeeder
    {
        private EmployeeCrud em;

        public EmployeeSeeder()
        {
            em = new EmployeeCrud("Zephy");
        }

        public void Seed(int amountOfEmployees)
        {
            if (em.docAmount > 0) return;

            for (var i = 0; i < amountOfEmployees; i++)
            {
                em.InsertEmployee(new EmployeeModel
                {
                    roles = GetRandomRoles(),
                    name = Faker.Name.FullName()
                }
                );
            }
        }

        public List<ObjectId> GetRandomRoles()
        {
            List<ObjectId> newRoles = new List<ObjectId>();

            var rm = new RoleCrud("Zephy");
            var roles = rm.LoadRoles<RoleModel>();

            var random = new Random();
            var rLoop = random.Next(5);

            for (var i = 0; i < rLoop; i++)
            {
                var rIndex = random.Next(roles.Count);
                newRoles.Add(roles[rIndex]._id);
            }

            return newRoles;
        }
    }
}
