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
        public List<Rectangle> Particles { get; set; }

        // parent
        private GameManager _game;

        public ParticleManager(GameManager game)
        {
            this._game = game;

            this.Init();
        }

        public void Init()
        {
            this.Particles = new List<Rectangle>();
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
            foreach (Rectangle rect in this.Particles)
            {
                RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, Color.Red);
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
