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
    public class Being : Collideable
    {
        //protected Texture2D pic;
        //protected Vector2 pos;
        protected float xVel = .1f, yVel = 0f;
        protected bool canMoveUp = true, canMoveDown = true, canMoveLeft = true, canMoveRight = true;
        //protected float rotation; //in degrees
        protected Color color;
        protected float health;
        //protected float scale;
        protected int width, height;
        bool showRect = false;
        KeyboardState keyboard;

        public Being(float x, float y, Texture2D _pic,float _scale)
        {
            //defaults
            pic = _pic;
            pos = new Vector2(x, y);
            
            scale = _scale;
            width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
            //prevPicRect = new Rectangle(x, y, 50, 50); ?????
            rotation = 0.0f;
            //velocity = 0;
            health = 1.0f;
            color = Color.White;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }

        public Being(Texture2D _pic, Rectangle rect, Color c)
        {
            pic = _pic;
            pos = new Vector2((float)rect.X, (float)rect.Y);
            rotation = 0.0f;
            width = rect.Width; height = rect.Height;
            scale = (((float)width/pic.Width) + ((float)height/pic.Height)) / 2.0f;
            //velocity = 0;
            health = 1.0f;
            color = c;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }

        public Being(Texture2D _pic, Rectangle _picRect, float _rotation,float _health, Color _color)
        {
            pic = _pic;
            pos = new Vector2((float)_picRect.X, (float)_picRect.Y);
            width = _picRect.Width; height = _picRect.Height;
            scale = (((float)width / pic.Width) + ((float)height / pic.Height)) / 2.0f;
            rotation = _rotation;
           // velocity = _velocity;
            health = _health;
            color = _color;
            picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            origin = new Vector2(pic.Width / 2f, pic.Height / 2f);
        }
        public virtual void UpdateMove(GameTime gameTime)
        {
            
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            if((xVel > 0 && canMoveRight) || (xVel < 0 && canMoveLeft))
            pos.X += (float)(xVel * elapsedTime);
            if((yVel > 0 && canMoveDown) || (yVel < 0 && canMoveUp))
            pos.Y += (float)(yVel * elapsedTime);
            if (pos.X <= 0)
            {
                pos.X += 2 * (0 - pos.X);
                
                xVel *= -1f;
            }
            if (pos.Y <= 0)
            {
                pos.Y += 2 * (0 - pos.Y);
                yVel *= -1f;
            }
            if (pos.X + pic.Width >= Game1.map.getWidth())
            {
                pos.X -= 2 * (pos.X + picRect.Width - Game1.map.getWidth());
                xVel *= -1f;
            }
            if (pos.Y + pic.Height >= Game1.map.getHeight())
            {
                pos.Y -= 2 * (pos.Y + picRect.Height - Game1.map.getHeight());
                yVel *= -1f;
            }
            canMoveUp = true; canMoveDown = true; canMoveLeft = true; canMoveRight = true;
            picRect.X = (int)pos.X; picRect.Y = (int)pos.Y;
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

        public void setSprite(Texture2D newSprite)
        {
            pic = newSprite;
        }

        public virtual void Draw(Rectangle scope, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, getPos(scope), null, color, rotation,origin,scale, SpriteEffects.None, 0f);
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
            //spriteBatch.Draw(pic, new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, pic.Width, pic.Height), Color.White);
            //spriteBatch.Draw(pic,new Rectangle(picRect.X - scope.X, picRect.Y - scope.Y, picRect.Width, picRect.Height), null, 
            //                color, rotation,new Vector2(pic.Width/2,pic.Height/2), SpriteEffects.None, 0f);
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
            Being futureSelf = new Being(pos.X, pos.Y, pic,scale);
            futureSelf.setVelocity(new Vector2(xVel, yVel));
            futureSelf.UpdateMove(gameTime);
            return futureSelf;
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            double factor = scope.Width / mini.Width;
            //batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), Color.White);
           // batch.Draw(pic, new Rectangle((int)(mini.X + ((picRect.X - scope.X) / factor)), (int)(mini.Y + ((picRect.Y - scope.Y) / factor)), (int)(picRect.Width / factor), (int)(picRect.Height / factor)), null,
            //                color, rotation, new Vector2(pic.Width / 2, pic.Height / 2), SpriteEffects.None, 0f);
            batch.Draw(pic, new Vector2((float)(mini.X + ((pos.X - scope.X) / factor)),(float)( mini.Y + ((pos.Y - scope.Y) / factor))), null, color, rotation, new Vector2(pic.Width / 2, pic.Height / 2),(float)(scale/factor), SpriteEffects.None, 0f);
        }
        public override void collidedWith(Collideable other)
        {
            canMoveUp = false; canMoveDown = false; canMoveLeft = false; canMoveRight = false;
            //setVelocity(new Vector2(0, 0));
        }
        public void setVelocity(Vector2 vel)
        {
            xVel = vel.X;
            yVel = vel.Y;
        }
        public Vector2 getVelocity()
        {
            return new Vector2(xVel, yVel);
        }
        public Vector2 getPos()//get actual point on Map
        {
            return pos;
        }
        public Vector2 getPos(Rectangle scope)//get point within scope
        {
            return new Vector2(pos.X - scope.X, pos.Y - scope.Y);
        }
        public void setMoveUp(bool b)
        {
            canMoveUp = b;
        }
        public void setMoveDown(bool b)
        {
            canMoveDown = b;
        }
        public void setMoveLeft(bool b)
        {
            canMoveLeft = b;
        }
        public void setMoveRight(bool b)
        {
            canMoveRight = b;
        }
    }
}
