﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Lidgren.Network;

using Snake.Server.Packages;

namespace Snake.Server
{
    public class Program
    {
        private static NetServer Server;
        private static NetPeerConfiguration Config;
        private static List<NetConnection> players;

        static Program()
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14242;
            Config.MaximumConnections = 200;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            players = new List<NetConnection>();
            Server = new NetServer(Config);
        }

        static void Main(string[] args)
        {
            Server.Start();
            Console.WriteLine("Server started...");

            NetIncomingMessage incomingPackage;
            
            while (true)
            {
                incomingPackage = Server.ReadMessage();

                if (incomingPackage != null)
                {
                    switch (incomingPackage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            incomingPackage.SenderConnection.Approve();
                            Console.WriteLine("Client connected {0}...", incomingPackage.SenderEndpoint.Address);
                            players.Add(incomingPackage.SenderConnection);

                            var handshake = new HandshakePackage();
                            Server.SendMessage(handshake.Encrypt(Server), incomingPackage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                            break;

                        case NetIncomingMessageType.Data:

                            break;
                    }
                }

                Thread.Sleep(30);
            }
        }
    }
}
