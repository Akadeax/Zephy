using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Packets.Auth
{
    public class LogoutPacketData
    {
        public string username, password;

        public LogoutPacketData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    class LogoutPacket : Packet
    {
        public const int TYPE = 2001;

        public LogoutPacket(LogoutPacketData data) : base(TYPE)
        {
            Data = data;
        }

        public LogoutPacket(byte[] packet)
            : base(packet) { }


        private LogoutPacketData Data
        {
            get { return ReadJsonObject<LogoutPacketData>(); }
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
