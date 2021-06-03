using Server;
using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace Packets.user
{
    public class FetchUserListRequestPacketData : PacketData
    {
        public string search;
        public int page;
        public List<string> optionalExcludeIds;

        public FetchUserListRequestPacketData(string search, int page, List<string> optionalExcludeIds)
        {
            this.search = search;
            this.page = page;
            this.optionalExcludeIds = optionalExcludeIds;
        }
    }

    class FetchUserListRequestPacketHandler : PacketHandler<FetchUserListRequestPacket>
    {
        const int PAGE_SIZE = 20;
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

            // Fetch all users that match the search & aren't the requester
            List<ListedUser> users = userCrud.ReadManyListedPaginated(
                data.page, PAGE_SIZE,
                x =>
                (x.identifier.ToLower().Contains(data.search)
                || x.fullName.ToLower().Contains(data.search))
                && x._id != user._id
            );



            // remove excludes
            if(data.optionalExcludeIds != null)
            {
                users.RemoveAll(x => data.optionalExcludeIds.Contains(x._id));
            }

            var response = new FetchUserListResponsePacket(new FetchUserListResponsePacketData(
                (int)HttpStatusCode.OK,
                data.page,
                users
            ));
            Zephy.serverSocket.SendPacket(response, sender);
        }

        private void SendError(HttpStatusCode code, Socket socket)
        {
            var errResponse = new FetchUserListResponsePacket(new FetchUserListResponsePacketData(
                (int)code,
                0,
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
