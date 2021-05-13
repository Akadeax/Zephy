using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Server.Networking
{
    public enum SessionState
    {
        Valid, Invalid
    }

    public static class Sessions
    {
        private static readonly List<Session> sessions = new List<Session>();

        public static Session CreateSession(string userId)
        {
            Zephy.Logger.Information($"Created new session for {userId}!");
            var newSession = new Session(
                userId,
                DateTime.UtcNow.AddDays(28)
            );

            sessions.Add(newSession);
            return newSession;
        }

        #region GetState
        public static SessionState GetStateById(string userId)
        {
            return GetState(GetSessionById(userId));
        }

        public static SessionState GetStateByToken(string token)
        {
            return GetState(GetSessionByToken(token));
        }

        public static SessionState GetState(Session session)
        {
            if (session == null) return SessionState.Invalid;

            if (DateTime.UtcNow >= session.expirationDate)
            {
                sessions.Remove(session);
                CreateSession(session.userId);
                return SessionState.Invalid;
            }
            else
            {
                return SessionState.Valid;
            }
        }
        #endregion

        #region GetSession
        public static Session GetSessionById(string userId)
        {
            return sessions.Find(x => x.userId == userId);
        }
        public static Session GetSessionByToken(string accessToken)
        {
            return sessions.Find(x => x.accessToken == accessToken);
        }
        #endregion
    }

    public class Session
    {
        public string userId;
        public string accessToken;
        public DateTime expirationDate;

        public Session(string userId, DateTime expirationDate)
        {
            this.userId = userId;
            this.expirationDate = expirationDate;
            GenerateToken();
        }

        private void GenerateToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 16)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            accessToken = resultToken.ToString();
        }
    }
}
