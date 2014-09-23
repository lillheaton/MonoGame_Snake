using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Definitions.Particles;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace Definitions.NetworkPackages
{
    public class BaseParticlePackage : IPackage<List<Rectangle>>
    {
        private List<BaseParticle> particles;

        public BaseParticlePackage(List<BaseParticle> particles)
        {
            this.particles = particles;
        }

        public NetOutgoingMessage Encrypt(NetPeer peer)
        {
            var message = peer.CreateMessage();
            message.Write((byte)PackageType.BaseParticle);

            message.Write(this.particles.Count);

            foreach (var baseParticle in this.particles)
            {
                // Position
                message.Write(baseParticle.Position.X);
                message.Write(baseParticle.Position.Y);

                // Size
                message.Write(baseParticle.Size.X);
                message.Write(baseParticle.Size.Y);
            }
            
            return message;
        }

        public static List<Rectangle> Decrypt(NetIncomingMessage package)
        {
            package.ReadByte();

            var numberOfRectangles = package.ReadInt32();
            var rectangles = new List<Rectangle>(numberOfRectangles);
            
            for (int i = 0; i < numberOfRectangles; i++)
            {
                var rectangle = new Rectangle();

                // Position
                rectangle.X = (int)package.ReadFloat();
                rectangle.Y = (int)package.ReadFloat();

                // Size
                rectangle.Width = (int)package.ReadFloat();
                rectangle.Height = (int)package.ReadFloat();

                rectangles.Add(rectangle);
            }
            
            return rectangles;
        }

        List<Rectangle> IPackage<List<Rectangle>>.Decrypt(NetIncomingMessage package)
        {
            return Decrypt(package);
        }
    }
}
