using Definitions.NetworkPackages;
using Lidgren.Network;
using System;
using System.Net;
using System.Threading;


namespace Server
{
    public class NetworkServerManager
    {
        private readonly NetServer Server;
        private NetPeerConfiguration Config;
        private NetIncomingMessage IncomingPackage { get; set; }
        private Thread ListenThread { get; set; }

        public event EventHandler NewConnection;
        public event EventHandler IncomingDataPackage;
        

        public NetworkServerManager()
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14242;
            Config.MaximumConnections = 200;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

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

                            // Accept new player
                            this.IncomingPackage.SenderConnection.Approve();
                            Console.WriteLine("Client connected {0}...", IncomingPackage.SenderEndpoint.Address);

                            // Raise event
                            OnNewConnection(IncomingPackage.SenderConnection);

                            // Send handshake
                            var handshake = new HandshakePackage();
                            Server.SendMessage(handshake.Encrypt(Server), IncomingPackage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                            break;

                        case NetIncomingMessageType.Data:
                            this.OnIncomingDataPackage(IncomingPackage);
                            break;
                    }
                }
            }
        }

        public void Send(Definitions.Snake snake, BasePackage package)
        {
            this.Server.SendMessage(package.Encrypt(this.Server), snake.Connection, NetDeliveryMethod.ReliableOrdered, 0);
        }

        protected virtual void OnNewConnection(NetConnection connection)
        {
            var handler = NewConnection;
            if (handler != null)
            {
                handler(connection, EventArgs.Empty);
            }
        }

        protected virtual void OnIncomingDataPackage(NetIncomingMessage incoming)
        {
            var handler = NewConnection;
            if (handler != null)
            {
                this.IncomingDataPackage(incoming, EventArgs.Empty);
            }
        }
    }
}
