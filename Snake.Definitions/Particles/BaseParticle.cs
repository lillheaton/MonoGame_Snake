﻿using System;
using Microsoft.Xna.Framework;

namespace Definitions.Particles
{
    public class BaseParticle
    {
        // base stats
        public  Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }

        protected Vector2 _positionVelocity;
        protected Vector2 _sizeVelocity;
        protected float _rotation;
        protected float _rotationVelocity;
        protected Color _color;

        protected TimeSpan _liveCount;

        public bool Alive { get { return _liveCount.Ticks > 0; } }

        protected BaseParticle()
            : this(Vector2.Zero, Vector2.Zero, Vector2.Zero, Color.White, 0.0f)
        {}

        public BaseParticle(Vector2 position, Vector2 velocity, Vector2 size, Color color, float liveCount)
        {
            this.Position = position;
            _positionVelocity = velocity;
            this.Size = size;
            _sizeVelocity = Vector2.Zero;
            _rotation = 0.0f;
            _rotationVelocity = 0.0f;
            _color = color;

            _liveCount = TimeSpan.FromMilliseconds(liveCount);
        }

        public void Update(GameTime gameTime)
        {
            this._liveCount -= gameTime.ElapsedGameTime;

            if (this.Alive)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                this.Position += _positionVelocity * elapsed;
                this.Size += _sizeVelocity * elapsed;
                _rotation += _rotationVelocity * elapsed;
            }
        }
    }
}
