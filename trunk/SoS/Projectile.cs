using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    public class Projectile : Collideable
    {
        protected Texture2D pic;
        protected string picName;
        //Rectangle prevPicRect;????????
        protected float speed;
        protected float rotation; //in degrees
        protected Color color;
        protected float power;
        protected Game1 game;

        public Projectile(int x, int y, Texture2D _pic, float _speed, Game1 _game)
        {
            //defaults
            pic = _pic;
            picRect = new Rectangle(x, y, pic.Width, pic.Height);
            //prevPicRect = new Rectangle(x, y, 50, 50); ?????
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = Color.White;
            game = _game;
        }
        public Projectile(Texture2D _pic, Rectangle rect, float _speed, Color c, Game1 _game)
        {
            pic = _pic;
            picRect = rect;
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
            game = _game;
        }

        public Projectile(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _power, float _speed, Color _color, Game1 _game)
        {
            pic = _pic;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
            speed = _speed;
            power = _power;
            color = _color;
            game = _game;
        }
        public Projectile(string name, Rectangle rect, float _speed, Color c, Game1 _game)
        {
            picName = name;
            picRect = rect;
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
            game = _game;
        }

        public Projectile(string name, Rectangle _picRect, float _rotation, int _velocity, float _power, Color _color)
        {
            picName = name;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
            //speed = _speed;
            power = _power;
            color = _color;
        }
        public virtual void UpdateMove(GameTime gameTime)
        {

            int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            picRect.X += (int)(speed * Math.Sin(rotation ) * elapsedTime);
            picRect.Y += (int)(speed * -Math.Cos(rotation ) * elapsedTime);
        }

        public virtual void Draw(Rectangle scope, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, picRect.Width, picRect.Height), null,
                            color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
        }
        public virtual Projectile Predict(GameTime gameTime)
        {
            Projectile futureSelf = new Projectile(picRect.X, picRect.Y, pic,speed, game);
            futureSelf.setRotation(rotation);
            futureSelf.UpdateMove(gameTime);
            return futureSelf;
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            double factor = scope.Width / mini.Width;
            //batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), Color.White);
            batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), null,
                            color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
        }
        public override bool collides(Collideable other)
        {
            return picRect.Intersects(other.getRectangle());
        }
        public override void collidedWith(Collideable other)
        {
            game.remove(other);
            game.remove(this);
        }
        public void setRotation(float angle)
        {
            rotation = angle;
        }
        public virtual bool intersects(Rectangle r)
        {
            return picRect.Intersects(r);
        }
        public string getPicName()
        {
            return picName;
        }
        public void setPic(Texture2D _pic)
        {
            pic = _pic;
        }
    }
}
