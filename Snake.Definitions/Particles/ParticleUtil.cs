using System;
using Microsoft.Xna.Framework;

namespace Definitions.Particles
{
    public static class ParticleUtil
    {
        public static void ParticleExplosion(ParticleManager manager, Vector2 position, Color color)
        {
            int particleCount = 16;

            float angle = 0.0f;
            float delta = (float)(Math.PI * 2.0) / particleCount;

            Vector2 size = new Vector2(10.0f, 10.0f);
            float liveCount = 400.0f;
            float speed = 100.0f;

            while (particleCount-- > 0)
            {
                manager.AddParticle(new BaseParticle(
                    position,
                    new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed),
                    size,
                    color,
                    liveCount
                ));
                angle += delta;
            }
        }
    }
}
