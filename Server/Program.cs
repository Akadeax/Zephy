using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Server.database.seeders;

namespace Server
{
    class Program
    {
        static ServerSocket serverSocket = new ServerSocket();

        static void Main(string[] args)
        {
            #region seeding
            // Will only seed empty collections in the Zephy database
            SeederHandler s = new SeederHandler();
            s.Start(entrees:100);

            EmployeeCrud employeeCrud = new EmployeeCrud("Zephy");

            // This will get a list of all employees in the system
            List<Employee> employees = employeeCrud.LoadEmployees();
            #endregion

            #region Socket
            serverSocket.Bind(port:6556);
            serverSocket.Listen(backlog:500);
            serverSocket.Accept();

            Console.WriteLine("Listening...");
            while (true)
                Console.ReadKey();

            #endregion
        }
    }
}
