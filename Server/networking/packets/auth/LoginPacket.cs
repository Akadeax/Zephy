using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Packets.Auth
{
    class LoginPacketData
    {
        public string username, password;

        public LoginPacketData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    class LoginPacketHandler : PacketHandler<LoginPacket>
    {
        protected override void Handle(LoginPacket packet, Socket sender)
        {
            IPEndPoint ep = sender.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Login attempt with '{packet.Username}' and '{packet.Password}' from {ep.Address}.");
        }
    }

    class LoginPacket : Packet
    {
        public const int TYPE = 2000;

        public LoginPacket(LoginPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public LoginPacket(byte[] packet)
            : base(packet) { }

        
        protected LoginPacketData Data
        {
            get { return ReadJsonObject<LoginPacketData>(); }
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
