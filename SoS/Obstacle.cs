using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOS;

namespace SoS
{
    public abstract class Obstacle : Collideable
    {
        //Texture2D sprite;
        int x, y;
        Color miniColor = Color.Black;
        protected Rectangle top, bottom, left, right;
        protected int size = 5;
        public Obstacle() { }
        public Obstacle(Texture2D _pic,int _x, int _y)
        {
            x = _x;
            y = _y;
            pic = _pic;
            rotation = 0f;
            pos = new Vector2((float)x, (float)y);
            origin = new Vector2(0f,0f);
        }
        public virtual void draw(SpriteBatch batch, Rectangle scope)
        {
            batch.Draw(pic, new Rectangle(x-scope.X, y-scope.Y, pic.Width, pic.Height), Color.White);
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            double factor = scope.Width / mini.Width;
            batch.Draw(pic, new Rectangle((int)(mini.X + ((x - scope.X) / factor)), (int)(mini.Y + ((y - scope.Y) / factor)), (int)(pic.Width / factor), (int)(pic.Height / factor)), Color.White);
        }
        public override void collidedWith(Collideable other)
        {
            if (other is Being)
            {
                Being b = (Being)other;
                //Rectangle top, bottom, left, right;
                //int size = 5;
                //top = new Rectangle(picRect.X, picRect.Y, picRect.Width, size);
                //bottom = new Rectangle(picRect.X, picRect.Y + picRect.Height - size, picRect.Width, size);
                //left = new Rectangle(picRect.X, picRect.Y, size, picRect.Height);
                //right = new Rectangle(picRect.X + picRect.Width - size, picRect.Y, size, picRect.Height);
                /*if(other.getBoundRect().Intersects(top))
                    b.setMoveDown(false);
                if (other.getBoundRect().Intersects(bottom))
                    b.setMoveUp(false);
                if (other.getBoundRect().Intersects(left))
                    b.setMoveRight(false);
                if (other.getBoundRect().Intersects(right))
                    b.setMoveLeft(false);*/
                if (b.getVelocity().Y > 0)
                {
                    b.setMoveDown(false);
                   // b.set
                }
                
            }
        }
    }
}
