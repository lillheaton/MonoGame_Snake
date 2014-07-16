using Lidgren.Network;
using Snake.Server;

namespace MonoGameTest_V1
{
    public class NetworkClientManager
    {
        public bool IsConnected { get; private set; }
        private NetClient Client { get; set; }
        private NetPeerConfiguration Config { get; set; }
        private NetIncomingMessage IncomingPackage { get; set; }

        public NetworkClientManager()
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14243;

            Client = new NetClient(Config);
            Client.Start();
        }

        public void Connect()
        {
            Client.Connect("localhost", 14242);
        }

        public void Listen()
        {
            if ((IncomingPackage = Client.ReadMessage()) != null)
            {
                switch (IncomingPackage.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        this.ManageIncomingData(IncomingPackage);
                        break;
                }
            }
        }

        //public void Send(BasePackage package)
        //{
        //    Client.SendMessage(package.Encrypt(Client), NetDeliveryMethod.ReliableOrdered, 0);
        //}

        public void ManageIncomingData(NetIncomingMessage dataPackage)
        {
            switch (dataPackage.PeekByte())
            {
                case (byte)PackageType.Handshake:
                    this.IsConnected = true;
                    break;


            }
        }
    }
}
