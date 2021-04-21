using packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{

    class ServerSocket
    {
        public readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public readonly List<Socket> clientSockets = new List<Socket>();
        const int BUFFER_SIZE = 4096;
        private readonly byte[] buffer = new byte[BUFFER_SIZE];
        int port;

        public void Start(int port)
        {
            this.port = port;

            Console.Title = "Server";
            SetupServer();
        }


        private void SetupServer()
        {
            Zephy.Logger.Information("Setting up TCP Listener...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Zephy.Logger.Information("Successfully set up TCP Listener.");
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        public void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Zephy.Logger.Information($"Client from '{socket.LocalEndPoint}' Connected, Waiting for requests...");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            received = current.EndReceive(AR, out SocketError err);

            if (err != SocketError.Success)
            {
                Zephy.Logger.Information($"Client '{current.LocalEndPoint}' forcefully disconnected!");
                current.Close();
                clientSockets.Remove(current);
                return;

            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.UTF8.GetString(recBuf);

            // Handle the packet
            int res = PacketReceiver.Handle(recBuf, current);
            if (res == PacketReceiver.SHUTDOWN) return;

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        public void SendPacket(Packet packet, Socket sendTo)
        {
            Console.WriteLine($"Sending packet to {sendTo.RemoteEndPoint}.");
            sendTo.Send(packet.Buffer);
        }
    }
}
