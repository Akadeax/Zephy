using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Server.database.seeders;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using Server.validation;

namespace Server
{
    class Program
    {
        static ServerSocket serverSocket = new ServerSocket();

        static void Main(string[] args)
        {
            #region Seeding
            // Will only seed empty collections in the Zephy database
            SeederHandler s = new SeederHandler();
            s.Start(entrees:50);
            #endregion

            bool user = Validator.Validate("goodwaifu");
            Console.WriteLine(user);

            #region Socket
            // Start the actual TCP Server
            serverSocket.Bind(port:6556);
            serverSocket.Listen(backlog:500);
            // Start client accept loop
            serverSocket.Accept();

            Console.WriteLine("Listening...");

            // Start the UDP Broadcast Receiver that answers Clients search for the local IP
            BroadcastReceiver receiver = new BroadcastReceiver(6556);
            receiver.StartReceive();

            while (true)
            {
                Console.ReadLine();
            }
            #endregion
        }
    }
}
