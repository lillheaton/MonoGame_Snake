using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Definitions.NetworkPackages
{
    public class StandardPackage : IPackage<StandardPackageData>
    {
        private StandardPackageData data;

        public StandardPackage(StandardPackageData data)
        {
            this.data = data;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            throw new NotImplementedException();
        }

        public static StandardPackageData Decrypt(NetIncomingMessage package)
        {
            throw new NotImplementedException();
        }

        StandardPackageData IPackage<StandardPackageData>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }
    }
}
