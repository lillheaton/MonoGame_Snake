using System;

using Lidgren.Network;

namespace Definitions.EventArguments
{
    public class ConnectionEventArgs : EventArgs
    {
        public NetConnection NetConnection { get; set; }

        public ConnectionEventArgs(NetConnection netConnection)
        {
            this.NetConnection = netConnection;
        }
    }
}
