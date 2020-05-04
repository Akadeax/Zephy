using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Server.database.seeders;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Will only seed empty collections in the Zephy database
            SeederHandler s = new SeederHandler();
            s.Start(entrees:100);

            EmployeeCrud employeeCrud = new EmployeeCrud("Zephy");

            // This will get a list of all employees in the system
            List<Employee> employees = employeeCrud.LoadEmployees();

            //RoleCache roleCache = new RoleCache("Zephy");

            List<PopulatedEmployee> pop = new List<PopulatedEmployee>();
            foreach(Employee emp in employees)
            {
                pop.Add(employeeCrud.Populate(emp));
            }

            foreach(PopulatedEmployee emp in pop)
            {
                Console.WriteLine(emp.name);
            }

            Console.WriteLine("Done");
            Console.ReadKey(true);
        }
    }
}
