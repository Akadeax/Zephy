using Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{

    /// <summary>
    /// Basis of TCP (!) Communication between Server and all clients.
    /// Runs callback loop of 'wait for request, then respond' with
    /// all connected sockets.
    /// </summary>
    class ServerSocket
    {
        public readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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

        private void AcceptCallback(IAsyncResult res)
        {
            try
            {
                Socket socket;

                socket = serverSocket.EndAccept(res);

                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
                Zephy.Logger.Information($"Client from '{socket.LocalEndPoint}' Connected, Waiting for requests...");

                serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch(Exception)
            {
                Zephy.Logger.Error("An error has occured when accepting a socket.");
            }
        }

        private void ReceiveCallback(IAsyncResult res)
        {
            Socket current = (Socket)res.AsyncState;
            int received;

            received = current.EndReceive(res, out SocketError err);

            // = forceful close
            if (err != SocketError.Success)
            {
                DisconnectHandler.OnDisconnect(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);

            // = graceful close
            if (recBuf.Length == 0)
            {
                DisconnectHandler.OnDisconnect(current);
                return;
            }

            PacketReceiver.Handle(recBuf, current);

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

        public void SendPacket(Packet packet, Socket sendTo)
        {
            Zephy.Logger.Information($"Sending packet of type {packet.PacketType} to {sendTo.RemoteEndPoint}.");
            try
            {
                sendTo.Send(packet.Buffer);
            }
            catch(Exception e)
            {
                Zephy.Logger.Error($"Fatal error, failed to send Packet: {e}");
            }
        }
    }
}
