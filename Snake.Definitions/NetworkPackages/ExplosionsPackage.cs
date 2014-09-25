using System.Collections.Generic;

using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class ExplosionsPackage : IPackage<List<Vector2>>
    {
        private readonly List<Vector2> explosions;

        public ExplosionsPackage(List<Vector2> explosions)
        {
            this.explosions = explosions;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var message = peer.CreateMessage();
            message.Write((byte)PackageType.Particle);

            message.Write(this.explosions.Count);
            foreach (var particle in this.explosions)
            {
                message.Write(particle.X);
                message.Write(particle.Y);
            }
            
            return message;
        }

        public static List<Vector2> Decrypt(NetIncomingMessage package)
        {
            package.ReadByte();
            var list = new List<Vector2>();

            var numberOfParticles = package.ReadInt32();
            for (int i = 0; i < numberOfParticles; i++)
            {
                list.Add(new Vector2(package.ReadFloat(), package.ReadFloat()));
            }

            return list;
        }

        List<Vector2> IPackage<List<Vector2>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }
    }
}
