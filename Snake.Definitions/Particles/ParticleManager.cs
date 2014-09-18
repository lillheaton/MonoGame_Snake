using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Definitions.Particles
{
    public class ParticleManager
    {
        // particles
        private List<BaseParticle> _particles;

        public ParticleManager()
        {
            this.Init();
        }

        public void Init()
        {
            this._particles = new List<BaseParticle>();
        }


        public void Update(GameTime gameTime)
        {
            for (int i = this._particles.Count; i-- > 0; )
            {
                var particle = this._particles[i];
                particle.Update(gameTime);

                if (!particle.Alive)
                {
                    this._particles.RemoveAt(i);
                }
            }
        }

        public void AddParticle(BaseParticle particle)
        {
            this._particles.Add(particle);
        }
        public void AddParticles(BaseParticle[] particles)
        {
            this._particles.AddRange(particles);
        }
    }
}
