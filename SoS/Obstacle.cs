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
        Texture2D sprite;
        int x, y;
        Color miniColor = Color.Black;
        public Obstacle() { }
        public Obstacle(Texture2D pic,int _x, int _y)
        {
            x = _x;
            y = _y;
            sprite = pic;
        }
        public virtual void draw(SpriteBatch batch, Rectangle scope)
        {
            batch.Draw(sprite, new Rectangle(x-scope.X, y-scope.Y, sprite.Width, sprite.Height), Color.White);
        }
        public virtual void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            double factor = scope.Width / mini.Width;
            batch.Draw(sprite, new Rectangle((int)(mini.X + ((x - scope.X) / factor)), (int)(mini.Y + ((y - scope.Y) / factor)), (int)(sprite.Width / factor), (int)(sprite.Height / factor)), Color.White);
        }
        public override bool collides(Collideable other)
        {
            return picRect.Intersects(other.getRectangle());
        }
        public override void collidedWith(Collideable other)
        {
            if (other is Being)
            {
                Being b = (Being)other;
                Rectangle top, bottom, left, right;
                int size = 5;
                top = new Rectangle(picRect.X, picRect.Y, picRect.Width, size);
                bottom = new Rectangle(picRect.X, picRect.Y + picRect.Height - size, picRect.Width, size);
                left = new Rectangle(picRect.X, picRect.Y, size, picRect.Height);
                right = new Rectangle(picRect.X + picRect.Width - size, picRect.Y, size, picRect.Height);
                if(other.getRectangle().Intersects(top))
                    b.setMoveDown(false);
                if(other.getRectangle().Intersects(bottom))
                    b.setMoveUp(false);
                if (other.getRectangle().Intersects(left))
                    b.setMoveRight(false);
                if (other.getRectangle().Intersects(right))
                    b.setMoveLeft(false);

            }
        }
    }
}
