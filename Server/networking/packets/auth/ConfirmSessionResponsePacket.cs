using Server;
using Server.Database.User;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.auth
{
    public class ConfirmSessionResponsePacketData : PacketData
    {
        public int httpStatus;
        public User user;

        public ConfirmSessionResponsePacketData(int httpStatus, User user)
        {
            this.httpStatus = httpStatus;
            this.user = user;
        }
    }

    class ConfirmSessionResponsePacket : Packet
    {
        public const int TYPE = 2004;

        public ConfirmSessionResponsePacket(ConfirmSessionResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ConfirmSessionResponsePacket(byte[] packet)
            : base(packet) { }

        public ConfirmSessionResponsePacketData Data
        {
            get { return ReadJsonObject<ConfirmSessionResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
