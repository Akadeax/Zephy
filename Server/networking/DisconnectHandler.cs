using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public static class DisconnectHandler
    {
        public static void OnDisconnect(Socket socket)
        {
            IPAddress disconnectedAddress = (socket.LocalEndPoint as IPEndPoint).Address;
            Zephy.Logger.Information($"{disconnectedAddress} has disconnected, closing & disposing socket.");

            if(ActiveUsers.IsLoggedIn(socket))
            {
                ActiveUser user = ActiveUsers.GetUser(socket);
                ActiveUsers.RemoveUser(socket);
                Zephy.Logger.Information($"User at {disconnectedAddress} was logged in as {user.userId}, logging out.");
            }
            socket.Close();
        }
    }
}
