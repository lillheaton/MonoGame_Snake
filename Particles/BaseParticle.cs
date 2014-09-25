using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Particles
{
    public class BaseParticle
    {
        protected Vector2 _position;
        protected Vector2 _positionVelocity;
        protected Vector2 _size;
        protected Vector2 _sizeVelocity;
        protected float _rotation;
        protected float _rotationVelocity;
        protected Color _color;

        protected TimeSpan _liveCount;

        public bool Alive { get { return this._liveCount.Ticks > 0; } }

        protected BaseParticle()
            : this(Vector2.Zero, Vector2.Zero, Vector2.Zero, Color.White, 0.0f)
        {}

        public BaseParticle(Vector2 position, Vector2 velocity, Vector2 size, Color color, float liveCount)
        {
            this._position = position;
            this._positionVelocity = velocity;
            this._size = size;
            this._sizeVelocity = Vector2.Zero;
            this._rotation = 0.0f;
            this._rotationVelocity = 0.0f;
            this._color = color;

            this._liveCount = TimeSpan.FromMilliseconds(liveCount);
        }

        public void Update(GameTime gameTime)
        {
            this._liveCount -= gameTime.ElapsedGameTime;

            if (this.Alive)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                this._position += this._positionVelocity * elapsed;
                this._size += this._sizeVelocity * elapsed;
                this._rotation += this._rotationVelocity * elapsed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var rect = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_size.X,
                (int)_size.Y
            );
            RectangleGraphicsHelper.DrawRectangle(spriteBatch, rect, _color);
        }
    }
}
