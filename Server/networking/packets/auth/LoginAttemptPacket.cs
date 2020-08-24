using System;
using System.Net;
using System.Net.Sockets;
using Server;

namespace Packets.Auth
{
    class LoginAttemptData
    {
        public string username, password;

        public LoginAttemptData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    class LoginAttemptPacketHandler : PacketHandler<LoginAttemptPacket>
    {
        protected override void Handle(LoginAttemptPacket packet, Socket sender)
        {
            IPEndPoint ep = sender.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Login attempt with '{packet.Username}' and '{packet.Password}' from {ep.Address}.");
            Packet retPacket = new LoginResultPacket(new LoginResultPacketData((int)HttpStatusCode.OK));
            Zephy.serverSocket.SendPacket(retPacket, sender);
        }
    }

    class LoginAttemptPacket : Packet
    {
        public const int TYPE = 2000;

        public LoginAttemptPacket(LoginAttemptData data) : base(TYPE)
        {
            Data = data;
        }

        public LoginAttemptPacket(byte[] packet)
            : base(packet) { }

        
        protected LoginAttemptData Data
        {
            get { return ReadJsonObject<LoginAttemptData>(); }
            set { WriteJsonObject(value); }
        }

        public string Username
        {
            get { return Data.username; }
        }

        public string Password
        {
            get { return Data.password; }
        }
    }
}
