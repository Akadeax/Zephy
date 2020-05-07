using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.Auth
{
    class LoginPacket : Packet
    {
        public const int TYPE = 2000;

        private string username;
        private string password;

        // Send structure: (ushort)LEN(ushort)TYPE(string)USERNAME|(string)PASSWORD
        public LoginPacket(string username, string password)
            : base(Convert.ToUInt16(BASE_PACKET_SIZE + username.Length + password.Length + 1), TYPE)
        {
            this.username = username;
            this.password = password;
        }

        public LoginPacket(byte[] packet)
            : base(packet) { }

        public string ReadUsernamePassword()
        {
            return ReadString(BASE_PACKET_SIZE, Buffer.Length - BASE_PACKET_SIZE);
        }

        public string Username
        {
            get
            {
                return ReadUsernamePassword().Split('|')[0];
            }
        }

        public string Password
        {
            get
            {
                return ReadUsernamePassword().Split('|')[1];
            }
        }
    }
}
