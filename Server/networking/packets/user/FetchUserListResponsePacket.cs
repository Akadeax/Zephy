using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Packets.user
{
    public class FetchUserListResponsePacketData : PacketData
    {
        public int httpStatus;
        public int page;
        public List<ListedUser> users;

        public FetchUserListResponsePacketData(int httpStatus, int page, List<ListedUser> users)
        {
            this.httpStatus = httpStatus;
            this.page = page;
            this.users = users;
        }
    }

    class FetchUserListResponsePacket : Packet
    {
        public const int TYPE = 5002;

        public FetchUserListResponsePacket(FetchUserListResponsePacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchUserListResponsePacket(byte[] packet)
            : base(packet) { }

        public FetchUserListResponsePacketData Data
        {
            get { return ReadJsonObject<FetchUserListResponsePacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
