using System.Collections.Generic;
using System.Linq;

using Lidgren.Network;
using System;
using System.Threading;

using Snake.Definitions;

namespace Snake.Server
{
    public class Program
    {
        private static NetworkServerManager Server;
        private static List<MonoGameTest_V1.Snake> Players { get; set; }

        static Program()
        {
            Players = new List<MonoGameTest_V1.Snake>();

            Server = new NetworkServerManager(Players);
            Server.EventPackage += Server_EventPackage;
            Server.Connect();
        }

        static void Server_EventPackage(object sender, EventArgs e)
        {
            var incomingPackage = sender as NetIncomingMessage;
            var player = Players.FirstOrDefault(s => s.SenderEndpoint == incomingPackage.SenderEndpoint);
            var packageType = (PackageType)incomingPackage.ReadByte();

            switch (packageType)
            {
                case PackageType.KeyboardInput:
                    player.UpdateInput((Direction)incomingPackage.ReadByte());
                    break;
            }
        }

        static void Main(string[] args)
        {
            // Keep the program alive!
            while (true)
            {
                Thread.Sleep(30);
            }
        }
    }
}
