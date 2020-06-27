using Packets;
using Packets.General;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class BroadcastReceiver
    {
        private readonly int port;
        private UdpClient client;

        public BroadcastReceiver(int port)
        {
            this.port = port;
            client = new UdpClient();
            client.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        ~BroadcastReceiver()
        {
            client.Close();
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
            Console.WriteLine("trying to receive UDP BC...");
            UdpReceiveResult res = await client.ReceiveAsync();

            // start new thread as soon as we have received any data (so we can
            // instantly start handling the next one while this one sends back)
            StartReceive();

            if (Packet.GetPacketType(res.Buffer) != IdentifyPacket.TYPE) return;

            IdentifyPacket recvPacket = new IdentifyPacket(res.Buffer);

            if (recvPacket.Src != "CLIENT") return;

            Console.WriteLine($"received IdentifyPacket from Client");

            // Send Packet with identifier "SERVER" back to client
            //to indicate that it was the server that sent data back
            IdentifyPacket sendPacket = new IdentifyPacket("SERVER");
            await client.Client.SendToAsync(sendPacket.Buffer, SocketFlags.None, new IPEndPoint(res.RemoteEndPoint.Address, res.RemoteEndPoint.Port));
            Console.WriteLine($"sending IdentifyPacket from Server back to sender.");
        }
    }
}
