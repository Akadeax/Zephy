using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using Packets.Auth;
using System.Text;
using Packets.General;

namespace Server
{
    class Zephy
    {
        public static readonly ServerSocket serverSocket = new ServerSocket();

        public const int PORT = 6556;

        static void Main()
        {
            #region Socket
            // Start UDP Broadcast Receiver that answers Clients search for the server's local IP
            BroadcastReceiver receiver = new BroadcastReceiver(PORT);
            receiver.StartReceive();


            // Start TCP Server
            serverSocket.Bind(PORT);
            serverSocket.Listen(backlog: 500);
            // Start client accept loop
            serverSocket.Accept();

            Console.WriteLine("Listening...");


            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd == "clear") Console.Clear();
            }
            #endregion
        }
    }
}
