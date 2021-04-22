using MongoDB.Bson;
using Newtonsoft.Json;
using Packets.Auth;
using Packets.Channel;
using Packets.Message;
using Serilog;
using Serilog.Core;
using Server.Database;
using Server.Database.Channel;
using Server.Database.Message;
using Server.Database.User;
using Server.Util;
using Server.UtilData;
using System;
using System.Linq;

namespace Server
{
    class Zephy
    {
        public static Logger Logger { get; } = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

        public static readonly BroadcastReceiver broadcastReceiver = new BroadcastReceiver(PORT, PORT + 1);
        public static readonly ServerSocket serverSocket = new ServerSocket();

        public const int PORT = 6556;

        static void Main()
        {
            #region Seeding
            SeederHandler.Seed(new SeederEntriesAmount
            {
                userSeederAmount = 100,
                channelSeederAmount = 15,
                messageSeederAmount = 5000,
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
                else if (cmd == "q")
                {
                    serverSocket.CloseAllSockets();
                    break;
                }
                else if (cmd == "dbg")
                {
                    UserCrud uc = new UserCrud();
                    ChannelCrud cc = new ChannelCrud();
                    Logger.Debug($"Active channels:");
                    foreach (ActiveUser u in UserUtilData.loggedInUsers)
                    {
                        Logger.Debug($"'{uc.ReadOneById(u.userId).identifier}': '{cc.ReadOneById(u.activeChannelId).name}'");
                    }
                }
            }
            #endregion
        }
    }
}
