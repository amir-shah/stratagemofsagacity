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

    }
}
