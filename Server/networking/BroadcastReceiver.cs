﻿using Packets;
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
            UdpReceiveResult receivedResult = await client.ReceiveAsync();
            // start new thread as soon as we have received any data (so we can
            // instantly start handling the next one while this one sends back)
            StartReceive();

            Console.WriteLine(Packet.GetPacketType(receivedResult.Buffer));
            if (Packet.GetPacketType(receivedResult.Buffer) != IdentifyPacket.TYPE) return;

            IdentifyPacket recvPacket = new IdentifyPacket(receivedResult.Buffer);
            if (recvPacket.Src != "CLIENT") return;


            IPEndPoint receivedFrom = new IPEndPoint(receivedResult.RemoteEndPoint.Address, receivedResult.RemoteEndPoint.Port);
            Console.WriteLine($"received IdentifyPacket from Client {receivedFrom.Address}.");

            // Send Packet with identifier "SERVER" back to client
            //to indicate that it was the server that sent data back
            IdentifyPacket sendPacket = new IdentifyPacket(new IdentifyPacketData("SERVER"));
            await client.Client.SendToAsync(sendPacket.Buffer, SocketFlags.None, receivedFrom);
            Console.WriteLine($"sending IdentifyPacket from Server back to sender.");
        }
    }
}
