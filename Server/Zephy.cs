using System;
using Server.database;
using Serilog;
using Serilog.Core;
using Newtonsoft.Json;
using Packets.message;
using Server.database.message;
using System.Linq;
using Server.database.channel;
using Server.database.user;
using MongoDB.Bson;
using Server.utilities;
using Packets.general;

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
                userSeederAmount = 40,
                roleSeederAmount = 7,
                channelSeederAmount = 15,
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
                else if(cmd == "ins")
                {
                    Console.Write("OID: ");
                    string oid = Console.ReadLine();
                    Channel channel = new ChannelCrud().ReadOneById(oid);

                    Message message = new Message
                    {
                        _id = ObjectId.GenerateNewId().ToString(),
                        author = new UserCrud().ReadOne()._id,
                        content = "git gud",
                        sentAt = Util.RandTimestamp(),
                    };

                    new MessageCrud().CreateOne(message);
                    channel.messages.Add(message._id);
                    Console.WriteLine("did");
                    new ChannelCrud().UpdateOne(oid, channel);
                }
            }
            #endregion
        }
    }
}
