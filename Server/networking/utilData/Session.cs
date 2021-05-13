using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Server.Networking
{
    public enum SessionState
    {
        Valid, Expired, NotFound
    }

    public static class Sessions
    {
        private static readonly List<Session> sessions = new List<Session>();

        public static Session AddSession(string userId)
        {
            MD5 md5 = MD5.Create();

            var newSession = new Session(
                userId,
                Encoding.UTF8.GetString(md5.ComputeHash(Encoding.UTF8.GetBytes(userId))),
                DateTime.UtcNow.AddDays(10)
            );

            sessions.Add(newSession);
            return newSession;
        }

        public static SessionState GetState(string userId)
        {
            Session session = sessions.Find(x => x.userId == userId);
            if (session == null) return SessionState.NotFound;
            if (session.expirationDate > DateTime.UtcNow)
            {
                sessions.Remove(session);
                return SessionState.Expired;
            }
            else
            {
                return SessionState.Valid;
            }
        }
    }

    public class Session
    {
        public string userId;
        public string accessToken;
        public DateTime expirationDate;

        public Session(string userId, string accessToken, DateTime expirationDate)
        {
            this.userId = userId;
            this.accessToken = accessToken;
            this.expirationDate = expirationDate;
        }
    }
}
