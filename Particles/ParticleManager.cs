using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Particles
{
    public class ParticleManager
    {
        // particles
        public List<Vector2> Particles { get; set; }

        // parent
        private GameManager _game;

        public ParticleManager(GameManager game)
        {
            this._game = game;

            this.Init();
        }

        public void Init()
        {
            this.Particles = new List<Vector2>();
        }


        //public void Update(GameTime gameTime)
        //{
        //    for (int i = this.Particles.Count; i-- > 0; )
        //    {
        //        var particle = this.Particles[i];
        //        particle.Update(gameTime);

        //        if (!particle.Alive)
        //        {
        //            this.Particles.RemoveAt(i);
        //        }
        //    }
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 vector in this.Particles)
            {
                RectangleGraphicsHelper.DrawRectangle(spriteBatch, new Rectangle((int)vector.X, (int)vector.Y, 10, 10), Color.Red);
            }
        }




        //public void AddParticle(BaseParticle particle)
        //{
        //    this.Particles.Add(particle);
        //}
        //public void AddParticles(BaseParticle[] particles)
        //{
        //    this.Particles.AddRange(particles);
        //}
    }
}
