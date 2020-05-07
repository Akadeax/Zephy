using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.User
{
    // CreateUser has the exact same arguments as a Login
    class CreateUserPacket : Auth.LoginPacket
    {
        public const int TYPE = 2100;

        public CreateUserPacket(string username, string password)
            : base(username, password) { }

        public CreateUserPacket(byte[] packet)
            : base(packet) { }
    }
}
