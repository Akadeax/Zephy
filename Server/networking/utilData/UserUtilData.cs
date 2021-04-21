using server.database.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace server.utilityData
{
    public static class UserUtilData
    {
        public static readonly Dictionary<Socket, ActiveUser> loggedInUsers = new Dictionary<Socket, ActiveUser>();

        /// <summary>
        /// Adds a user to the current list of active users
        /// </summary>
        /// <returns>success</returns>
        public static bool AddActiveUser(ActiveUser toAdd)
        {
            if(loggedInUsers.Values.Any(x => x.userId == toAdd.userId)) return false;
            loggedInUsers[toAdd.clientSocket] = toAdd;
            return true;
        }
        public static void RemoveUser(Socket socket)
        {
            loggedInUsers.Remove(socket);
        }

        public static ActiveUser GetUser(string id)
        {
            return loggedInUsers.Values.FirstOrDefault(x => x.userId == id);
        }

        public static ActiveUser GetUser(Socket clientSocket)
        {
            return loggedInUsers.Values.FirstOrDefault(x => x.clientSocket == clientSocket);
        }

        public static bool IsLoggedIn(string userId)
        {
            if (userId == null) return false;
            foreach(var user in loggedInUsers.Values)
            {
                if (user.userId == userId) return true;
            }
            return false;
        }

        public static bool IsLoggedIn(Socket userSocket)
        {
            return loggedInUsers.ContainsKey(userSocket);
        }

        public static List<ActiveUser> GetActiveInChannel(string channelId)
        {
            return loggedInUsers.Values.Where(x => x.activeChannelId == channelId).ToList();
        }
    }

    public class ActiveUser
    {
        public string userId;
        public Socket clientSocket;

        public string activeChannelId = "";

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
