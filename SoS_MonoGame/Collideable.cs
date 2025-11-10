using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoS
{
    public abstract class Collideable
    {
        protected Rectangle picRect;
        protected Texture2D pic;
        protected Vector2 pos;
        protected float rotation;
        protected float scale;
        protected Vector2 origin;

        public virtual bool collides(Collideable other)
        {
            return this.getRectangle().Intersects(other.getRectangle());
        }

        public abstract void collidedWith(Collideable other);

        public virtual Rectangle getRectangle()
        {
            return picRect;
        }

        public virtual Rectangle getBoundRect()
        {
            return picRect;
        }

        public virtual Matrix getMatrix()
        {
            return Matrix.Identity;
        }

        public Point getQuad(Collideable o)
        {
            Point pos = new Point(o.getRectangle().X, o.getRectangle().Y);
            int numQuads = 100;
            return new Point(pos.X/100, pos.Y/100);
        }

        public virtual void setRotation(float r)
        {
            rotation = r;
        }

        public virtual Point getPoint()
        {
            return new Point((int)pos.X, (int)pos.Y);
        }

        public virtual bool intersects(Rectangle rect)
        {
            return this.getRectangle().Intersects(rect);
        }
    }
}
