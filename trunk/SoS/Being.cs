using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoS;

namespace SOS
{
    class Being
    {
        protected Texture2D pic;
        protected Rectangle picRect;
        //Rectangle prevPicRect;????????
        protected float xVel = .1f, yVel = 0f;
        protected float rotation; //in degrees
        protected Color color;
        protected float health;

        KeyboardState keyboard;

        public Being(int x, int y, Texture2D _pic)
        {
            //defaults
            pic = _pic;
            picRect = new Rectangle(x, y, pic.Width, pic.Height);
            //prevPicRect = new Rectangle(x, y, 50, 50); ?????
            rotation = 0.0f;
            //velocity = 0;
            health = 1.0f;
            color = Color.White;
        }
        public Being(Texture2D _pic, Rectangle rect, Color c)
        {
            pic = _pic;
            picRect = rect;
            rotation = 0.0f;
            //velocity = 0;
            health = 1.0f;
            color = c;
        }

        public Being(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _health, Color _color)
        {
            pic = _pic;
            picRect = _picRect;
            //prevPicRect = _picRect;??????
            rotation = _rotation;
           // velocity = _velocity;
            health = _health;
            color = _color;
        }
        public virtual void UpdateMove(GameTime gameTime)
        {
            
            int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            picRect.X += (int)(xVel * elapsedTime);
            picRect.Y += (int)(yVel * elapsedTime);
            if (picRect.X <= 0)
            {
                picRect.X += 2 * (0 - picRect.X);
                
                xVel *= -1f;
            }
            if (picRect.Y <= 0)
            {
                picRect.Y += 2 * (0 - picRect.Y);
                yVel *= -1f;
            }
            if (picRect.X + pic.Width >= Game1.map.getWidth())
            {
                picRect.X -= 2 * (picRect.X + pic.Width - Game1.map.getWidth());
                xVel *= -1f;
            }
            if (picRect.Y + pic.Height >= Game1.map.getHeight())
            {
                picRect.Y -= 2 * (picRect.Y + pic.Height - Game1.map.getHeight());
                yVel *= -1f;
            } 
     
            //?????????????????????????????????????
            /*if (gameTime.TotalGameTime.Seconds >= 2)
            {
                prevPicRect = picRect;
            } */ 
            /*if (keyboard.IsKeyDown(Keys.Up))
            {
                picRect.Y += velocity * elapsedTime;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                picRect.Y -= velocity * elapsedTime;
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                picRect.X -= velocity * elapsedTime;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                picRect.X += velocity * elapsedTime;
            } */
            //?????????????????????????????????????
            /*if (keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyUp(Keys.Left) && keyboard.IsKeyUp(Keys.Right))
            {
                velocity /= 10;
            }*/

            

        }

        public virtual void Draw(Rectangle scope, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pic, new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, pic.Width, pic.Height), Color.White);
            spriteBatch.Draw(pic,new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, picRect.Width, picRect.Height), null, 
                            color, rotation,new Vector2(pic.Width/2,pic.Height/2), SpriteEffects.None, 0f);
        }

        public virtual Being Predict(GameTime gameTime)
        {
            /*
            Rectangle _picRect = new Rectangle(picRect.X, picRect.Y, picRect.Width, picRect.Height);
            Being being = new Being(pic, picRect, rotation, velocity, health, color);

            int slope = (_picRect.Y - picRect.Y) / (_picRect.X - picRect.X);
            being.picRect.X += _picRect.X - picRect.X;
            being.picRect.Y += _picRect.Y - picRect.Y;

            picRect = new Rectangle(_picRect.X, _picRect.Y, _picRect.Width, _picRect.Height);

            return being; */
            Being futureSelf = new Being(picRect.X, picRect.Y, pic);
            futureSelf.setVelocity(new Vector2(xVel, yVel));
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
        public virtual bool intersects(Rectangle r)
        {
            return picRect.Intersects(r);
        }
        public void setVelocity(Vector2 vel)
        {
            xVel = vel.X;
            yVel = vel.Y;
        }
        public Point getPoint()//get actual point on Map
        {
            return new Point(picRect.X, picRect.Y);
        }
        public Point getPoint(Rectangle scope)//get point within scope
        {
            return new Point(picRect.X - scope.X, picRect.Y - scope.Y);
        }
        public void setRotation(float angle)
        {
            rotation = angle;
        }
    }
}
