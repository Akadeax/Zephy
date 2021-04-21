using server;
using server.database.user;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace packets.auth
{
    public class LoginResponsePacketData : PacketData
    {
        public int httpStatus;
        public User user;

        public LoginResponsePacketData(int httpStatus, User user)
        {
            this.httpStatus = httpStatus;
            this.user = user;
        }
    }

    class LoginResponsePacket : Packet
    {
        public const int TYPE = 2002;

        public LoginResponsePacket(LoginResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public LoginResponsePacket(byte[] packet)
            : base(packet) { }

        public LoginResponsePacketData Data
        {
            get { return ReadJsonObject<LoginResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
