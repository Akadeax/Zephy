using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Server.database.seeders;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using Packets.Auth;
using System.Text;
using Packets.General;

namespace Server
{
    class Program
    {
        static readonly ServerSocket serverSocket = new ServerSocket();

        public const int PORT = 6556;

        static void Main()
        {
            #region Seeding
            // Will only seed empty collections in the Zephy database
            SeederHandler s = new SeederHandler();
            s.Start(entrees: 100);

            UserCrud userCrud = new UserCrud("Zephy");

            // This will get a list of all employees in the system
            List<User> users = userCrud.ReadRecords();
            // remove later, this is just so VS doesn't complain
            _ = users;
            #endregion


            #region Socket
            // Start the UDP Broadcast Receiver that answers Clients search for the server's local IP
            BroadcastReceiver receiver = new BroadcastReceiver(PORT);
            receiver.StartReceive();


            // Start the actual TCP Server
            serverSocket.Bind(PORT);
            serverSocket.Listen(backlog: 500);
            // Start client accept loop
            serverSocket.Accept();

            Console.WriteLine("Listening...");


            while (true)
            {
                // TODO: Remove, only for debugging
                string cmd = Console.ReadLine();
                if (cmd == "clear") Console.Clear();
            }
            #endregion
        }
    }
}
