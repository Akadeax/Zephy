using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Packets.Auth
{
    public class LoginPacketData
    {
        public string username, password;

        public LoginPacketData(string username, string password)
        {
            this.username = username;
            this.password = password;
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

        
        private LoginPacketData Data
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
