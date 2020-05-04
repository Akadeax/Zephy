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
            socket.Listen(500);
        }

        public void Accept()
        {
            socket.BeginAccept(OnAccepted, null);
        }

        private void OnAccepted(IAsyncResult result)
        {
            Socket clientSocket = socket.EndAccept(result);
            BeginNewBufferReceive(clientSocket);
            Accept();
        }

        private void OnDataReceived(IAsyncResult result)
        {
            Socket clientSocket = (Socket)result.AsyncState;
            if (clientSocket == null) return;

            int bufferSize = clientSocket.EndReceive(result);
            byte[] packet = new byte[bufferSize];
            Array.Copy(buffer, packet, packet.Length);

            // Handle the packet
            PacketHandler.Handle(packet, clientSocket);

            BeginNewBufferReceive(clientSocket);
        }

        private void BeginNewBufferReceive(Socket clientSocket)
        {
            buffer = new byte[1024];
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnDataReceived, clientSocket);
        }
    }
}
