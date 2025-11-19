using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    public class Projectile : Collideable
    {
        //protected Texture2D pic;
        protected string picName;
        //Rectangle prevPicRect;????????
        protected float speed;
        //protected float rotation; //in degrees
        protected Color color;
        protected float power;
        protected Game1 game;
        bool showRect = false;
        //protected Vector2 pos;
        //protected float scale;
        protected int width, height;
        public Projectile(float x, float y, Texture2D _pic, float _speed, Game1 _game)
        {
            //defaults
            pic = _pic;
            pos = new Vector2(x, y);
            scale = 1;
            picRect = new Rectangle((int)x,(int) y, pic.Width, pic.Height);
            //prevPicRect = new Rectangle(x, y, 50, 50); ?????
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = Color.White;
            game = _game;
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
            
        }
        public Projectile(Texture2D _pic, Rectangle rect, float _speed, Color c, Game1 _game)
        {
            pic = _pic;
            pos = new Vector2((float)rect.X, (float)rect.Y);
            width = rect.Width; height = rect.Height;
            scale = (((float)width / pic.Width) + ((float)height / pic.Height)) / 2.0f;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
            game = _game;
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }

        public Projectile(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _power, float _speed, Color _color, Game1 _game)
        {
            pic = _pic;
            pos = new Vector2((float)_picRect.X, (float)_picRect.Y);
            width = _picRect.Width; height = _picRect.Height;
            scale = (((float)width / pic.Width) + ((float)height / pic.Height)) / 2.0f;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            rotation = _rotation;
            speed = _speed;
            power = _power;
            color = _color;
            game = _game;
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }
        public Projectile(string name, Rectangle rect, float _speed, Color c, Game1 _game)
        {
            picName = name;
            pos = new Vector2((float)rect.X, (float)rect.Y);
            scale = 1f;
            width = rect.Width; height = rect.Height;
            picRect = rect;
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = c;
            game = _game;
            origin = new Vector2(width / 2f, height / 2f);
        }
        public Projectile(float x, float y, string name, float _speed,int _width, int _height, Game1 _game)
        {
            picName = name;
            pic = null;
            pos = new Vector2(x, y);
            width = _width; height = _height;
            scale = 1f;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            rotation = 0.0f;
            speed = _speed;
            power = 1.0f;
            color = Color.White;
            game = _game;
            origin = new Vector2(width / 2f, height / 2f);
        }
        public Projectile(string name, Rectangle _picRect, float _rotation, int _velocity, float _power, Color _color)
        {
            picName = name;
            pic = null;
            pos = new Vector2((float)_picRect.X, (float)_picRect.Y);
            width = _picRect.Width; height = _picRect.Height;
            scale = 1f;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
            //speed = _speed;
            power = _power;
            color = _color;
            origin = new Vector2(width / 2f, height / 2f);
        }
        public virtual void UpdateMove(GameTime gameTime)
        {

            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

            pos.X += (float)(speed * Math.Sin(rotation ) * elapsedTime);
            pos.Y += (float)(speed * -Math.Cos(rotation ) * elapsedTime);
            picRect.X = (int)pos.X;
            picRect.Y = (int)pos.Y;
        }

        public virtual void Draw(Rectangle scope, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pic, new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, picRect.Width, picRect.Height), null,
            //                color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
            spriteBatch.Draw(pic, new Vector2(pos.X - scope.X, pos.Y - scope.Y), null, color, rotation, origin, scale,SpriteEffects.None, 0f);
            if (showRect)
            {
                Rectangle rect = getBoundRect();
                Rectangle rect2 = picRect;
                spriteBatch.Draw(pic, new Rectangle(rect.Left, rect.Top, rect.Width, 5), Color.Black);
                spriteBatch.Draw(pic, new Rectangle(rect.Left, rect.Bottom, rect.Width, 5), Color.Black);
                spriteBatch.Draw(pic, new Rectangle(rect.Left, rect.Top, 5, rect.Height), Color.Black);
                spriteBatch.Draw(pic, new Rectangle(rect.Right, rect.Top, 5, rect.Height), Color.Black);

                spriteBatch.Draw(pic, new Rectangle(rect2.Left, rect2.Top, rect2.Width, 5), Color.Red);
                spriteBatch.Draw(pic, new Rectangle(rect2.Left, rect2.Bottom, rect2.Width, 5), Color.Red);
                spriteBatch.Draw(pic, new Rectangle(rect2.Left, rect2.Top, 5, rect2.Height), Color.Red);
                spriteBatch.Draw(pic, new Rectangle(rect2.Right, rect2.Top, 5, rect2.Height), Color.Red);
            }
        }
        public virtual Projectile Predict(GameTime gameTime)
        {
            Projectile futureSelf = new Projectile(pos.X, pos.Y, pic,speed, game);
            futureSelf.setRotation(rotation);
            futureSelf.UpdateMove(gameTime);
            return futureSelf;
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            float factor = scope.Width / mini.Width;
            //batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), Color.White);
            //batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), null,
            //                color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
            batch.Draw(pic, new Vector2(mini.X + ((pos.X - scope.X) / factor), mini.Y + ((pos.Y - scope.Y) / factor)), null, color, rotation, origin,1f/factor, SpriteEffects.None, 0f);
        }
        public override void collidedWith(Collideable other)
        {
            game.remove(other);
            game.remove(this);
        }
        public string getPicName()
        {
            return picName;
        }
        public void setPic(Texture2D _pic)
        {
            pic = _pic;
            scale = (((float)width / pic.Width) + ((float)height / pic.Height)) / 2.0f;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }
    }
}
