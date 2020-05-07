using System;
using System.Collections.Generic;
using System.Text;

namespace Packets.User
{
    // DeleteUser has the exact same arguments as a Logout
    class DeleteUserPacket : Auth.LogoutPacket
    {
        public const int TYPE = 2101;

        public DeleteUserPacket(string username)
            : base(username) { }

        public DeleteUserPacket(byte[] packet)
            : base(packet) { }
    }
}
