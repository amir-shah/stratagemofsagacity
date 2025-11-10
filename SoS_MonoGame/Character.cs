using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SoS
{
    // Unused legacy class - kept for compatibility
    class Character : Being
    {
        public Character(Texture2D texture) : base(0, 0, texture, 1.0f)
        {
            picRect = new Rectangle(0, 0, 50, 50);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, picRect, Color.Chocolate);
        }
    }
}
