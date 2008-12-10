using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    public abstract class Obstacle
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
        public virtual Rectangle getRectangle()
        {
            return new Rectangle(x, y, sprite.Width, sprite.Height);
        }
        public virtual bool intersects(Rectangle r)
        {
            return getRectangle().Intersects(r);
        }
    }
}
