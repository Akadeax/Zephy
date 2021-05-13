using Server;
using Server.Database.User;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.Auth
{
    public class LoginResponsePacketData : PacketData
    {
        public int httpStatus;
        public User user;
        public string accessToken;

        public LoginResponsePacketData(int httpStatus, User user, string sessionToken)
        {
            this.httpStatus = httpStatus;
            this.user = user;
            this.accessToken = sessionToken;
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
