using System;

using Lidgren.Network;

namespace Definitions.EventArguments
{
    public class PackageEventArgs : EventArgs
    {
        public NetIncomingMessage IncomingPackage { get; set; }
        
        public PackageEventArgs(NetIncomingMessage incomingPackage)
        {
            this.IncomingPackage = incomingPackage;
        }
    }
}
