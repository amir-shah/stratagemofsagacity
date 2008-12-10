using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    public class Projectile
    {
        protected Texture2D pic;
        protected string picName;
        protected Rectangle picRect;
        //Rectangle prevPicRect;????????
        protected float speed;
        protected float rotation; //in degrees
        protected Color color;
        protected float power;


        public Projectile(int x, int y, Texture2D _pic, float _speed)
        {
            //defaults
            pic = _pic;
            picRect = new Rectangle(x, y, pic.Width, pic.Height);
            //prevPicRect = new Rectangle(x, y, 50, 50); ?????
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = Color.White;
        }
        public Projectile(Texture2D _pic, Rectangle rect,float _speed, Color c)
        {
            pic = _pic;
            picRect = rect;
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
        }

        public Projectile(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _power,float _speed, Color _color)
        {
            pic = _pic;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
            speed = _speed;
            power = _power;
            color = _color;
        }
        public Projectile(string name, Rectangle rect, float _speed, Color c)
        {
            picName = name;
            picRect = rect;
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
        }

        public Projectile(string name, Rectangle _picRect, float _rotation, int _velocity, float _power, float _speed, Color _color)
        {
            picName = name;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
            speed = _speed;
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
            Projectile futureSelf = new Projectile(picRect.X, picRect.Y, pic,speed);
            futureSelf.setRotation(rotation);
            GameTime futureTime = new GameTime(new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 2));
            futureSelf.UpdateMove(futureTime);
            return futureSelf;
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            double factor = scope.Width / mini.Width;
            //batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), Color.White);
            batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), null,
                            color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
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
