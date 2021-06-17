using Server;
using Server.Database.Channel;
using Server.Database.User;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.channel
{
    public class FetchMembersResponsePacketData : PacketData
    {
        public int httpStatus;
        public string channel;
        public List<ListedUser> members;

        public FetchMembersResponsePacketData(int httpStatus, string channel, List<ListedUser> members)
        {
            this.httpStatus = httpStatus;
            this.channel = channel;
            this.members = members;
        }
    }


    class FetchMembersResponsePacket : Packet
    {
        public const int TYPE = 5004;

        public FetchMembersResponsePacket(FetchMembersResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchMembersResponsePacket(byte[] packet)
            : base(packet) { }

        public FetchMembersResponsePacketData Data
        {
            get { return ReadJsonObject<FetchMembersResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
