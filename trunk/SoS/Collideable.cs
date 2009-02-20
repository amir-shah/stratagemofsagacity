using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SoS
{
    public abstract class Collideable : Object
    {
        protected Rectangle picRect;

        public abstract bool collides(Collideable other);
        public abstract void collidedWith(Collideable other);

        public virtual Rectangle getRectangle()
        {
            return picRect;
        }
        public Point getQuad(Collideable o)
        {
            Point pos = new Point(o.getRectangle().X, o.getRectangle().Y);
            int numQuads = 100;
            return new Point(pos.X/100, pos.Y/100);
        }

            

    }
}
