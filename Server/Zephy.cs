using Serilog;
using Serilog.Core;
using Server.Database;
using System;

namespace Server
{
    class Zephy
    {
        public static Logger Logger { get; } = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

        public static readonly BroadcastReceiver broadcastReceiver = new BroadcastReceiver();
        public static readonly ServerSocket serverSocket = new ServerSocket();

        static void Main()
        {
            #region Seeding
            SeederHandler.Seed(new SeederEntriesAmount
            {
                userSeederAmount = 1000,
                channelSeederAmount = 1000,
                messageSeederAmount = 5000,
            });
            #endregion

            #region Socket
            // Start UDP Broadcast Receiver that answers Clients search for the server's local IP
            broadcastReceiver.StartReceive();

            // Start TCP Server
            serverSocket.Start();

            Logger.Information("Listening to new connections...");

            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd == "clear") Console.Clear();
                else if (cmd == "q")
                {
                    break;
                }
            }
            #endregion
        }
    }
}
