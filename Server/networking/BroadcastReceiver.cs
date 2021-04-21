using packets;
using packets.general;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class BroadcastReceiver
    {
        readonly UdpClient client;
        readonly int clientListenPort;

        public BroadcastReceiver(int port, int clientListenPort)
        {
            client = new UdpClient();
            client.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            this.clientListenPort = clientListenPort;
        }

        ~BroadcastReceiver()
        {
            Close();
        }
        public void Close()
        {
            client.Close();
        }

        /// <summary>
        /// Starts the Thread-calling loop that accepts UDP Shouts by clients
        /// </summary>
        public void StartReceive()
        {
            Task.Run(ReceiveDataAsync);
        }

        private async Task ReceiveDataAsync()
        {
            Zephy.Logger.Information("trying to receive UDP BC...");
            UdpReceiveResult receivedResult = await client.ReceiveAsync();
            // start new thread as soon as we have received any data (so we can
            // instantly start handling the next one while this one sends back)
            StartReceive();

            if (Packet.GetPacketType(receivedResult.Buffer) != IdentifyPacket.TYPE) return;

            IdentifyPacket recvPacket = new IdentifyPacket(receivedResult.Buffer);
            if (recvPacket.Data.src != "CLIENT") return;


            IPEndPoint receivedFrom = new IPEndPoint(receivedResult.RemoteEndPoint.Address, clientListenPort);
            Zephy.Logger.Information($"received IdentifyPacket from Client {receivedFrom.Address}.");

            // Send Packet with identifier "SERVER" back to client
            // to indicate that it was the server that sent data back
            IdentifyPacket sendPacket = new IdentifyPacket(new IdentifyPacketData("SERVER"));

            await client.Client.SendToAsync(sendPacket.Buffer, SocketFlags.None, receivedFrom);
            Zephy.Logger.Information($"sending IdentifyPacket from Server back to sender.");
        }
    }
}
