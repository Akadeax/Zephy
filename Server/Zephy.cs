using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using Packets.auth;
using System.Text;
using Packets.general;
using Server.database.user;
using Server.database;
using Server.database.message;
using Server.database.channel;
using Newtonsoft.Json;
using Server.database.role;
using Serilog;
using Serilog.Core;
using Packets.channel;
using System.Linq;
using Packets.message;
using System.Xml;

namespace Server
{
    class Zephy
    {
        public static Logger Logger { get; } = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

        public static readonly BroadcastReceiver broadcastReceiver = new BroadcastReceiver(PORT);
        public static readonly ServerSocket serverSocket = new ServerSocket();

        public const int PORT = 6556;

        static void Main()
        {
            #region Seeding
            SeederHandler.Seed(new SeederEntriesAmount
            {
                userSeederAmount = 30,
                roleSeederAmount = 7,
                channelSeederAmount = 8,
                messageSeederAmount = 20000,
            });
            #endregion

            #region Socket
            // Start UDP Broadcast Receiver that answers Clients search for the server's local IP
            broadcastReceiver.StartReceive();

            // Start TCP Server
            serverSocket.Start(PORT);

            Logger.Information("Listening to new connections...");

            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd == "clear") Console.Clear();
            }
            #endregion
        }
    }
}
