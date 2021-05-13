using Server.Database.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Server
{
    public static class ActiveUsers
    {
        public static readonly List<ActiveUser> loggedInUsers = new List<ActiveUser>();

        /// <summary>
        /// Adds a user to the current list of active users
        /// </summary>
        /// <returns>success</returns>
        public static bool AddActiveUser(ActiveUser toAdd)
        {
            if (loggedInUsers.Any(x => x.userId == toAdd.userId)) return false;
            loggedInUsers.Add(toAdd);
            Zephy.Logger.Information($"Added {toAdd.userId} to active users.");
            return true;
        }

        /// <summary>
        /// Removes a user from the current list of active users
        /// </summary>
        /// <returns>whether user was in list</returns>
        public static bool RemoveUser(Socket socket)
        {
            ActiveUser user = loggedInUsers.First(x => x.clientSocket == socket);
            if (user == null) return false;
            loggedInUsers.Remove(user);
            return true;
        }

        public static ActiveUser GetUser(string id)
        {
            return loggedInUsers.FirstOrDefault(x => x.userId == id);
        }

        public static ActiveUser GetUser(Socket clientSocket)
        {
            return loggedInUsers.FirstOrDefault(x => x.clientSocket == clientSocket);
        }

        public static bool IsLoggedIn(string userId)
        {
            if (userId == null) return false;
            foreach(var user in loggedInUsers)
            {
                if (user.userId == userId) return true;
            }
            return false;
        }

        public static bool IsLoggedIn(Socket userSocket)
        {
            return loggedInUsers.Any(x => x.clientSocket == userSocket);
        }
    }

    public class ActiveUser
    {
        public string userId;
        public Socket clientSocket;

        public ActiveUser(string userId, Socket clientSocket)
        {
            this.userId = userId;
            this.clientSocket = clientSocket;
        }

        public User GetUser(UserCrud userCrud)
        {
            return userCrud.ReadOneById(userId);
        }
    }
}
