using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace MonoGameTest_V1
{
    public class NetworkClientHelper
    {
        NetClient Client { get; set; }
        NetPeerConfiguration Config { get; set; }

        public NetworkClientHelper()
        {
            Config = new NetPeerConfiguration("SnakeGame");
            Config.Port = 14242;

            Client = new NetClient(Config);
        }
    }
}
