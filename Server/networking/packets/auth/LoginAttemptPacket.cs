using Server;
using Server.Database.User;
using Server.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Packets.auth
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
        readonly UserCrud userCrud = new UserCrud();

        protected override void Handle(LoginAttemptPacket packet, Socket sender)
        {
            var data = packet.Data;
            if (data == null) return;

            // TODO: REMOVE
            #region DEBUG
            if(data.identifier == "test")
            {
                User dbgUser = userCrud.ReadOne();
                data.identifier = dbgUser.identifier;
                data.password = dbgUser.password;
            }
            #endregion

            if (ActiveUsers.IsLoggedIn(sender))
            {
                SendError(HttpStatusCode.Forbidden, sender);

                return;
            }

            User user = userCrud.ReadOne(x => x.identifier == data.identifier);
            if(user == null || user.password != data.password)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            bool addSuccess = ActiveUsers.AddActiveUser(new ActiveUser(user._id, sender));
            if(!addSuccess)
            {
                SendError(HttpStatusCode.Forbidden, sender);
                return;
            }

            Session session;
            if(Sessions.GetStateById(user._id) == SessionState.Invalid)
            {
                session = Sessions.CreateSession(user._id);
            }
            else
            {
                session = Sessions.GetSessionById(user._id);
            }

            var successResponse = new LoginResponsePacket(new LoginResponsePacketData(
                (int)HttpStatusCode.OK, user, session.accessToken
            ));
            Zephy.serverSocket.SendPacket(successResponse, sender);
        }


        private void SendError(HttpStatusCode code, Socket sender)
        {
            var response = new LoginResponsePacket(new LoginResponsePacketData(
                    (int)code, null, null
            ));
            Zephy.serverSocket.SendPacket(response, sender);
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
