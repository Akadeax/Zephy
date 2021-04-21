using server;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace packets.auth
{
    public class LoginAttemptPacketData : PacketData
    {
        public string identifier;
        public string password;

        public LoginAttemptPacketData(string identifier, string password)
        {
            this.identifier = identifier;
            this.password = password;
        }
    }

    class LoginAttemptPacketHandler : PacketHandler<LoginAttemptPacket>
    {
        protected override void Handle(LoginAttemptPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            sender.Send(new LoginResponsePacket(new LoginResponsePacketData((int)HttpStatusCode.Unauthorized, null)).Buffer);
        }
    }

    class LoginAttemptPacket : Packet
    {
        public const int TYPE = 2001;

        public LoginAttemptPacket(LoginAttemptPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public LoginAttemptPacket(byte[] packet)
            : base(packet) { }

        public LoginAttemptPacketData Data
        {
            get { return ReadJsonObject<LoginAttemptPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
