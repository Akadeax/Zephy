using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MongoDB.Bson;
using Newtonsoft.Json;
using Server;
using Server.database.user;
using Server.utilities;
using System.Linq;

namespace Packets.auth
{
    class LoginAttemptPacketData : PacketData
    {
        public string email, password;

        public LoginAttemptPacketData(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }

    class LoginAttemptPacketHandler : PacketHandler<LoginAttemptPacket>
    {
        readonly UserCrud userCrud = new UserCrud();

        protected override void Handle(LoginAttemptPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            IPEndPoint ep = sender.LocalEndPoint as IPEndPoint;
            Zephy.Logger.Information($"Login attempt with '{data.email}' and '{data.password}' from {ep.Address}.");

            HttpStatusCode code = HttpStatusCode.OK;
            PopulatedUser user = userCrud.ReadOnePopulated(x => x.email == data.email);

            if (user == null || user.password != data.password)
            {
                code = HttpStatusCode.Unauthorized;
                user = null;
            }

            LoginResultPacket retPacket = new LoginResultPacket(new LoginResultPacketData(
                (int)code,
                user
            ));

            Zephy.serverSocket.SendPacket(retPacket, sender);
        }
    }

    class LoginAttemptPacket : Packet
    {
        public const int TYPE = 2000;

        public LoginAttemptPacket(LoginAttemptPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public LoginAttemptPacket(byte[] packet)
            : base(packet) { }

        
        public LoginAttemptPacketData Data
        {
            get { return ReadJsonObject<LoginAttemptPacketData>(); }
            private set { WriteJsonObject(value); }
        }
    }
}
