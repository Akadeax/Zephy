using Packets;
using Packets.general;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Basis for all UDP (!) communication between Server and clients.
    /// Receives all UDP Broadcasts from clients and informs them of the
    /// IP Address of the Zephy instance.
    /// </summary>
    class BroadcastReceiver
    {
        private const int UDP_PORT = 6556;

        readonly UdpClient client;

        public BroadcastReceiver()
        {
            client = new UdpClient(new IPEndPoint(IPAddress.Any, UDP_PORT))
            {
                EnableBroadcast = true
            };
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

            Zephy.Logger.Information($"received IdentifyPacket from Client {receivedResult.RemoteEndPoint.Address}.");

            // Send Packet with identifier "SERVER" back to client
            // to indicate that it was Server instance that sent data back
            IdentifyPacket sendPacket = new IdentifyPacket(new IdentifyPacketData("SERVER"));

            await client.Client.SendToAsync(sendPacket.Buffer, SocketFlags.None, receivedResult.RemoteEndPoint);
            Zephy.Logger.Information($"sending IdentifyPacket from Server back to sender.");
        }
    }
}
