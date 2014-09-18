using Lidgren.Network;

using Microsoft.Xna.Framework;

using MonoGameTest_V1;

using Snake.Definitions.NetworkPackages;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Snake.Server
{
    public class NetworkServerManager
    {
        private readonly NetServer Server;
        private NetPeerConfiguration Config;
        private List<MonoGameTest_V1.Snake> players;
        private NetIncomingMessage IncomingPackage { get; set; }
        private Thread ListenThread { get; set; }
        public event EventHandler EventPackage;

        public NetworkServerManager(List<MonoGameTest_V1.Snake> players)
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14242;
            Config.MaximumConnections = 200;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            this.players = players;
            Server = new NetServer(Config);

            ListenThread = new Thread(this.Listen);
            Server.Start();
        }

        public void Connect()
        {
            ListenThread.Start();
        }

        public void Disconnect()
        {
            this.ListenThread.Abort();
        }

        public void Listen()
        {
            while (true)
            {
                while ((this.IncomingPackage = Server.ReadMessage()) != null)
                {
                    switch (this.IncomingPackage.MessageType)
                    {
                        case NetIncomingMessageType.ConnectionApproval:
                            this.IncomingPackage.SenderConnection.Approve();
                            Console.WriteLine("Client connected {0}...", IncomingPackage.SenderEndpoint.Address);

                            players.Add(new MonoGameTest_V1.Snake(new Vector2(10.0f, 0.0f), SnakeDirection.East, IncomingPackage.SenderEndpoint));

                            var handshake = new HandshakePackage();
                            Server.SendMessage(handshake.Encrypt(Server), IncomingPackage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                            break;

                        case NetIncomingMessageType.Data:
                            this.OnIncomingPackage(IncomingPackage);
                            break;
                    }
                }
            }
        }

        public void OnIncomingPackage(NetIncomingMessage incoming)
        {
            if (this.IncomingPackage != null)
            {
                this.EventPackage(incoming, EventArgs.Empty);
            }
        }
    }
}
