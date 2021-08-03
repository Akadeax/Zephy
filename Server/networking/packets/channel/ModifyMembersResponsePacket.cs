using Server;
using Server.Database.Channel;
using Server.Database.User;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.channel
{
    public class ModifyMembersResponsePacketData : PacketData
    {
        public int httpStatus;
        public ListedUser user;
        public string channel;
        public int action;

        public ModifyMembersResponsePacketData(int httpStatus, ListedUser user, string channel, int action)
        {
            this.httpStatus = httpStatus;
            this.user = user;
            this.channel = channel;
            this.action = action;
        }
    }

    class ModifyMembersResponsePacket : Packet
    {
        public const int TYPE = 3006;

        public ModifyMembersResponsePacket(ModifyMembersResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ModifyMembersResponsePacket(byte[] packet)
            : base(packet) { }

        public ModifyMembersResponsePacketData Data
        {
            get { return ReadJsonObject<ModifyMembersResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
