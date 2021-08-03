using Server;
using Server.Database.User;
using Server.Networking;
using System.Linq;
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

            // TODO: REMOVE, DEBUG LOGIN
            #region DEBUG
            if(data.identifier.StartsWith("test") && data.identifier.Length == 5)
            {
                int num = int.Parse(data.identifier[4].ToString());
                User dbgUser = userCrud.ReadMany().ToList()[num];
                data.identifier = dbgUser.identifier;
                data.password = dbgUser.password;
            }
            #endregion

            if (ActiveUsers.IsLoggedIn(sender))
            {
                SendError(HttpStatusCode.Forbidden, sender);

                return;
            }
            // check whether the request is valid
            User user = userCrud.ReadOne(x => x.identifier == data.identifier);
            if(user == null || user.password != data.password)
            {
                SendError(HttpStatusCode.Unauthorized, sender);
                return;
            }

            // add user to list of logged in users
            bool addSuccess = ActiveUsers.AddActiveUser(new ActiveUser(user._id, sender));
            if(!addSuccess)
            {
                SendError(HttpStatusCode.Forbidden, sender);
                return;
            }

            // fetch the logging in user's session, or create new one if not exists
            Session session;
            if(Sessions.GetStateById(user._id) == SessionState.Invalid)
            {
                session = Sessions.CreateSession(user._id);
            }
            else
            {
                session = Sessions.GetSessionById(user._id);
            }

            // respond
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
