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

        public const int PORT = 6556;

        static void Main(string[] args)
        {
            #region Seeding
            // Will only seed empty collections in the Zephy database
            SeederHandler s = new SeederHandler();
            s.Start(entrees: 100);

            EmployeeCrud employeeCrud = new EmployeeCrud("Zephy");

            // This will get a list of all employees in the system
            List<Employee> employees = employeeCrud.LoadEmployees();
            #endregion

            #region Socket
            // Start the actual TCP Server
            serverSocket.Bind(PORT);
            serverSocket.Listen(backlog: 500);
            // Start client accept loop
            serverSocket.Accept();

            Console.WriteLine("Listening...");

            // Start the UDP Broadcast Receiver that answers Clients search for the server's local IP
            BroadcastReceiver receiver = new BroadcastReceiver(PORT);
            receiver.StartReceive();

            while (true)
            {
                Console.ReadLine();
            }
            #endregion
        }
    }
}
