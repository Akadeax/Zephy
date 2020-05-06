using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class LoginPacket : Packet
    {
        public const int TYPE = 2000;

        private string email;
        private string password;

        // Send structure: (ushort)LEN(ushort)TYPE(string)EMAIL|(string)PASSWORD
        public LoginPacket(string email, string password)
            : base(Convert.ToUInt16(4 + email.Length + password.Length + 1), TYPE)
        {
            this.email = email;
            this.password = password;
        }

        public LoginPacket(byte[] packet)
            : base(packet) { }

        public string ReadEmailPassword()
        {
            return ReadString(4, Buffer.Length - 4);
        }

        public string Email
        {
            get
            {
                return ReadEmailPassword().Split('|')[0];
            }
        }

        public string Password
        {
            get
            {
                return ReadEmailPassword().Split('|')[1];
            }
        }
    }
}
