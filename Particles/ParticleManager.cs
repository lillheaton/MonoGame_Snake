using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Particles
{
    public class ParticleManager
    {
        // particles
        public List<BaseParticle> Particles;

        public ParticleManager()
        {
            this.Init();
        }

        public void Init()
        {
            this.Particles = new List<BaseParticle>();
        }


        public void Update(GameTime gameTime)
        {
            for (int i = this.Particles.Count; i-- > 0; )
            {
                var particle = this.Particles[i];
                particle.Update(gameTime);

                if (!particle.Alive)
                {
                    this.Particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            foreach (var baseParticle in Particles)
            {
                baseParticle.Draw(sprite);
            }
        }

        public void AddParticle(BaseParticle particle)
        {
            this.Particles.Add(particle);
        }
        public void AddParticles(BaseParticle[] particles)
        {
            this.Particles.AddRange(particles);
        }
    }
}
