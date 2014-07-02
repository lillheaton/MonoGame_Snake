using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Lidgren.Network;

namespace Snake.Server
{
    public class Program
    {
        private static NetServer Server;
        private static NetPeerConfiguration Config;

        static Program()
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14242;
            Config.MaximumConnections = 200;
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

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

                            var helloMessage = new HelloPackage();
                            helloMessage.HelloMessage = "Tjena!";

                            Server.SendMessage(helloMessage.Encrypt(Server), incomingPackage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                            break;
                    }
                }

                Thread.Sleep(30);
            }
        }
    }
}
