using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.Auth
{
    class LogoutPacket : Packet
    {
        public const int TYPE = 2001;

        private string username;

        public LogoutPacket(string username)
            : base(Convert.ToUInt16(4 + username.Length), TYPE)
        {
            this.username = username;
        }

        public LogoutPacket(byte[] packet)
            : base(packet) { }

        public string Username
        {
            get
            {
                return ReadString(BASE_PACKET_SIZE, Buffer.Length - BASE_PACKET_SIZE);
            }
        }
    }
}
