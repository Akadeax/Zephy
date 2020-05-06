using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Server.database.seeders;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;

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
            // Start the actual TCP Server
            serverSocket.Bind(port:6556);
            serverSocket.Listen(backlog:500);
            serverSocket.Accept();

            Console.WriteLine("Listening...");

            // Start the UDP Broadcast Receiver that answers Clients
            BroadcastReceiver receiver = new BroadcastReceiver(6556);
            receiver.StartReceive();

            while (true)
            {
                Console.ReadKey();
            }
            #endregion
        }
    }
}
