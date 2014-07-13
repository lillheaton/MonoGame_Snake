using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTest_V1.Particles
{
    public class ParticleManager
    {
        // particles
        private List<BaseParticle> _particles;

        // parent
        private GameManager _game;

        public ParticleManager(GameManager game)
        {
            this._game = game;

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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BaseParticle particle in this._particles)
            {
                particle.Draw(spriteBatch);
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
