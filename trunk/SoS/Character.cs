using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SOS
{
    class Character : Being
    {
        public Character()
        {
            Content.RootDirectory = "Content";

            pic = Content.Load<Texture2D>("WizardSquare");
            picRect = new Rectangle(0, 0, 50, 50);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, picRect, Color.Chocolate);
        }
    }
}
