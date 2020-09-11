using Server.database.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server.utilityData
{
    public static class UserUtilData
    {
        public static Dictionary<string, ActiveUser> loggedInUsers = new Dictionary<string, ActiveUser>();

        public static ActiveUser GetUser(string id)
        {
            return loggedInUsers[id];
        }

        public static ActiveUser GetUser(Socket clientSocket)
        {
            return loggedInUsers.Values.FirstOrDefault(x => x.clientSocket == clientSocket);
        }

        public static bool IsLoggedIn(string userId)
        {
            if (userId == null) return false;
            return loggedInUsers.ContainsKey(userId);
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
