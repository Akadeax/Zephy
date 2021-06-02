using Server;
using Server.Database.User;
using Server.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.auth
{
    public class ConfirmSessionRequestPacketData : PacketData
    {
        public string accessToken;

        public ConfirmSessionRequestPacketData(string accessToken)
        {
            this.accessToken = accessToken;
        }
    }

    class ConfirmSessionRequestPacketHandler : PacketHandler<ConfirmSessionRequestPacket>
    {
        readonly UserCrud userCrud = new UserCrud();

        protected override void Handle(ConfirmSessionRequestPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            // Check whether sessions doesn't exist/is invalid
            Session session = Sessions.GetSessionByToken(data.accessToken);
            if(Sessions.GetState(session) == SessionState.Invalid)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            // Check whether user exists
            User user = userCrud.ReadOneById(session.userId);
            if(user == null)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            // Add to active users
            bool addSuccess = ActiveUsers.AddActiveUser(new ActiveUser(user._id, sender));
            if (!addSuccess)
            {
                SendError(HttpStatusCode.Forbidden, sender);
                return;
            }

            // Respond with confirmed session, client can instantly redirect
            var successResponse = new ConfirmSessionResponsePacket(new ConfirmSessionResponsePacketData(
                (int)HttpStatusCode.OK,
                user
            ));
            Zephy.serverSocket.SendPacket(successResponse, sender);
        }

        private void SendError(HttpStatusCode code, Socket sender)
        {
            var errResponse = new ConfirmSessionResponsePacket(new ConfirmSessionResponsePacketData(
                (int)code,
                null
            ));
            Zephy.serverSocket.SendPacket(errResponse, sender);
        }
    }


    class ConfirmSessionRequestPacket : Packet
    {
        public const int TYPE = 2003;

        public ConfirmSessionRequestPacket(ConfirmSessionRequestPacketData data) : base(TYPE, data)
        {
            Data = data;
        }

        public ConfirmSessionRequestPacket(byte[] packet)
            : base(packet) { }

        public ConfirmSessionRequestPacketData Data
        {
            get { return ReadJsonObject<ConfirmSessionRequestPacketData>(); }
            set { WriteJsonObject(value); }
        }
    }
}
