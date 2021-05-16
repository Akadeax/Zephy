using Server;
using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Packets.user
{
    public class FetchUserListRequestPacketData : PacketData
    {
        public string search;

        public FetchUserListRequestPacketData(string search)
        {
            this.search = search;
        }
    }

    class FetchUserListRequestPacketHandler : PacketHandler<FetchUserListRequestPacket>
    {
        readonly UserCrud userCrud = new UserCrud();

        protected override void Handle(FetchUserListRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;
            data.search = data.search.ToLower();

            ActiveUser activeUser = ActiveUsers.GetUser(sender);
            if (activeUser == null)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            User user = userCrud.ReadOneById(activeUser.userId);

            List<ListedUser> users = userCrud.ReadManyListed(x =>
                x.identifier.ToLower().Contains(data.search) ||
                x.fullName.ToLower().Contains(data.search)
            );

            var response = new FetchUserListResponsePacket(new FetchUserListResponsePacketData(
                (int)HttpStatusCode.OK,
                users
            ));
            Zephy.serverSocket.SendPacket(response, sender);
        }

        private void SendError(HttpStatusCode code, Socket socket)
        {
            var errResponse = new FetchUserListResponsePacket(new FetchUserListResponsePacketData(
                (int)code,
                null
            ));
            Zephy.serverSocket.SendPacket(errResponse, socket);
        }
    }


    class FetchUserListRequestPacket : Packet
    {
        public const int TYPE = 5001;

        public FetchUserListRequestPacket(FetchUserListRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public FetchUserListRequestPacket(byte[] packet)
            : base(packet) { }

        public FetchUserListRequestPacketData Data
        {
            get { return ReadJsonObject<FetchUserListRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
