using Packets;
using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{

    class ServerSocket
    {
        Socket socket;
        byte[] buffer;

        public ServerSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            socket.Listen(backlog);
        }

        public void Accept()
        {
            // start accepting any TCP connection async
            socket.BeginAccept(OnAccepted, null);
        }

        /// <summary>
        /// Callback when a Client to accept is found
        /// </summary>
        /// <param name="result"></param>
        private void OnAccepted(IAsyncResult result)
        {
            Socket clientSocket = socket.EndAccept(result);
            // start receiving data from the client that just connected (async, w/ callbacks)
            BeginReceive(clientSocket);
            // after accepting current client keep on accepting new ones
            Accept();
        }

        /// <summary>
        /// Callback when any client sends any Data
        /// </summary>
        /// <param name="result"></param>
        private void OnDataReceived(IAsyncResult result)
        {
            Socket clientSocket = (Socket)result.AsyncState;
            if (clientSocket == null) return;

            // output SocketError cause VS throws us an exception otherwise (why??)
            int bufferSize = clientSocket.EndReceive(result, out SocketError err);
            if (err != SocketError.Success)
            {
                bufferSize = 0;
            }

            // Basically just a trimmed buffer
            byte[] packet = new byte[bufferSize];
            Array.Copy(buffer, packet, packet.Length);

            // Handle the packet
            int res = PacketHandler.Handle(packet, clientSocket);
            // If packet is empty, close Socket & return out of function for this socket (it closed)
            if (res == PacketHandler.SHUTDOWN) return;

            BeginReceive(clientSocket);
        }

        private void BeginReceive(Socket clientSocket)
        {
            // Empty buffer and wait for receive from clientSocket again
            buffer = new byte[1024];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnDataReceived, clientSocket);
        }
    }
}
